using System;
using System.IO;
using System.Text;
using System.Linq;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Timer = System.Diagnostics.Stopwatch;

public static class JsonStreamTestMenu
{
  [MenuItem("JSON Stream/test 10000 Vec3s")]
  public static void MenuTest_10k()
  {
    var stream = new TestJsonStreamSerializer<SerialVector3>(10000, 50);
    MakeFakeVec3s(stream, 10000);

    stream.StartSaving();
  }

  [MenuItem("JSON Stream/test 50000 vec3s")]
  public static void MenuTest_50k()
  {
    var stream = new TestJsonStreamSerializer<SerialVector3>(50000, 50);
    MakeFakeVec3s(stream, 50000);

    stream.StartSaving();
  }

  [MenuItem("JSON Stream/test 200k vec3s, 100 chunk")]
  public static void MenuTest_200k()
  {
    var stream = new TestJsonStreamSerializer<SerialVector3>(200000, 100);
    MakeFakeVec3s(stream, 200000);

    stream.StartSaving();
  }

  [MenuItem("JSON Stream/test 10k vec3s, 20 chunk")]
  public static void MenuTest_10k_20chunk()
  {
    var stream = new TestJsonStreamSerializer<SerialVector3>(10000, 20);
    MakeFakeVec3s(stream, 10000);

    stream.StartSaving();
  }

  [MenuItem("JSON Stream/test 10k vec3s, 10 chunk")]
  public static void MenuTest_10k_10chunk()
  {
    var stream = new TestJsonStreamSerializer<SerialVector3>(10000, 10);
    MakeFakeVec3s(stream, 10000);

    stream.StartSaving();
  }

  [MenuItem("JSON Stream/test 10k vec3s, 100 chunk")]
  public static void MenuTest_10k_100chunk()
  {
    var stream = new TestJsonStreamSerializer<SerialVector3>(10000, 100);
    MakeFakeVec3s(stream, 10000);

    stream.StartSaving();
  }

  [MenuItem("JSON Stream/test 10k vec3s, 50 chunk")]
  public static void MenuTest_10k_50chunk()
  {
    var stream = new TestJsonStreamSerializer<SerialVector3>(10000, 50);
    MakeFakeVec3s(stream, 10000);
      
    stream.StartSaving();
  }

  // min :     70,  70,  70,  70, 70
  // max :     300, 330, 230, 460, 290
  // average : 90,  98,  101, 102, 104
  [MenuItem("JSON Stream/GetChunk speed - size 50")]
  public static void MenuTest_5k_50_GetChunk_PreInit()
  {
    TestGetChunkSpeed(5000, 50);
  }

	// min :     69,70, 70
	// max :     83520, 100415
	// average : 128, 115, 109														-- 118 global average
	[MenuItem("JSON Stream/GetChunk speed - 500k, 50")]
	public static void MenuTest_500k_50_GetChunk_PreInit()
	{
		TestGetChunkSpeed(500000, 50);
	}

	// NOTE : max without the first one taking way longer is ~90 ticks
	// 4500 / 100 = 45, so it's adding ~45 to the average of 100 repeats
	// so, average without that looks to be more like 40-45 ticks
	// min :     25,   25, 25
	// max :     4575, 4652
	// average : 80,   87,  79.2
	[MenuItem("JSON Stream/CopyChunk speed - 50")]
	public static void MenuTest_5k_50_CopyChunk()
	{
		TestCopyChunkSpeed(50000, 50);
	}

	// min :     25,  22, 
	// max :     4464, 3618, (just the first one)
	// average : 30,  30
	[MenuItem("JSON Stream/CopyChunk speed - 500k, 50")]
	public static void MenuTest_500k_50_CopyChunk()
	{
		TestCopyChunkSpeed(500000, 50);
	}

  // min :     40, 40, 40, 40, 40
  // max :     170, 310, 150, 110, 110
  // average : 64, 57, 55, 52, 53 
  [MenuItem("JSON Stream/GetChunk speed - size 20")]
  public static void MenuTest_2k_20_GetChunk_PreInit()
  {
    TestGetChunkSpeed(2000, 20);
  }

	// min :     10, 10
	// max :     4281,	550						(always just the first one)
	// average : 14, 14 
	[MenuItem("JSON Stream/CopyChunk speed - 200k, 20")]
	public static void MenuTest_200k_20_CopyChunk()
	{
		TestCopyChunkSpeed(200000, 20);
	}

  // min :     30, 30, 20, 30, 30
  // max :     100, 190, 180, 80, 330
  // average : 42, 42, 39, 38, 47
  [MenuItem("JSON Stream/GetChunk speed - size 10")]
  public static void MenuTest_1k_10_GetChunk_PreInit()
  {
    TestGetChunkSpeed(1000, 10);
  }

  // ALL CHUNK TIMINGS (so far) ON MID-2015 15" MBP - max spec

  // min :     1800, 1730, 1760, 1720,  1730
  // max :     6160, 4570, 3650, 73790, 3200
  // average : 2260, 2171, 2063, 2847,  2192
  [MenuItem("JSON Stream/WriteChunkJson speed - pre, 100 chunk")]
  public static void MenuTest_10k_100chunk_writechunkjson_preinit()
  {
    TestWriteChunkJsonSpeed(10000, 100);
  }


  // so, more buffer means slightly faster average but higher max ?
  // with 4kb string buffers:
  // min :     910,  910,  910,  920, 930
  // max :     1740, 1930, 2330, 2200, 2320 
  // average : 1120, 1150, 1200, 1212, 1105     -- 1157.4
  // with 8kb stream buffers:
  // min :     590,  590, 600
  // max :     2610, 2500, 2440
  // average : 1167, 1058, 1079                 -- 1101.333
  // with 16kb stream buffers:
  // min :     580, 580, 590
  // max :     2840, 3080, 3600
  // average : 1047, 1074, 1103
  // with 64kb stream buffers:
  // min :     590,  590,  590
  // max :     8450, 8780, 8640
  // average : 1046, 1031, 1023
  [MenuItem("JSON Stream/WriteChunkJson speed - pre, 50 chunk")]
  public static void MenuTest_5k_50chunk_writechunkjson_preinit()
  {
    TestWriteChunkJsonSpeed(5000, 50);
  }

  // with 4kb stream buffers:
  // min :     400,  390,  390,  400, 390
  // max :     3690, 1150, 3570, 1130, 3630
  // average : 552,  527,  533,  547, 527
  // with 8kb stream buffers:
  // min :     290,  290, 290
  // max :     2700, 1640, 3170
  // average : 530,  537, 542
  // with 64kb stream buffers:
  // min :     280,  290, 290
  // max :     9680, 8560, 9090
  // average : 520,  496, 478
  [MenuItem("JSON Stream/WriteChunkJson speed - pre, 20 chunk")]
  public static void MenuTest_2k_20chunk_writechunkjson_preinit()
  {
    TestWriteChunkJsonSpeed(5000, 20);
  }

  // min :     190, 180, 190, 190, 200
  // max :     3720, 1320, 3550, 3820, 3840
  // average : 338, 315, 331, 341, 347
  [MenuItem("JSON Stream/WriteChunkJson speed - pre, 10 chunk")]
  public static void MenuTest_5k_10chunk_writechunkjson_preinit()
  {
    TestWriteChunkJsonSpeed(5000, 10);
  }

  // min :     140, 150, 140, 140, 140 
  // max :     4010, 650, 680, 590, 3910
  // average : 249, 232, 238, 231, 254
  // with 64kb stream buffers:
  // min :     140,  130, 140
  // max :     7050, 7990, 7010
  // average : 218,  212, 208
  [MenuItem("JSON Stream/WriteChunkJson speed - pre, 5 chunk")]
  public static void MenuTest_1k_5chunk_writechunkjson_preinit()
  {
    TestWriteChunkJsonSpeed(1000, 5);
  }

  private static void TestWriteChunkJsonSpeed(int total, int chunkSize)
  {
    var realTotal = total + chunkSize;
    var stream = new TestJsonStreamSerializer<SerialVector3>(realTotal, chunkSize);
    MakeFakeVec3s(stream, realTotal);

    var timer = new Timer();
    var timings = new List<int>();

    // write one chunk first to see if pre-init helps
    stream.WriteChunkJson(stream.GetChunk());

    while (stream.queue.Count > 0)
    {
      var chunk = stream.GetChunk();
      timer.Start();

      stream.WriteChunkJson(chunk);

      timer.Stop();
      int time = (int)timer.ElapsedTicks;
      Debug.Log("write chunk json ticks: " + time);
      timings.Add(time);
      timer.Reset();
    }

    Debug.Log("WriteChunkJson (pre-init) timings, chunk size: " + chunkSize);
    PrintTimings(timings);
  }

  private static void TestGetChunkSpeed(int total, int chunkSize)
  {
    var stream = new TestJsonStreamSerializer<SerialVector3>(total, chunkSize);
    MakeFakeVec3s(stream, total);

    var timer = new Timer();
    var chunkList = new List<Chunk<SerialVector3>>();
    var timings = new List<int>();

    stream.InitQueueAccess();

    while (stream.queue.Count > 0)
    {
      timer.Start();

      chunkList.Add(stream.GetChunk());

      timer.Stop();
      int time = (int)timer.ElapsedTicks;
      Debug.Log("GetChunk ticks: " + time);
      timings.Add(time);
      timer.Reset();
    }

    Debug.Log("GetChunk (pre-init) timings, chunk size: " + chunkSize);
    PrintTimings(timings);
  }

	private static void TestCopyChunkSpeed(int total, int chunkSize)
	{
		var stream = new TestJsonStreamSerializer<SerialVector3>(total, chunkSize);
		MakeFakeVec3s(stream, total);

		var timer = new Timer();
		var timings = new List<int>();

		stream.InitQueueAccess();

		while (stream.queue.Count > 0)
		{
			timer.Start();

			stream.CopyChunk();

			timer.Stop();
			int time = (int)timer.ElapsedTicks;
			Debug.Log("CopyChunk (array copy) ticks: " + time);
			timings.Add(time);
			timer.Reset();
		}

		Debug.Log("CopyChunk (array copy) timings, chunk size: " + chunkSize);
		PrintTimings(timings);
	}

  private static void MakeFakeVec3s(IStreamSerializer<SerialVector3> stream, int count)
  {
    for (int i = 0; i < count; i++)
    {
      var sv3 = new SerialVector3();
      float xValue = i /  100f;
      float yValue = i / 500f;
      float zValue = i / 1000f;
      sv3.x = xValue;
      sv3.y = yValue;
      sv3.z = zValue;
      stream.Add(sv3);
    }
  }

  private static void PrintTimings(List<int> timings, string unit = "ticks")
  {
    Debug.Log("min : " + timings.Min() + " " + unit);
    Debug.Log("max : " + timings.Max() + " " + unit);
    Debug.Log("average : " + timings.Average() + " " + unit);
  }


	[MenuItem("JSON Stream/CopyChunk Allocations")]
	public static void CopyChunk_AllocationsPerCallback()
	{
		var stream = new TestJsonStreamSerializer<SerialVector3>(100000, 25);
		MakeFakeVec3s(stream, 100000);

		stream.InitQueueAccess();

		// if you comment out the line that serializes to json + writes to stream,
		// there are 0 allocations per frame added by this.
		stream.StartSaving ();
	}


}


