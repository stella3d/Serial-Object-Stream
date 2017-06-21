using System;
using System.IO;
using System.Text;
using System.Linq;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Timer = System.Diagnostics.Stopwatch;

public class TestJsonStreamSerializer<T> : JsonStreamSerializer<T>
{
  // these are just for performance testing
  private Timer timer = new Timer();
  private Timer chunkTimer = new Timer();
  private List<int> chunkTimingRecords = new List<int>();

  public TestJsonStreamSerializer(int queueLength = 10000, int chunkSize = 50) 
    : base (queueLength, chunkSize)
  {
    stopWhenDrained = true;
  }

  new public void StartSaving()
  {
    Debug.Log("started saving stream...");
    timer.Start();

    base.StartSaving();
  }

  public override void StopSaving()
  {
    base.StopSaving();

    timer.Stop();
    Debug.Log("stream save done, total time in ms : " + timer.ElapsedMilliseconds);

    int averageTicks = (int)chunkTimingRecords.Average();
    int minTicks = chunkTimingRecords.Min();
    int maxTicks = chunkTimingRecords.Max();
    Debug.Log("chunks of size " + chunkSize + " saved in average of " + averageTicks + " ticks");
    Debug.Log("chunk save times in ticks - shortest: " + minTicks + " , longest: " + maxTicks);
  }

  internal override void SaveChunkOnUpdate()
  {
    chunkTimer.Reset();
    chunkTimer.Start();

    base.SaveChunkOnUpdate();

    chunkTimer.Stop();
    chunkTimingRecords.Add((int)chunkTimer.ElapsedTicks);
  }

}


