﻿using UnityEngine;
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

	public void Init(Vector2 p0, Vector2 p1)
	{
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

	public void DrawLine(Texture2D tex)
	{
		for (; x < maxX; x++)
		{
			_Step(tex, x);
		}
	}

	public bool DrawLineStepY(Texture2D tex, out Vector2 actualPoint)
	{
		int actualY = y;
		for (; x < maxX; ++x)
		{
			_Step(tex, x);
			if (y != actualY)
			{
				if (steep)
					actualPoint = new Vector2(y, x);
				else
					actualPoint = new Vector2(x, y);
				++x;
				return false;
			}
		}
		actualPoint = new Vector2(x, y);
		return true;
	}

	private void _Step(Texture2D tex, int x)
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

	public static void Swap<T>(ref T lhs, ref T rhs)
	{
		T temp;
		temp = lhs;
		lhs = rhs;
		rhs = temp;
	}
}