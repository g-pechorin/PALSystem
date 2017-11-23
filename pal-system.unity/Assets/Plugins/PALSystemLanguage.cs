using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PALSystemLanguage : ScriptableObject
{
	[System.Serializable]
	public struct Rule
	{
		public string pattern;
		public string results;

		public Rule(string pattern, string results)
		{
			this.pattern = pattern;
			this.results = results;
		}
	};

	[SerializeField]
	[HideInInspector]
#if UNITY_EDITOR
	public
#else
	private
#endif
		PALSystemDictionary dictionary = null;

	public PALSystemDictionary Dictionary { get { return dictionary; } }

	[SerializeField]
	[HideInInspector]
	public Rule[] rules = new Rule[0];

#if UNITY_EDITOR
	public string this[string key]
	{
		set
		{
			ArrayUtility.Add(ref rules, new Rule(key, value));
		}
	}

	[MenuItem("Assets/Create/PALSystem/Language")]
	public static void Create()
	{
		Assets.CreateScriptableObjectFile<PALSystemLanguage>();
	}

	[CustomEditor(typeof(PALSystemLanguage))]
	public class Inspector : Editor
	{
		PALSystemLanguage palSystemLanguage { get { return target as PALSystemLanguage; } }

		private string newPattern = "", newResults = "";
		private bool showRules = true;
		private bool showNewRule = true;
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			// dictionary assignment
			{
				var oldDictionary = palSystemLanguage.dictionary;
				var newDictionary = EditorGUILayout.ObjectField(oldDictionary, typeof(PALSystemDictionary), false) as PALSystemDictionary;

				if (oldDictionary != newDictionary)
				{
					Undo.RecordObject(palSystemLanguage, "Change on `" + palSystemLanguage.name + "`");
					palSystemLanguage.dictionary = newDictionary;
					EditorUtility.SetDirty(palSystemLanguage);
				}

				if (null == palSystemLanguage.dictionary)
				{
					EditorGUILayout.TextArea("Bro!\nYou must assign a dictionary");
					return;
				}
			}

			// list of rules
			if (showRules = EditorGUILayout.Foldout(showRules, "Rules"))
				for (int i = 0; i < palSystemLanguage.rules.Length; ++i)
				{
					GUILayout.BeginHorizontal();

					{
						var oldPattern = palSystemLanguage.rules[i].pattern;
						var newPattern = EditorGUILayout.TextField(oldPattern);
						if (oldPattern != newPattern)
						{
							Undo.RecordObject(palSystemLanguage, "Change pattern `" + oldPattern + "` to `" + newPattern + "` in `" + palSystemLanguage.name + "` [" + i + "]");
							palSystemLanguage.rules[i].pattern = newPattern;
							EditorUtility.SetDirty(palSystemLanguage);
						}
					}

					{
						var oldResults = palSystemLanguage.rules[i].results;
						var newResults = EditorGUILayout.TextField(oldResults);
						if (oldResults != newResults)
						{
							Undo.RecordObject(palSystemLanguage, "Change results `" + oldResults + "` to `" + newResults + "` in `" + palSystemLanguage.name + "` [" + i + "]");
							palSystemLanguage.rules[i].results = newResults;
							EditorUtility.SetDirty(palSystemLanguage);
						}
					}

					if (GUILayout.Button("Drop"))
					{
						Undo.RecordObject(palSystemLanguage, "Drop rule  [" + i + "] in `" + palSystemLanguage.name + "` [" + i + "]");
						ArrayUtility.RemoveAt(ref palSystemLanguage.rules, i);
						--i;
						EditorUtility.SetDirty(palSystemLanguage);
					}
					GUILayout.EndHorizontal();
				}

			// 
			// new rule thingie
			if (showNewRule = EditorGUILayout.Foldout(showNewRule, "New Rule"))
			{
				newPattern = EditorGUILayout.TextField("New Pattern", newPattern);
				newResults = EditorGUILayout.TextField("New Results", newResults);
				if (GUILayout.Button("Add"))
					if ("" != newPattern && "" != newResults)
						palSystemLanguage[newPattern] = newResults;
			}
		}
	}
#endif
}
