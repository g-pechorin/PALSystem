using UnityEngine;
using System.Collections;
using System;

public class PALSystemTwistYVary : MonoBehaviour, PALSystem.ISoar
{
	public MinMaxRange twistX = new MinMaxRange(0, 90);

	void PALSystem.ISoar.Soar(System.Random variance)
	{
		transform.localEulerAngles =
			new Vector3(
				(float)twistX.Random(variance),
				(float)variance.Next(0.0, 360.0),
				(float)0);
	}
}
