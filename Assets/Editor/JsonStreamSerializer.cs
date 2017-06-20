using System;
using System.IO;
using System.Text;
using System.Linq;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Timer = System.Diagnostics.Stopwatch;


public class JsonStreamSerializer<T>: FileStream
{
  public int chunkSize;
  public Queue<T> queue;
  public List<int> chunkTimingRecords; 

  private Timer timer = new Timer();
  private Timer chunkTimer = new Timer();

  public JsonStreamSerializer(int queueLength = 10000, int chunkSize = 50) : 
    base(AssetDatabase.GenerateUniqueAssetPath("assets/streamedJSON"), FileMode.CreateNew) 
  {
    this.chunkSize = chunkSize;
    this.queue = new Queue<T>(queueLength);
    chunkTimingRecords = new List<int>();
    Debug.Log(Timer.Frequency + " timer ticks per second");

  }

  public void Add(T obj)
  {
    queue.Enqueue(obj);
  }
 
  public void WriteObjectChunkJson(Chunk<T> obj)
  {
    // add the comma to seperate chunks in the stream
    string json = JsonUtility.ToJson(obj) + ",";
    byte[] bytes = Encoding.UTF8.GetBytes(json);
    Write(bytes, 0, bytes.Length);
  }

  public void StartSaving()
  {
    Debug.Log("started saving stream!");
    timer.Start();
    EditorApplication.update += OnSerialize;
  }

  public void StopSaving()
  {
    timer.Stop();
    EditorApplication.update -= OnSerialize;
    Debug.Log("stopped saving stream!");
    Debug.Log("took this many milliseconds : " + timer.ElapsedMilliseconds);

    int averageChunkTicks = (int)chunkTimingRecords.Average();
    Debug.Log("saving chunks of size " + chunkSize + 
      " took an average of " + averageChunkTicks + " ticks");
  }

  public void OnSerialize()
  {
    if (queue.Count == 0)
      StopSaving();

    chunkTimer.Reset();
    chunkTimer.Start();

    var chunk = new Chunk<T>();
    chunk.data = queue.DequeueChunk(chunkSize).ToArray();

    WriteObjectChunkJson(chunk);
    chunkTimer.Stop();
    chunkTimingRecords.Add((int)chunkTimer.ElapsedTicks);
    Debug.Log("wrote chunk of size " + chunkSize + 
      " to disk in " + chunkTimer.ElapsedTicks + " ticks");
  }
}

public static class QueueExtensions
{
  public static IEnumerable<T> DequeueChunk<T>(this Queue<T> queue, int chunkSize) 
  { 
    for (int i = 0; i < chunkSize && queue.Count > 0; i++)
    {
      yield return queue.Dequeue();
    }
  }
}


