using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PALSystemDictionary : ScriptableObject
{
	[SerializeField]
	[HideInInspector]
	public Term[] terms = new Term[0];

	[System.Serializable]
	public struct Term
	{
		public char symbol;
		public GameObject data;

		public Term(char symbol, GameObject data)
		{
			this.symbol = symbol;
			this.data = data;
		}
	}

	public GameObject this[char key]
	{
		get
		{
			for (int i = 0; i < terms.Length; ++i)
				if (key == terms[i].symbol)
					return terms[i].data;

			throw new System.IndexOutOfRangeException();
		}
#if UNITY_EDITOR
		set
		{
			if (null != value && value.activeInHierarchy)
				throw new CantSetThat("Must use a prefab");

			if (null != value && 1 != value.GetComponentsInChildren<PALSystemLeaf>().Length)
				throw new CantSetThat("Must have exactly one {PALSystemLeaf}");

			var indexOf = terms.HeadIndexOf(term => term.symbol == key);

			if (-1 != indexOf)
				terms[indexOf].data = value;
			else
				ArrayUtility.Add(ref terms, new Term(key, value));
		}
#endif
	}
#if UNITY_EDITOR

	class CantSetThat : System.Exception { public CantSetThat(string message) : base(message) { } }

	[MenuItem("Assets/Create/PALSystem/Dictionary")]
	public static void Create()
	{
		Assets.CreateScriptableObjectFile<PALSystemDictionary>();
	}

	[CustomEditor(typeof(PALSystemDictionary))]
	public class BuildPALInspector : Editor
	{
		PALSystemDictionary palSystemDictionary { get { return target as PALSystemDictionary; } }
		private string newTerm = "";
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			for (int i = 0; i < palSystemDictionary.terms.Length; ++i)
			{
				GUILayout.BeginHorizontal();

				// pattern chaging
				var named = palSystemDictionary.terms[i].symbol;
				{
					var rename = EditorGUILayout.TextField(named + "");

					if (rename.Length == 1 && rename[0] != named)
					{
						Undo.RecordObject(palSystemDictionary, "Rename `" + named + "`'s to `" + rename + "` in  `" + palSystemDictionary.name + "`");
						palSystemDictionary.terms[i].symbol = rename[0];
						EditorUtility.SetDirty(palSystemDictionary);
					}
				}

				// change a term's data
				{
					var oldValue = palSystemDictionary.terms[i].data;
					var newValue = EditorGUILayout.ObjectField(oldValue, typeof(GameObject), true) as GameObject;

					if (oldValue != newValue)
					{
						try
						{
							// try to set it
							palSystemDictionary[named] = newValue;

							try
							{
								// put it back, set undo, proper change it, finally mark it as diryt
								palSystemDictionary[named] = oldValue;
								Undo.RecordObject(palSystemDictionary, "Set the data for `" + named + "`'s to `" + newValue.name + "` in  `" + palSystemDictionary.name + "`");
								palSystemDictionary[named] = newValue;
								EditorUtility.SetDirty(palSystemDictionary);
							}
							catch (CantSetThat why)
							{
								throw new UnityException("Impossible", why);
							}
						}
						catch (CantSetThat why)
						{
							Debug.LogError(why.Message);
						}
					}
				}

				// drop button for a term
				if (GUILayout.Button("Drop"))
				{
					Undo.RecordObject(palSystemDictionary, "Drop `" + palSystemDictionary.terms[i].symbol + "`'s from `" + palSystemDictionary.name + "`");
					ArrayUtility.RemoveAt(ref palSystemDictionary.terms, i);
					--i;
					EditorUtility.SetDirty(palSystemDictionary);
				}

				GUILayout.EndHorizontal();
			}

			GUILayout.BeginHorizontal();
			{
				newTerm = EditorGUILayout.TextField(newTerm);
				newTerm = newTerm.Substring(0, Mathf.Min(1, newTerm.Length));

				var newObject = EditorGUILayout.ObjectField("Add", null, typeof(GameObject), true) as GameObject;

				if (null != newObject)
					if (newObject.activeInHierarchy)
						Debug.LogError("Need to use prefabs");
					else if (1 != newObject.GetComponentsInChildren<PALSystemLeaf>().Length)
						Debug.LogError("Need exactly one PALSystemLeaf on the prefab");
					else
					{
						if ("" != newTerm)
							if (!palSystemDictionary.terms.Has(term => term.symbol == newTerm[0]))
							{
								Undo.RecordObject(palSystemDictionary, "Add `" + newTerm[0] + "` to `" + palSystemDictionary.name + "`");
								palSystemDictionary[newTerm[0]] = newObject;
								EditorUtility.SetDirty(palSystemDictionary);
							}
					}
			}
			GUILayout.EndHorizontal();
		}
	}
#endif
}
