using System;
using System.IO;
using System.Text;
using System.Linq;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class JsonStreamSerializer<T>: FileStream, IStreamSerializer<T> where T: new()
{
  public int chunkSize {get; set;}
  public bool stopWhenDrained {get; set;}

  public Queue<T> queue {get; set;}

  private StreamWriter sw;

	private Chunk<T> chunkBuffer;
	private int bufferIndex;

  public JsonStreamSerializer(int queueLength = 10000, int chunkSize = 50, int bufferKb = 8) : 
    base(
      AssetDatabase.GenerateUniqueAssetPath("assets/streamedJSON"), 
      FileMode.CreateNew, FileAccess.Write, FileShare.None, 1024 * bufferKb
    ) 
  {
    this.chunkSize = chunkSize;
    this.queue = new Queue<T>(queueLength);

		this.chunkBuffer = new Chunk<T> (chunkSize);
		this.bufferIndex = 0;

    stopWhenDrained = false;
    sw = new StreamWriter(this, Encoding.UTF8, 1024 * bufferKb);
    InitJsonStream();
  }

  // waiting until we need to record something to make the first write slows it down
  // so here, go ahead & start the json array when we init to have stream ready
  private void InitJsonStream()
  {
    sw.WriteLine('[');
    sw.Flush();
    Flush();
  }

  private void EndJsonStream()
  {
    // for if a chunk that isn't full hasn't been written at the end
		if (queue.Count > 0)
		{
			CopyChunk ();
			WriteChunkJson (chunkBuffer);
		}
    sw.Write(']');
    sw.Flush();
    Flush();
    EditorApplication.update -= EndJsonStream;
  }

  public void Add(T obj)
  {
    queue.Enqueue(obj);
  }

	// this is experimental, not ready
	public void Add(T obj, bool useChunkBufferDirectly)
	{
		if (useChunkBufferDirectly) 
		{
			if (bufferIndex < chunkSize) 
			{
				chunkBuffer.data[bufferIndex] = obj;
				bufferIndex++;
			}

			if (bufferIndex == chunkSize) 
			{
				// TODO - this might not work with having writing all done on another thread
				WriteChunkJson (chunkBuffer);
				Array.Clear (chunkBuffer.data, 0, chunkSize);
				bufferIndex = 0;
			}
		} 
		else
			Add (obj);
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
		
  // this writes to the OS file buffer , not disk directly - OS flushes to disk.
  internal void WriteChunkJson(Chunk<T> chunk)
  {
    sw.WriteLine(JsonUtility.ToJson(chunk));
  }

	// testing shows there's only one allocation made per callback, around 6.9 kb with size 50
	// and that's in ToJson, which is unavoidable using jsonutility as-is
	// TODO - look into adding support for writing the json string directly
	// to an existing char[] array to eliminate that allocation in the future
  internal virtual void SaveChunkOnUpdate()
  {
		if (queue.Count >= chunkSize) 
		{
			CopyChunk ();
			WriteChunkJson (chunkBuffer);
		}
    else if (queue.Count == 0)
    {
      if (!stopWhenDrained)
        return;
      else 
        StopSaving();
    }  
  }

	// will be removed soon, here for performance test reasons
  internal Chunk<T> GetChunk()
  {
		var chunk = new Chunk<T>(chunkSize);
    // TODO - maybe more optimized if array-to-array copy ?
		// answer - yes, 3-4 times faster with 11 less allocations (none) using array copy directly
    chunk.data = queue.DequeueChunk(chunkSize).ToArray();
    return chunk;
  }

	// this is 3-4 times faster than GetChunk<T> and allocates less - 
	internal void CopyChunk()
	{
		queue.DequeueChunkTo(chunkBuffer.data, chunkSize);
	}

  // first read access to the queue takes almost 2ms
  // but if we do this before, seems to help cache that cost
  // edit - maybe not ???
  internal void InitQueueAccess()
  {
    queue.Enqueue(new T());
    queue.Dequeue();
  }

}




