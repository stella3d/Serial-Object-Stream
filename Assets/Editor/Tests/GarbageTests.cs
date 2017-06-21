using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using Timer = System.Diagnostics.Stopwatch;

/*
public class SuperSerial {

  //Vector3 vec3 = new Vector3(0,0,0);
  //Vector3[] vectorArray = new Vector3[20000];

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
  public void Array_Instantiation_Memory() {
    long memUsed = GC.GetTotalMemory(false);
    Debug.Log("mem before so many serial_ve3cs: " + memUsed);
    var vec3s = new SerialVector3[100000];
    long memUsedAfter = GC.GetTotalMemory(false);
    Debug.Log("100000 new SerialVector3s took this many bytes: " + (memUsedAfter - memUsed));
  }

  [Test]
  public void SuperSerialShallowCopy() {
    
    var vec3s = Enumerable.Repeat<SerialVector3>
      (new SerialVector3{x=0f,y=0f,z=0f}, 100000).ToArray();

    Vector3 v3 = new Vector3(0f,0f,0f);

    for (int i = 0; i < 99999; i++)
    {
      float xValue = i / 100f;
      float yValue = i / 500f;

      v3.x = xValue;
      v3.y = yValue;

      vec3s[i].x = v3.x;  
      vec3s[i].y = v3.y;
    }

    Debug.Log("this should be x:5.0, y:1.0");
    Debug.Log("x: " + vec3s[500].x + " , y: " + vec3s[500].y);

    var container = ScriptableObject.CreateInstance<DataContainer>();
    container.vectors = vec3s;
    File.WriteAllText("vec3s.json", JsonUtility.ToJson(container));
    AssetDatabase.CreateAsset(container, "Assets/vec3s.asset");
    AssetDatabase.SaveAssets();
  }

  [Test]
  // came to about 102400
  public void SuperSerialShallowCopy_UsingClass() {

    var vec3s = Enumerable.Repeat<SerialVector3Class>(new SerialVector3Class{x=0,y=0,z=0}, 100000).ToArray();

    Vector3 v3 = new Vector3(0,0,0);

    for (int i = 0; i < 99999; i++)
    {
      float xValue = i / 100f;
      float yValue = i / 500f;

      v3.x = xValue;
      v3.y = yValue;

      vec3s[i].x = v3.x;  
      vec3s[i].y = v3.y;
    }

    Debug.Log("this should be x:5.0, y:1.0");
    Debug.Log("x: " + vec3s[500].x + " , y: " + vec3s[500].y);

  }
        
}




[Serializable]
public class SerialVector3Class
{
  public float x;
  public float y;
  public float z;
}
*/
