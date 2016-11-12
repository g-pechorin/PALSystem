using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PALSystemRoot : MonoBehaviour
{

	public int iterations = 3;
	public int seed;
	public string axiom = "A";
	public PALSystemLanguage language;

	public bool worldReSeed = false;

	void Reset()
	{
		seed = new System.Random().Next();
	}

	void Start()
	{
		Generate();
	}

	GameObject Generate()
	{
		var text = axiom;

		var variance = new System.Random(worldReSeed ? seed ^ transform.position.GetHashCode() : seed);

		for (int i = 0; i < iterations; ++i)
			text = PALSystem.Iterate(variance, text, language);

		var root = PALSystem.Substitude(variance, text, language.Dictionary);
		root.hideFlags = root.hideFlags | HideFlags.DontSaveInEditor | HideFlags.DontSave | HideFlags.DontSaveInBuild;
		root.transform.SetParent(transform, false);

		return root;
	}
#if UNITY_EDITOR
	[CustomEditor(typeof(PALSystemRoot))]
	public class Inspector : Editor
	{
		public PALSystemRoot Target { get { return target as PALSystemRoot; } }

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			if (Application.isPlaying)
				return;

			if (GUILayout.Button("Shuffle"))
			{
				Target.seed = new System.Random().Next();

				if (null != Target.GetComponentInChildren<EditJunk>())
				{
					DestroyImmediate(Target.GetComponentInChildren<EditJunk>().gameObject);
					Target.Generate().AddComponent<EditJunk>();
				}
			}

			if (GUILayout.Button("Preview"))
			{
				if (null != Target.GetComponentInChildren<EditJunk>())
				{
					DestroyImmediate(Target.GetComponentInChildren<EditJunk>().gameObject);
				}
				Target.Generate().AddComponent<EditJunk>().gameObject.hideFlags |= HideFlags.HideAndDontSave; ;
			}
		}

		class EditJunk : MonoBehaviour
		{
		}
	}
#endif
}
