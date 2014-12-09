using UnityEngine;
using System.Collections;

public class Raycast : MonoBehaviour
{
	public bool PreviewTriangle = false;

	public static Color colorActive;
	private static Texture2D _tex;
	private static Vector2 _oldTexPoint;
	private static bool _oldTexPointSetted;

	void Start()
	{
		colorActive = Color.black;
		_tex = null;
		_oldTexPointSetted = false;
	}

	void Update()
	{
		if (!PreviewTriangle && !Input.GetMouseButton(1) && !Input.GetMouseButtonDown(2))
			return;
		RaycastHit hit;
		if (!Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
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
		Vector3 pt0 = hitTransform.TransformPoint(p0);
		Vector3 pt1 = hitTransform.TransformPoint(p1);
		Vector3 pt2 = hitTransform.TransformPoint(p2);
		Debug.DrawLine(pt0, pt1, Color.red);
		Debug.DrawLine(pt1, pt2, Color.red);
		Debug.DrawLine(pt2, pt0, Color.red);

		GameObject objHit = getGameObjectFromMesh(hitTransform, mesh);
		if (objHit != null)
		{
			Vector2 uv0 = mesh.uv[triangles[hit.triangleIndex * 3 + 0]];
			Vector2 uv1 = mesh.uv[triangles[hit.triangleIndex * 3 + 1]];
			Vector2 uv2 = mesh.uv[triangles[hit.triangleIndex * 3 + 2]];

			Vector2 uvHit = new Vector2();

			Texture2D texHit = objHit.renderer.material.GetTexture(0) as Texture2D;
			if (texHit != null && Input.GetMouseButton(1))
			{
				if (_tex == null)
					_tex = new Texture2D(texHit.width, texHit.height, texHit.format, texHit.mipmapCount != 0);
				uvHit = getInternalUV(hit.point, pt1, pt0, pt2, uv1, uv0, uv2);
				if (_oldTexPointSetted == false)
				{
					_tex.SetPixel((int)(_tex.width * uvHit.x),
									(int)(_tex.height * uvHit.y), Color.red);
					_oldTexPoint = new Vector2(_tex.width * uvHit.x, _tex.height * uvHit.y);
					_oldTexPointSetted = true;
				}
				else
				{
					Vector2 newTexPoint = new Vector2(_tex.width * uvHit.x, _tex.height * uvHit.y);
					DrawHelper.DrawLine(_oldTexPoint, newTexPoint, colorActive, _tex);
					_oldTexPoint = newTexPoint;
				}
				_tex.Apply();
				objHit.renderer.material.SetTexture(0, _tex);
			}
			else if (!Input.GetMouseButton(1))
			{
				_oldTexPointSetted = false;
			}
			if (texHit != null && Input.GetMouseButtonDown(2))
			{
				if (_tex == null)
					_tex = new Texture2D(texHit.width, texHit.height, texHit.format, texHit.mipmapCount != 0);
				uvHit = getInternalUV(hit.point, pt1, pt0, pt2, uv1, uv0, uv2);
				Vector2 pTex0 = new Vector2(_tex.width * uv0.x, _tex.height * uv0.y);
				Vector2 pTex1 = new Vector2(_tex.width * uv1.x, _tex.height * uv1.y);
				Vector2 pTex2 = new Vector2(_tex.width * uv2.x, _tex.height * uv2.y);
				Vector2 phit = new Vector2(_tex.width * uvHit.x, _tex.height * uvHit.y);
				//DrawHelper.DrawLine(pTex0, pTex1, _tex);
				//DrawHelper.DrawLine(pTex1, pTex2, _tex);
				//DrawHelper.DrawLine(pTex0, pTex2, _tex);
				//DrawHelper.FillTriangle(pTex0, pTex1, pTex2, _tex);
				DrawHelper.FillTriangleWithPropagate(pTex0, pTex1, pTex2, phit, colorActive, _tex);
				_tex.Apply();
				objHit.renderer.material.SetTexture(0, _tex);
			}
		}
	}

	public static void ComputeHit(RaycastHit hit, int buttonClicked)
	{
		MeshCollider meshCollider = hit.collider as MeshCollider;
		if (meshCollider == null || meshCollider.sharedMesh == null)
			return;

		Mesh mesh = meshCollider.sharedMesh;
		Vector3[] vertices = mesh.vertices;
		int[] triangles = mesh.triangles;
		Transform hitTransform = hit.collider.transform;
		Vector3 p0 = vertices[triangles[hit.triangleIndex * 3 + 0]];
		Vector3 p1 = vertices[triangles[hit.triangleIndex * 3 + 1]];
		Vector3 p2 = vertices[triangles[hit.triangleIndex * 3 + 2]];
		Vector3 pt0 = hitTransform.TransformPoint(p0);
		Vector3 pt1 = hitTransform.TransformPoint(p1);
		Vector3 pt2 = hitTransform.TransformPoint(p2);

		GameObject objHit = getGameObjectFromMesh(hitTransform, mesh);
		if (objHit != null)
		{
			Vector2 uv0 = mesh.uv[triangles[hit.triangleIndex * 3 + 0]];
			Vector2 uv1 = mesh.uv[triangles[hit.triangleIndex * 3 + 1]];
			Vector2 uv2 = mesh.uv[triangles[hit.triangleIndex * 3 + 2]];

			Texture2D texHit = objHit.renderer.sharedMaterial.GetTexture(0) as Texture2D;
			if (texHit != null && buttonClicked == 0)
			{
				if (_tex == null)
					_tex = texHit;// new Texture2D(texHit.width, texHit.height, texHit.format, texHit.mipmapCount != 0);
				Vector2 uvHit = getInternalUV(hit.point, pt1, pt0, pt2, uv1, uv0, uv2);
				if (_oldTexPointSetted == false)
				{
					_tex.SetPixel((int)(_tex.width * uvHit.x),
									(int)(_tex.height * uvHit.y), Color.red);
					_oldTexPoint = new Vector2(_tex.width * uvHit.x, _tex.height * uvHit.y);
					_oldTexPointSetted = true;
				}
				else
				{
					Vector2 newTexPoint = new Vector2(_tex.width * uvHit.x, _tex.height * uvHit.y);
					DrawHelper.DrawLine(_oldTexPoint, newTexPoint, colorActive, _tex);
					_oldTexPoint = newTexPoint;
				}
				_tex.Apply();
				objHit.renderer.sharedMaterial.SetTexture(0, _tex);
			}
			else if (!Input.GetMouseButton(1))
			{
				_oldTexPointSetted = false;
			}
			if (texHit != null && buttonClicked == 2)
			{
				if (_tex == null)
					_tex = new Texture2D(texHit.width, texHit.height, texHit.format, texHit.mipmapCount != 0);
				Vector2 pTex0 = new Vector2(_tex.width * uv0.x, _tex.height * uv0.y);
				Vector2 pTex1 = new Vector2(_tex.width * uv1.x, _tex.height * uv1.y);
				Vector2 pTex2 = new Vector2(_tex.width * uv2.x, _tex.height * uv2.y);
				//DrawHelper.DrawLine(pTex0, pTex1, _tex);
				//DrawHelper.DrawLine(pTex1, pTex2, _tex);
				//DrawHelper.DrawLine(pTex0, pTex2, _tex);
				DrawHelper.FillTriangle(pTex0, pTex1, pTex2, colorActive, _tex);
				_tex.Apply();
				objHit.renderer.sharedMaterial.SetTexture(0, _tex);
			}
		}
	}

	private static GameObject getGameObjectFromMesh(Transform parentTransform, Mesh mesh)
	{
		int childCount = parentTransform.childCount;
		MeshCollider collider = parentTransform.gameObject.GetComponent<MeshCollider>();
		if (collider != null && collider.sharedMesh == mesh)
			return parentTransform.gameObject;
		for (int i = 0; i < childCount; ++i)
		{
			GameObject child = parentTransform.GetChild(i).gameObject;
			MeshCollider childCollider = child.GetComponent<MeshCollider>();
			if (childCollider != null && childCollider.sharedMesh == mesh)
				return child;
		}
		return null;
	}

	private static Vector2 getInternalUV(Vector3 hitPoint,
								  Vector3 p0, Vector3 p1, Vector3 p2,
								  Vector2 uv0, Vector2 uv1, Vector2 uv2)
	{
		float aT = triangleArea(p0, p1, p2);
		float a0 = triangleArea(hitPoint, p1, p2) / aT;
		float a1 = triangleArea(p0, hitPoint, p2) / aT;
		float a2 = triangleArea(p0, p1, hitPoint) / aT;

		Vector2 uv = uv0 * a0 + uv1 * a1 + uv2 * a2;
		return uv;
	}

	private static float triangleArea(Vector3 p0, Vector3 p1, Vector3 p2)
	{
		float a = (p1 - p0).magnitude;
		float b = (p2 - p1).magnitude;
		float c = (p0 - p2).magnitude;
		float s = (a + b + c) / 2;

		return Mathf.Sqrt(s * (s - a) * (s - b) * (s - c));
	}
}