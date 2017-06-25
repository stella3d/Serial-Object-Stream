using System;
using System.Collections.Generic;

public static class QueueExtensions
{
  public static IEnumerable<T> DequeueChunk<T>(this Queue<T> queue, int chunkSize) 
  { 
    for (int i = 0; i < chunkSize && queue.Count > 0; i++)
    {
      yield return queue.Dequeue();
    }
  }

	public static void DequeueChunkTo<T>(this Queue<T> queue, T[] targetArray, int chunkSize) 
	{ 
		for (int i = 0; i < chunkSize; i++) 
		{
			if (queue.Count > 0)
				targetArray [i] = queue.Dequeue ();
			else if (queue.Count == 0) 
			{
				Array.Clear (targetArray, i, chunkSize - i);
				break;
			}
		}
	}
}
