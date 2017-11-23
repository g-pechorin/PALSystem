using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class TestPALSystem
{
	private static string testIterate(string axiom)
	{
		var objectA = new GameObject("A");
		var objectB = new GameObject("B");

		PALSystemDictionary dictionary = null;
		try
		{
			dictionary = ScriptableObject.CreateInstance<PALSystemDictionary>();

			dictionary['A'] = null;
			dictionary['B'] = null;

			PALSystemLanguage language = null;
			try
			{
				language = ScriptableObject.CreateInstance<PALSystemLanguage>();

				language["A"] = "AB";
				language["B"] = "A";

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
	public void TestIterate_A_AB()
	{
		Assert.AreEqual("AB", testIterate("A"));
	}

	[Test]
	public void TestIterate_ABA_AB()
	{
		Assert.AreEqual("ABA", testIterate("AB"));
	}

	[Test]
	public void TestIterate_ABAAB_ABA()
	{
		Assert.AreEqual("ABAAB", testIterate("ABA"));
	}

	[Test]
	public void TestIterate_ABAABABA_ABAAB()
	{
		Assert.AreEqual("ABAABABA", testIterate("ABAAB"));
	}

	[Test]
	public void TestIterate_ABAABABAABAAB_ABAABABA()
	{
		Assert.AreEqual("ABAABABAABAAB", testIterate("ABAABABA"));
	}

	[Test]
	public void TestIterate_ABAABABAABAABABAABABA_ABAABABAABAAB()
	{
		Assert.AreEqual("ABAABABAABAABABAABABA", testIterate("ABAABABAABAAB"));
	}

	[Test]
	public void TestIterate_ABAABABAABAABABAABABAABAABABAABAAB_ABAABABAABAABABAABABA()
	{
		Assert.AreEqual("ABAABABAABAABABAABABAABAABABAABAAB", testIterate("ABAABABAABAABABAABABA"));
	}
}
