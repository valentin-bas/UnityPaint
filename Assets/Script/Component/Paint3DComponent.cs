using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class Paint3DComponent : MonoBehaviour
{
	public Texture2D ActualTexture;
	public Texture2D OldTexture;
	public GameObject AttachedGameObject;

	public void ResetTexture()
	{
		if (OldTexture != null && AttachedGameObject != null)
		{
			Debug.Log("ResetTexture");
			AttachedGameObject.renderer.sharedMaterial.SetTexture(0, OldTexture);
			ActualTexture = OldTexture;
		}
	}

	public void SaveTexture()
	{
		if (OldTexture == ActualTexture)
			return;
		byte[] bytes = ActualTexture.EncodeToPNG();
		DestroyImmediate(ActualTexture);
		string assetPath = "/" + AssetDatabase.GetAssetPath(OldTexture);
		Debug.Log("path:" + assetPath);

		if (assetPath.StartsWith("/Assets"))
			assetPath = assetPath.Remove(0, 7);
		Debug.Log(Application.dataPath + assetPath);
		File.WriteAllBytes(Application.dataPath + assetPath, bytes);
		AssetDatabase.Refresh();
		ResetTexture();
	}
}
