using System;
using UnityEngine;
using System.Collections.Generic;

public class DataWrapper<T>: ScriptableObject
{
  public Queue<T> queue = new Queue<T>();
}

