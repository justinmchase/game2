using System;
using UnityEngine;

namespace Game {
    
    public class ViewportRect {
		public float startX;
		public float startY;
		public float endX;
		public float endY;

        public static ViewportRect FromCenterRadius(float centerX, float centerY, float radiusX, float radiusY){
            return new ViewportRect() {
				startX = centerX - radiusX,
				startY = centerY - radiusY,
				endX = centerX + radiusX,
				endY = centerY + radiusY,
			};
        }

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

        public ViewportRect Clamp(ViewportRect bounds){
            return new ViewportRect() {
				startX = Math.Max(bounds.startX, startX),
				startY = Math.Max(bounds.startY, startY),
				endX = Math.Min(bounds.endX, endX),
				endY = Math.Min(bounds.endY, endY)
            };
        }

        public ViewportRectInt ToInt(){
            return new ViewportRectInt(){
                startX = (int)this.startX,
                startY =  (int)this.startY,
                endX =  (int)this.endX,
                endY =  (int)this.endY
            };
        }
	}
}
