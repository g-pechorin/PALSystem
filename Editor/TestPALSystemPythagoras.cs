using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class TestPALSystemPythagoras
{
	private static string testIterate(string axiom)
	{
		var objectA = new GameObject("A").AddComponent<PALSystemLeaf>().gameObject;
		var objectB = new GameObject("B").AddComponent<PALSystemLeaf>().gameObject;

		PALSystemDictionary dictionary = null;
		try
		{
			dictionary = ScriptableObject.CreateInstance<PALSystemDictionary>();

			//	dictionary['A'] = objectA;
			//	dictionary['B'] = objectB;

			PALSystemLanguage language = null;
			try
			{
				language = ScriptableObject.CreateInstance<PALSystemLanguage>();

				language.dictionary = dictionary;
				language["A"] = "AA";
				language["B"] = "A[B]B";

				return PALSystem.Iterate(new System.Random(3141983), axiom, language);
			}
			finally
			{
				Object.DestroyImmediate(language);
			}
		}
		finally
		{
			Object.DestroyImmediate(dictionary);
			Object.DestroyImmediate(objectA);
			Object.DestroyImmediate(objectB);
		}
	}

	[Test]
	public void TestPythagoras_A_AB()
	{
		Assert.AreEqual("A[B]B", testIterate("B"));
	}

	[Test]
	public void TestPythagoras_ABA_AB()
	{
		Assert.AreEqual("AA[A[B]B]A[B]B", testIterate("A[B]B"));
	}

	[Test]
	public void TestPythagoras_ABAAB_ABA()
	{
		Assert.AreEqual("AAAA[AA[A[B]B]A[B]B]AA[A[B]B]A[B]B", testIterate("AA[A[B]B]A[B]B"));
	}
}
