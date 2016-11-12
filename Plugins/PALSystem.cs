using UnityEngine;
using System.Collections.Generic;


public static class PALSystem
{
	public interface ISoar
	{
		void Soar(System.Random variance);
	}

	public class NoMatchingRule : UnityException
	{
		public NoMatchingRule(char wrong, PALSystemLanguage language) :
			base("No rule for `" + wrong + "` in `" + language.name + "`")
		{
		}
	}
	public class NoMatchingTerm : UnityException
	{
		public NoMatchingTerm(char wrong, PALSystemDictionary dictionary) :
			base("No term for `" + wrong + "` in `" + dictionary.name + "`")
		{
		}
	}

	public static string Iterate(System.Random variance, string input, PALSystemLanguage language)
	{
		if ("" == input)
			return "";

		if ('[' == input[0] || ']' == input[0])
			return input[0] + Iterate(variance, input.Substring(1), language);

		// find all rules that we can use here
		var ruleIndicies = new int[language.rules.Length];
		var ruleCount = 0;
		for (int i = 0; i < language.rules.Length; ++i)
			if (language.rules[i].pattern.Length <= input.Length && input.StartsWith(language.rules[i].pattern))
				ruleIndicies[ruleCount++] = i;

		if (0 == ruleCount)
			throw new NoMatchingRule(input[0], language);

		// choose an index
		var usedIndex = ruleIndicies[variance.Next(0, ruleCount)];

		// use the index
		return language.rules[usedIndex].results + Iterate(variance, input.Substring(language.rules[usedIndex].pattern.Length), language);
	}

	public static GameObject Substitude(System.Random variance, string input, PALSystemDictionary dictionary)
	{
		Debug.Assert(null != dictionary);
		Debug.Assert(0 < input.Length);

		GameObject root = new GameObject("PALSystem:Root");

		NodeTree<char>.Graph(input, c => '[' == c, c => ']' == c).visit(root, (parent, symbol) =>
		{
			Debug.Assert(parent == root || null != parent.GetComponent<PALSystemLeaf>());

			// make us!
			var self = Object.Instantiate(dictionary[symbol] as GameObject, (parent != root ? parent.GetComponentInChildren<PALSystemLeaf>().gameObject : parent).transform, false) as GameObject;

			// check we've got a single leaf
			Debug.Assert(1 == self.GetComponentsInChildren<PALSystemLeaf>().Length);

			// twist us
			foreach (var child in self.GetComponentsInChildren<ISoar>())
				child.Soar(variance);

			// return our leaf
			return self.GetComponentInChildren<PALSystemLeaf>().gameObject;
		});

		return root;
	}
}
