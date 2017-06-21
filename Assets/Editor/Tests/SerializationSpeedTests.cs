using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using Timer = System.Diagnostics.Stopwatch;

public class SerializationSpeedTests {

  [Test]
  // came to about 1024000
  public void Enumerable_Filled_Array_Memory() {
    long memUsed = GC.GetTotalMemory(false);
    Debug.Log("mem before so many ve3cs: " + memUsed);
    var vec3s = Enumerable.Repeat<SerialVector3>(new SerialVector3{x=0,y=0,z=0}, 100000).ToArray();
    long memUsedAfter = GC.GetTotalMemory(false);
    Debug.Log("100000 new SerialVector3s took this many bytes: " + (memUsedAfter - memUsed));
  }



  [Test]
  // usually comes to about 1220608 bytes ?
  public void Array_Instantiation_Memory_Usage() {
    long memUsed = GC.GetTotalMemory(false);
    Debug.Log("mem before so many serial_ve3cs: " + memUsed);
    var vec3s = new SerialVector3[100000];
    long memUsedAfter = GC.GetTotalMemory(false);
    Debug.Log("100000 new SerialVector3s took this many bytes: " + (memUsedAfter - memUsed));
  }

  [TestCase(1000)]
  [TestCase(5000)]
  [TestCase(10000)]
  [TestCase(50000)]
  [TestCase(100000)]
  public void Time_Scaling(int count) {

    var container = ScriptableObject.CreateInstance<DataContainer>();

    container.vectors = Enumerable.Repeat<SerialVector3>
      (new SerialVector3{x=0f,y=0f,z=0f}, count).ToArray();

    Vector3 v3 = new Vector3(0f,0f,0f);

    for (int i = 0; i < count - 1; i++)
    {
      float xValue = i / 100f;
      float yValue = i / 500f;
      v3.x = xValue;
      v3.y = yValue;
      container.vectors[i].x = v3.x;  
      container.vectors[i].y = v3.y;
    }

    var timer = new Timer();
    timer.Start();
    AssetDatabase.CreateAsset(container, "Assets/vec3s-timed-" + count + ".asset");
    timer.Stop();
    Debug.Log(count + " objects - asset creation time: " + timer.ElapsedMilliseconds);

    timer.Start();
    AssetDatabase.SaveAssets();
    timer.Stop();
    Debug.Log(count + " objects - asset creation + save time: " + timer.ElapsedMilliseconds);

    timer.Reset();
    timer.Start();
    var json = EditorJsonUtility.ToJson(container);
    timer.Stop();
    Debug.Log(count + " objects - to JSON time: " + timer.ElapsedMilliseconds);

    timer.Start();
    string path = AssetDatabase.GenerateUniqueAssetPath("Assets/jsonspeed-" +count + ".json");
    File.WriteAllText(path, json);
    Debug.Log(count + " objects - JSON serialization + write time: " + timer.ElapsedMilliseconds);
    timer.Stop();
  }

  [TestCase(1000)]
  [TestCase(5000)]
  [TestCase(10000)]
  [TestCase(50000)]
  public void Time_Scaling_Json_Stream(int count) {

    var container = ScriptableObject.CreateInstance<DataContainer>();

    //var jsonStreamer = new JsonStreamSerializer<SerialVector3>();

    container.vectors = Enumerable.Repeat<SerialVector3>
      (new SerialVector3{x=0f,y=0f,z=0f}, count).ToArray();

    Vector3 v3 = new Vector3(0f,0f,0f);

    for (int i = 0; i < count - 1; i++)
    {
      float xValue = i /  100f;
      float yValue = i / 500f;
      v3.x = xValue;
      v3.y = yValue;
      container.vectors[i].x = v3.x;  
      container.vectors[i].y = v3.y;
      //jsonStreamer.Add(container.vectors[i]);
    }

    var timer = new Timer();
    timer.Start();
    AssetDatabase.CreateAsset(container, "Assets/vec3s-timed-" + count + ".asset");
    timer.Stop();
    Debug.Log(count + " objects - asset creation time: " + timer.ElapsedMilliseconds);

    timer.Start();
    AssetDatabase.SaveAssets();
    timer.Stop();
    Debug.Log(count + " objects - asset creation + save time: " + timer.ElapsedMilliseconds);

    timer.Reset();
    timer.Start();
    var json = EditorJsonUtility.ToJson(container);
    timer.Stop();
    Debug.Log(count + " objects - to JSON time: " + timer.ElapsedMilliseconds);

    timer.Start();
    string path = AssetDatabase.GenerateUniqueAssetPath("Assets/jsonspeed-" +count + ".json");
    File.WriteAllText(path, json);
    Debug.Log(count + " objects - JSON serialization + write time: " + timer.ElapsedMilliseconds);
    timer.Stop();
  }

  //private IEnumerator RunSaveAsset(DataContainer container, int count)
  //{
  //  AssetDatabase.CreateAsset(container,  "Assets/vec3s-timed-" + count + ".asset");
  //}

}

[Serializable]
// mutable structs are good actually 
// (in this case where we need to copy values without dynamic allocation)
public struct SerialVector3
{
  public float x;
  public float y;
  public float z;
}


