using System;

[Serializable]
public class Chunk<T>
{
	public T[] data;

	public Chunk (int size)
	{
		this.data = new T[size];
	}
		
}



