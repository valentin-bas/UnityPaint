using UnityEngine;
using System.Collections;
using UnityEditor;

public class Paint3D : EditorWindow
{
	static GameObject _sceneObject;

	// Add menu named "My Window" to the Window menu
	[MenuItem("Window/Paint3D")]
	static void Init()
	{
		// Get existing open window or if none, make a new one:
		Paint3D window = (Paint3D)EditorWindow.GetWindow(typeof(Paint3D));
		Paint3DComponent[] sceneObjects = FindObjectsOfType(typeof(Paint3DComponent)) as Paint3DComponent[];
		if (sceneObjects != null && sceneObjects.Length > 0)
			_sceneObject = sceneObjects[0].gameObject;
		else
			_sceneObject = null;
	}

	void OnGUI()
	{
		if (_sceneObject == null && GUILayout.Button("Add Paint3DSceneObject"))
		{
			_sceneObject = new GameObject("Paint3DSceneObject");
			_sceneObject.AddComponent<Paint3DComponent>();
		}
		else if (_sceneObject != null && GUILayout.Button("Remove Paint3DSceneObject"))
		{
			DestroyImmediate(_sceneObject);
			_sceneObject = null;
		}

		if (_sceneObject != null)
		{
			if (GUILayout.Button("Reset Texture"))
				_sceneObject.GetComponent<Paint3DComponent>().ResetTexture();
			if (GUILayout.Button("Save Texture"))
				_sceneObject.GetComponent<Paint3DComponent>().SaveTexture();

			GUILayout.Label("Settings", EditorStyles.boldLabel);
			//myString = EditorGUILayout.TextField("Text Field", myString);
			Raycast.colorActive = EditorGUILayout.ColorField("Active Color", Raycast.colorActive);

			//groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
			//myBool = EditorGUILayout.Toggle("Toggle", myBool);
			//myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
			//EditorGUILayout.EndToggleGroup();
		}
	}

}