﻿using System;
using System.IO;
using System.Text;
using System.Linq;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class JsonStreamSerializer<T>: FileStream, IStreamSerializer<T>
{
  public int chunkSize {get; set;}
  public bool stopWhenDrained {get; set;}

  public Queue<T> queue {get; set;}

  private StreamWriter sw;

  public JsonStreamSerializer(int queueLength = 10000, int chunkSize = 50) : 
    base(AssetDatabase.GenerateUniqueAssetPath("assets/streamedJSON"), FileMode.CreateNew) 
  {
    this.chunkSize = chunkSize;
    this.queue = new Queue<T>(queueLength);
    stopWhenDrained = false;
    sw = new StreamWriter(this);
    InitJsonStream();
  }

  // waiting until we need to record something to make the first write slows it down
  // so here, go ahead & start the json array when we init to have stream ready
  private void InitJsonStream()
  {
    sw.WriteLine('[');
  }

  private void EndJsonStream()
  {
    sw.Write(']');
    sw.Flush();
    Flush();
    EditorApplication.update -= EndJsonStream;
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
    EditorApplication.update += EndJsonStream;
  }

  // we do allocate some memory for strings here, within JsonUtility, 
  // but it doesn't seem problematic enough to actually cause hitching
  // this writes to the OS file buffer , not disk directly - OS flushes to disk.
  internal void WriteChunkJson(Chunk<T> chunk)
  {
    sw.Write(JsonUtility.ToJson(chunk));
    // add the comma to seperate objects in the stream array
    sw.WriteLine(',');
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
    // TODO - maybe preallocate chunks instead, but they're cheap lists of refs
    // TODO - don't write if less than 2/3 the buffer size ?
    var chunk = new Chunk<T>();
    // TODO - maybe more optimized if array-to-array copy ?
    chunk.data = queue.DequeueChunk(chunkSize).ToArray();

    WriteChunkJson(chunk);
  }
}




