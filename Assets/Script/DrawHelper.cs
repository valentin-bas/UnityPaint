using UnityEngine;
using System.Collections;

public class DrawHelper
{
	public static void DrawLine(Vector2 p0, Vector2 p1, Texture2D tex)
	{
		TriangleFiller filler = new TriangleFiller();

		filler.Init(p0, p1);
		filler.DrawLine(tex);
	}

	public static void FillTriangle(Vector2 p0, Vector2 p1, Vector2 p2, Texture2D tex)
	{
		TriangleFiller filler0 = new TriangleFiller();
		TriangleFiller filler1 = new TriangleFiller();

		if (p0.y > p1.y)
			TriangleFiller.Swap(ref p0, ref p1);
		if (p0.y > p2.y)
			TriangleFiller.Swap(ref p0, ref p2);
		if (p1.x > p2.x)
			TriangleFiller.Swap(ref p1, ref p2);

		filler0.Init(p0, p1);
		filler1.Init(p0, p2);

		TriangleFiller filler2 = new TriangleFiller();
		filler2.Init(p1, p2);

		Vector2 garbage = new Vector2();

		//filler0.DrawLine(tex);
		//filler1.DrawLine(tex);
		//filler2.DrawLine(tex);

		//while (!filler0.DrawLineStepY(tex, out garbage))
		//	;
		//while (!filler1.DrawLineStepY(tex, out garbage))
		//	;
		//while (!filler2.DrawLineStepY(tex, out garbage))
		//	;

		Vector2 actualPoint0 = new Vector2();
		Vector2 actualPoint1 = new Vector2();
		
		while (!filler0.DrawLineStepY(tex, out actualPoint0))
		{
			filler1.DrawLineStepY(tex, out actualPoint1);
			TriangleFiller tmpLine = new TriangleFiller();
			tmpLine.Init(actualPoint0, actualPoint1);
			//tmpLine.DrawLine(tex);
		}
	}
}
