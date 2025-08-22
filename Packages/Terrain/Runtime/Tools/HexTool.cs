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
			float width = size;
			float height = Mathf.Sqrt(3f) * size * 0.5f;
			float offsetX = xy.x * (width * 0.75f);
			float offsetY = xy.y * height + (xy.x % 2 == 0 ? 0 : height / 2f);
			return new Vector3(offsetX, 0, offsetY);
		}
		/// <summary> 世界坐标转格子坐标 </summary>
		public static Vector2Int WorldToHex(Vector3 worldPosition, float size) {
			float width = size;
			float height = Mathf.Sqrt(3f) * size * 0.5f;
			// 计算近似的x
			int x = Mathf.RoundToInt(worldPosition.x / (width * 0.75f));
			// 计算近似的y
			float yOffset = (x % 2 == 0) ? 0 : height / 2f;
			int y = Mathf.RoundToInt((worldPosition.z - yOffset) / height);
			return new Vector2Int(x, y);
		}
		/// <summary> 计算中心点 </summary>
		public static Vector3 CenterPoint(int wide, int high, float size) {
			float width = size;
			float height = size * Mathf.Sqrt(3f) * 0.5f;
			float x = -(wide - 1) * width * 0.75f * 0.5f;
			float z = -(high - 1) * height * 0.5f;
			return new Vector3(x, 0, z);
		}
		/// <summary> 邻近位置 </summary>
		public static List<Vector2Int> Neighbour(Vector2Int xy) {
			int evenNumber = xy.x % 2 == 0 ? -1 : 1;
			return new List<Vector2Int> {
				new Vector2Int(xy.x + 0, xy.y + 1),
				new Vector2Int(xy.x + 0, xy.y - 1),

				new Vector2Int(xy.x + 1, xy.y + 0),
				new Vector2Int(xy.x - 1, xy.y + 0),

				new Vector2Int(xy.x + 1, xy.y + evenNumber),
				new Vector2Int(xy.x - 1, xy.y + evenNumber)
			};
		}
	}
}
