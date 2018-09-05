using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System.Collections.Generic;

public class PokemonsEditorWindow : EditorWindow {
	private ReorderableList list;
	public IList pokemons ;
	public TypeID type1 = new TypeID();
	public TypeID type2 = new TypeID();

	[MenuItem ("RPG Creator/Pokemons")]
	static void Init () {
		// Get existing open window or if none, make a new one:
		PokemonsEditorWindow window = (PokemonsEditorWindow)EditorWindow.GetWindow (typeof (PokemonsEditorWindow));
		window.minSize = new Vector2(32 * 2,32);
		window.name = ("Pokemons");
		window.Show();
	}
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void Awake()
	{
		//PokemonsEditorWindowScriptableObject scriptableObject = ScriptableObjectUtility.LoadAsset<ToolBoxScriptableObject>("Assets/DataBases/.asset");
	}
	
	void OnEnable()
	{
		pokemons = new List<PokemonEditor>();
		PokemonEditor poke0 = new PokemonEditor();
		pokemons.Insert( pokemons.Count, poke0);
		PokemonEditor poke1 = new PokemonEditor();
		poke1.HPBase=1;
		pokemons.Insert( pokemons.Count, poke1);
		PokemonEditor poke2 = new PokemonEditor();
		poke2.HPBase=2;
		pokemons.Insert( pokemons.Count, poke2);
		list = new ReorderableList(pokemons,typeof(PokemonEditor));
		list.drawElementCallback =  
		(Rect rect, int index, bool isActive, bool isFocused) => {
			GUI.Label(rect,index.ToString());
			rect.x += 20;
			GUI.Label(rect,((PokemonEditor)pokemons[index]).HPBase.ToString());
			rect.x += 20;
			//var element = list.serializedProperty.GetArrayElementAtIndex(index);
			
			/*EditorGUI.PropertyField(
				new Rect(rect.x, rect.y, 60, EditorGUIUtility.singleLineHeight),
				element.FindPropertyRelative("Type"), GUIContent.none);
			EditorGUI.PropertyField(
				new Rect(rect.x + 60, rect.y, rect.width - 60 - 30, EditorGUIUtility.singleLineHeight),
				element.FindPropertyRelative("Prefab"), GUIContent.none);
			EditorGUI.PropertyField(
				new Rect(rect.x + rect.width - 30, rect.y, 30, EditorGUIUtility.singleLineHeight),
				element.FindPropertyRelative("Count"), GUIContent.none);*/
		};
	}
	
	void OnGUI () 
	{
		if(list == null)
		{
			pokemons = new List<PokemonEditor>();
			pokemons.Insert( pokemons.Count, new PokemonEditor());
			list = new ReorderableList(pokemons,typeof(PokemonEditor));
		}
		GUILayout.BeginHorizontal();
		GUILayout.BeginVertical();
		list.DoLayoutList();
		GUILayout.EndVertical();
		GUILayout.BeginVertical();
		GUILayout.Button("button");
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
	}
}
