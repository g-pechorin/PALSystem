using UnityEngine;
using System.Collections;
using System;

public class PALSystemScaleVary : MonoBehaviour, PALSystem.ISoar
{
	public MinMaxRange scaling = new MinMaxRange();

	void PALSystem.ISoar.Soar(System.Random variance)
	{
		transform.localScale = Vector3.one * (float)scaling.Random(variance);
	}
}
