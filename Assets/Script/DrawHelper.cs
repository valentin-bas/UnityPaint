using UnityEngine;
using System.Collections;

public class DrawHelper
{
	public static void DrawLine(Vector2 p0, Vector2 p1, Texture2D tex)
	{
		TriangleFiller filler = new TriangleFiller();

		filler.Init(p0, p1, tex);
		filler.DrawLine();
	}

	public static void FillTriangle(Vector2 p0, Vector2 p1, Vector2 p2, Texture2D tex)
	{
		TriangleFiller filler0 = new TriangleFiller();
		TriangleFiller filler1 = new TriangleFiller();
		TriangleFiller filler2 = new TriangleFiller();
		TriangleFiller tmpLine = new TriangleFiller();
		Vector2 actualPoint0 = new Vector2();
		Vector2 actualPoint1 = new Vector2();

		if (p0.y > p1.y)
			TriangleFiller.Swap(ref p0, ref p1);
		if (p0.y > p2.y)
			TriangleFiller.Swap(ref p0, ref p2);
		if (p1.x > p2.x)
			TriangleFiller.Swap(ref p1, ref p2);

		filler0.Init(p0, p1, tex);
		filler1.Init(p0, p2, tex);
		filler2.Init(p1, p2, tex);

		while (filler0.IsEnded() == false && filler1.IsEnded() == false)
		{
			filler0.DrawLineStepY(out actualPoint0);
			filler1.DrawLineStepY(out actualPoint1);
			//Debug.Log("point 1 : " + actualPoint0 + "   point 2 : " + actualPoint1);
			tmpLine.Init(actualPoint0, actualPoint1, tex);
			tmpLine.DrawLine();
		}

		if (filler0.IsEnded() == true)
			filler0 = filler2;
		else if (filler1.IsEnded() == true)
			filler1 = filler2;

		while (filler0.IsEnded() == false || filler1.IsEnded() == false)
		{
			filler0.DrawLineStepY(out actualPoint0);
			filler1.DrawLineStepY(out actualPoint1);
			//Debug.Log("point 1 : " + actualPoint0 + "   point 2 : " + actualPoint1);
			tmpLine.Init(actualPoint0, actualPoint1, tex);
			tmpLine.DrawLine();
		}
	}
}
