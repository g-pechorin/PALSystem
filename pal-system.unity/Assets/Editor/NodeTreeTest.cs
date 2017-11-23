using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class NodeTreeTest
{

	[Test]
	public void EditorTest1()
	{
		var e = NodeTree<char>.Root('a');
		e.Append('a').Append('c');
		e.Append('b');

		var a = NodeTree<char>.Root('a');
		a.Append('a').Append('c');
		a.Append('b');

		Assert.AreEqual(e, a);
	}

	[Test]
	public void EditorTest2()
	{
		var e = NodeTree<char>.Root('a');
		e.Append('a').Append('d');
		e.Append('c');

		var a = NodeTree<char>.Root('a');
		a.Append('a').Append('c');
		a.Append('b');

		Assert.AreNotEqual(e, a);
	}

	[Test]
	public void EditorTest3()
	{
		var e = NodeTree<char>.Root('a');
		e.Append('a').Append('d');
		e.Append('c');

		var a = NodeTree<char>.Graph("a[ad]c", o => o == '[', o => o == ']');

		Assert.AreEqual(e, a);
	}
}
