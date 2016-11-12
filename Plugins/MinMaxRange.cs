using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class MinMaxRange
{
	public float min;
	public float max;

	public MinMaxRange(float min, float max)
	{
		this.min = min;
		this.max = max;
	}

	public MinMaxRange()
	{
		min = 0;
		max = 1;
	}

	public double Random(System.Random variance)
	{
		return min + (variance.NextDouble() * (max - min));
	}
}