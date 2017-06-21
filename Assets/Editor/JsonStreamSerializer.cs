using System;
using System.IO;
using System.Text;
using System.Linq;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Timer = System.Diagnostics.Stopwatch;


public class JsonStreamSerializer<T>: FileStream, IStreamSerializer<T>
{
  public int chunkSize {get; set;}
  public bool stopWhenDrained {get; set;}

  public Queue<T> queue {get; set;}

  public JsonStreamSerializer(int queueLength = 10000, int chunkSize = 50) : 
    base(AssetDatabase.GenerateUniqueAssetPath("assets/streamedJSON"), FileMode.CreateNew) 
  {
    this.chunkSize = chunkSize;
    this.queue = new Queue<T>(queueLength);
    stopWhenDrained = false;
  }

  public void Add(T obj)
  {
    queue.Enqueue(obj);
  }

  public void StartSaving()
  {
    EditorApplication.update += SaveChunkOnUpdate;
  }

  public virtual void StopSaving()
  {
    EditorApplication.update -= SaveChunkOnUpdate;
    Flush();
  }

  // we do allocate some memory for strings here, 
  // but it doesn't seem problematic enough to actually cause hitching
  // TODO - consider seeing if using a StringBuilder will allow us to reduce allocations
  internal void WriteChunkJson(Chunk<T> chunk)
  {
    // add the comma to seperate chunks in the stream - 
    // TODO consider using linebreaks and reading by line instead
    string json = JsonUtility.ToJson(chunk) + ",";
    byte[] bytes = Encoding.UTF8.GetBytes(json);
    Write(bytes, 0, bytes.Length);
  }

  internal virtual void SaveChunkOnUpdate()
  {
    if (queue.Count == 0)
    {
      if (stopWhenDrained)
        StopSaving();
      else 
        return; 
    }
    // TODO - preallocate chunks instead, but they're cheap
    var chunk = new Chunk<T>();
    // TODO - maybe more optimized if array-to-array copy ?
    chunk.data = queue.DequeueChunk(chunkSize).ToArray();

    WriteChunkJson(chunk);
  }
}




