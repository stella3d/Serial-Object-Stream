﻿using System.Collections.Generic;

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
