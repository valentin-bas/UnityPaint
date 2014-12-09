using UnityEngine;
using System.Collections;

public class TriangleFiller
{
	private int x1;
	private int x2;
	private int y1;
	private int y2;
	private bool steep;
	private float dx;
	private float dy;
	private float error;
	private int ystep;
	private int y;
	private int x;
	private int maxX;
	private Texture2D tex;
	private bool isEnded;

	public void Init(Vector2 p0, Vector2 p1, Texture2D texp)
	{
		isEnded = false;
		tex = texp;
		x1 = (int)p0.x;
		x2 = (int)p1.x;
		y1 = (int)p0.y;
		y2 = (int)p1.y;

		// Bresenham's line algorithm
		steep = (Mathf.Abs(y2 - y1) > Mathf.Abs(x2 - x1));
		if (steep)
		{
			Swap(ref x1, ref y1);
			Swap(ref x2, ref y2);
		}

		if (x1 > x2)
		{
			Swap(ref x1, ref x2);
			Swap(ref y1, ref y2);
		}

		dx = x2 - x1;
		dy = Mathf.Abs(y2 - y1);
		error = dx / 2.0f;
		ystep = (y1 < y2) ? 1 : -1;
		y = (int)y1;
		maxX = (int)x2;
		x = (int)x1;
	}

	public void DrawLine()
	{
		for (; x < maxX; x++)
		{
			_Step();
			//Debug.Log("X: " + x);
		}
		isEnded = true;
	}

	public bool DrawLineStepY(out Vector2 actualPoint)
	{
		int actualY = y;
		int actualX = x;
		for (; x < maxX; ++x)
		{
			_Step();
			//if (y != actualY)
			{
				if (steep)
					actualPoint = new Vector2(y, x);
				else
					actualPoint = new Vector2(x, y);
				++x;
				return false;
			}
		}
		if (steep)
			actualPoint = new Vector2(y, x);
		else
			actualPoint = new Vector2(x, y);
		isEnded = true;
		return true;
	}

	private void _Step()
	{
		if (steep)
			tex.SetPixel(y, x, Color.green);
		else
			tex.SetPixel(x, y, Color.green);
		error -= dy;
		if (error < 0)
		{
			y += ystep;
			error += dx;
		}
	}

	public bool IsEnded()
	{
		return isEnded;
	}

	public static void Swap<T>(ref T lhs, ref T rhs)
	{
		T temp;
		temp = lhs;
		lhs = rhs;
		rhs = temp;
	}
}
