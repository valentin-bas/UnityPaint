using UnityEngine;
using System.Collections;

public class DrawHelper
{
	public static void DrawLine(Vector2 p0, Vector2 p1, Color color, Texture2D tex)
	{
		TriangleFiller filler = new TriangleFiller();

		filler.Init(p0, p1, color, tex);
		filler.DrawLine();
	}

	public static void FillTriangle(Vector2 p0, Vector2 p1, Vector2 p2, Color color, Texture2D tex)
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

		filler0.Init(p0, p1, color, tex);
		filler1.Init(p0, p2, color, tex);
		filler2.Init(p1, p2, color, tex);

		while (filler0.IsEnded() == false && filler1.IsEnded() == false)
		{
			filler0.DrawLineStepY(out actualPoint0);
			filler1.DrawLineStepY(out actualPoint1);
			tmpLine.Init(actualPoint0, actualPoint1, color, tex);
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
			tmpLine.Init(actualPoint0, actualPoint1, color, tex);
			tmpLine.DrawLine();
		}
	}

	public static void FillTriangleWithPropagate(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 start, Color color, Texture2D tex)
	{
		TriangleFiller filler0 = new TriangleFiller();
		TriangleFiller filler1 = new TriangleFiller();
		TriangleFiller filler2 = new TriangleFiller();

		filler0.Init(p0, p1, color, tex);
		filler1.Init(p0, p2, color, tex);
		filler2.Init(p1, p2, color, tex);

		filler0.DrawLine();
		filler1.DrawLine();
		filler2.DrawLine();

		//Vector2 start = p0 + (((p1 - p0) / 2.0f) + ((p2 - p0) / 2.0f)) / 4.0f;
		//Debug.Log("P0: " + p0 + "  P1: " + p1 + "  P2: " + p2 + "  Start: " + start);

		PropagateInTexture((int)start.x, (int)start.y, color, tex.GetPixel((int)start.x, (int)start.y), tex);
	}

	private static void PropagateInTexture(int x, int y, Color colorToFill, Color colorStart, Texture2D tex)
	{
		if (tex.GetPixel(x, y) != colorStart)
			return;
		tex.SetPixel(x, y, colorToFill);
		PropagateInTexture(x - 1, y, colorToFill, colorStart, tex);
		PropagateInTexture(x + 1, y, colorToFill, colorStart, tex);
		PropagateInTexture(x, y - 1, colorToFill, colorStart, tex);
		PropagateInTexture(x, y + 1, colorToFill, colorStart, tex);
	}
}
