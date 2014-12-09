using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Paint3DComponent))]
public class Paint3DComponentEditor : Editor
{
	private Vector2 _mousePos;
	//private bool _enable;
	private Vector3[] _pointsToDraw;
	private RaycastHit _lastHit;
	private GameObject _hittedGameObject;

	private void CallbackFunction()
	{
		if (Camera.current == null || _mousePos == null)
			return;

		RaycastHit hit;
		if (!Physics.Raycast(Camera.current.ScreenPointToRay(_mousePos), out hit))
			return;
		MeshCollider meshCollider = hit.collider as MeshCollider;
		if (meshCollider == null || meshCollider.sharedMesh == null)
			return;

		Mesh mesh = meshCollider.sharedMesh;
		Vector3[] vertices = mesh.vertices;
		int[] triangles = mesh.triangles;
		Vector3 p0 = vertices[triangles[hit.triangleIndex * 3 + 0]];
		Vector3 p1 = vertices[triangles[hit.triangleIndex * 3 + 1]];
		Vector3 p2 = vertices[triangles[hit.triangleIndex * 3 + 2]];
		Transform hitTransform = hit.collider.transform;
		_pointsToDraw = new Vector3[3];
		_pointsToDraw[0] = hitTransform.TransformPoint(p0);
		_pointsToDraw[1] = hitTransform.TransformPoint(p1);
		_pointsToDraw[2] = hitTransform.TransformPoint(p2);
		_lastHit = hit;
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		//ObjectBuilderScript myScript = (ObjectBuilderScript)target;
		//if (!_enable && GUILayout.Button("Enable"))
		//	_enable = true;
		//else if (_enable && GUILayout.Button("Disable"))
		//	_enable = false;
	}

	void OnSceneGUI()
	{
		Event current = Event.current;
		int controlID = GUIUtility.GetControlID(GetHashCode(), FocusType.Passive);

		switch (current.type)
		{
			case EventType.mouseMove:
				//if (!_enable)
				//	break;
				_mousePos = Event.current.mousePosition;
				_mousePos.y = Screen.height - (_mousePos.y + 35);
				CallbackFunction();
				_LauchRay();
				current.Use();
				break;

			case EventType.mouseDown:
				_LauchRay();
				break;

			case EventType.layout:
				HandleUtility.AddDefaultControl(controlID);
				break;
		}
		if (_pointsToDraw != null)
		{
			Handles.color = Color.green;
			Handles.DrawLine(_pointsToDraw[0], _pointsToDraw[1]);
			Handles.DrawLine(_pointsToDraw[1], _pointsToDraw[2]);
			Handles.DrawLine(_pointsToDraw[2], _pointsToDraw[0]);
		}
		if (GUI.changed)
			EditorUtility.SetDirty(target);
	}

	private void _LauchRay()
	{
		Texture2D tex = Raycast.GetHittedTexture(_lastHit);
		Paint3DComponent component = target as Paint3DComponent;
		if (tex != null && component != null &&
			(component.ActualTexture != tex || component.OldTexture == tex))
		{
			component.OldTexture = tex;
			component.ActualTexture = new Texture2D(tex.width, tex.height);
			component.ActualTexture.SetPixels(tex.GetPixels());
			component.ActualTexture.Apply();
			_hittedGameObject = Raycast.GetHittedMeshGameObject(_lastHit);
			if (_hittedGameObject != null)
				_hittedGameObject.renderer.sharedMaterial.SetTexture(0, component.ActualTexture);
			component.AttachedGameObject = _hittedGameObject;
		}
		Raycast.ComputeHit(_lastHit, Event.current.button, component.ActualTexture);
	}

	void OnEnable()
	{
		//_enable = false;
		_pointsToDraw = null;
		//EditorApplication.update += CallbackFunction;
	}

	void OnDisable()
	{
		//EditorApplication.update -= CallbackFunction;
	}
}