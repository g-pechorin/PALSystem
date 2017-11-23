using UnityEngine;
using System.Collections;

public class FieldBoy : MonoBehaviour
{
	public int count = 14;
	public float span = 3;

	void Start()
	{
		Destroy(this);
		var count2 = count * count;

		for (int i = -count; i <= count; ++i)
			for (int j = -count; j <= count; ++j)
				if (((i * i) + (j * j)) <= count2)
					if (0 != (i | j))
						(Instantiate(gameObject, transform.parent) as GameObject).transform.position = transform.position + (span * new Vector3(i, 0, j));
	}
}
