using System;
using UnityEngine;

public class ViewportRectInt {
	public int startX;
	public int startY;
	public int endX;
	public int endY;

	public bool PointInRect(int x, int y)
	{

		return (x >= startX &&
			x < endX &&
			y >= startY &&
			y < endY ) ;
	}
	
	public bool PointInRect(float x, float y)
	{
		return (x >= startX &&
			x < endX &&
			y >= startY &&
			y < endY ) ;
	}

	public ViewportRectInt Clamp(ViewportRectInt bounds){
		return new ViewportRectInt() {
			startX = Math.Max(bounds.startX, startX),
			startY = Math.Max(bounds.startY, startY),
			endX = Math.Min(bounds.endX, endX),
			endY = Math.Min(bounds.endY, endY)
		};
	}
}
