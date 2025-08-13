using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuHua {
	/// <summary>
	/// 六边形 - 工具
	/// </summary>
	public static class HexTool {
		/// <summary> 格子坐标转世界坐标 </summary>
		public static Vector3 HexToWorld(Vector2Int xy, float size) {
			float width = size * 2f;
			float height = Mathf.Sqrt(3f) * size;
			float offsetX = xy.x * (width * 0.75f);
			float offsetY = xy.y * height + (xy.x % 2 == 0 ? 0 : height / 2f);
			return new Vector3(offsetX, 0, offsetY);
		}
		/// <summary> 世界坐标转格子坐标 </summary>
		public static Vector2Int WorldToHex(Vector3 worldPosition, float size) {
			float width = size * 2f;
			float height = Mathf.Sqrt(3f) * size;
			// 计算近似的x
			int x = Mathf.RoundToInt(worldPosition.x / (width * 0.75f));
			// 计算近似的y
			float yOffset = (x % 2 == 0) ? 0 : height / 2f;
			int y = Mathf.RoundToInt((worldPosition.z - yOffset) / height);
			return new Vector2Int(x, y);
		}
		/// <summary> 计算中心点 </summary>
		public static Vector3 CenterPoint(Vector2Int xy, float size) {
			float width = size * 2f;
			float height = size * Mathf.Sqrt(3f);
			float x = -(xy.x - 1) * width * 0.75f * 0.5f;
			float z = -(xy.y - 1) * height * 0.5f;
			return new Vector3(x, 0, z);
		}
	}
}
