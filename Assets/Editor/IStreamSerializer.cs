using System.Collections.Generic;

internal interface IStreamSerializer<T>
{
  int chunkSize {get; set;} 

  bool stopWhenDrained {get; set;}

  Queue<T> queue {get; set;}

  void StartSaving();

  void StopSaving();

  void Add(T obj);
}


