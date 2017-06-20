using System;
using System.IO;
using System.Text;
using System.Linq;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public static class JsonStreamTestMenu
{
  [MenuItem("JSON Stream/test 10000 Vec3s")]
  public static void MenuTest_10k()
  {
    var stream = new JsonStreamSerializer<SerialVector3>(10000, 50);

    for (int i = 0; i < 10000; i++)
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

    stream.StartSaving();
  }

  [MenuItem("JSON Stream/test 50000 vec3s")]
  public static void MenuTest_50k()
  {
    var stream = new JsonStreamSerializer<SerialVector3>();

    for (int i = 0; i < 50000; i++)
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

    stream.StartSaving();
  }

  [MenuItem("JSON Stream/test 500k vec3s, 100 chunk")]
  public static void MenuTest_500k()
  {
    var stream = new JsonStreamSerializer<SerialVector3>(500000, 100);

    for (int i = 0; i < 500000; i++)
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

    stream.StartSaving();
  }

  [MenuItem("JSON Stream/test 10k vec3s, 20 chunk")]
  public static void MenuTest_10k_20chunk()
  {
    var stream = new JsonStreamSerializer<SerialVector3>(10000, 20);

    for (int i = 0; i < 10000; i++)
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

    stream.StartSaving();
  }

  [MenuItem("JSON Stream/test 10k vec3s, 10 chunk")]
  public static void MenuTest_10k_10chunk()
  {
    var stream = new JsonStreamSerializer<SerialVector3>(10000, 10);

    for (int i = 0; i < 10000; i++)
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

    stream.StartSaving();
  }

  [MenuItem("JSON Stream/test 10k vec3s, 100 chunk")]
  public static void MenuTest_10k_100chunk()
  {
    var stream = new JsonStreamSerializer<SerialVector3>(10000, 100);

    for (int i = 0; i < 10000; i++)
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

    stream.StartSaving();
  }

  [MenuItem("JSON Stream/test 10k vec3s, 50 chunk")]
  public static void MenuTest_10k_50chunk()
  {
    var stream = new JsonStreamSerializer<SerialVector3>(10000, 50);

    for (int i = 0; i < 10000; i++)
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

    stream.StartSaving();
  }


  private static SerialVector3[] MakeFakeVec3s(int count = 0)
  {
    SerialVector3[] buffer = new SerialVector3[count];

    for (int i = 0; i < count; i++)
    {
      var sv3 = new SerialVector3();
      float xValue = i /  100f;
      float yValue = i / 500f;
      float zValue = i / 1000f;
      sv3.x = xValue;
      sv3.y = yValue;
      sv3.z = zValue;
      buffer[i] = sv3;
    }

    return buffer;
  }
}


