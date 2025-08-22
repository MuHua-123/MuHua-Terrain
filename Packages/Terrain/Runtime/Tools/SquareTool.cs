using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuHua {
	/// <summary>
	/// 正方形 - 工具
	/// </summary>
	public static class SquareTool {

		/// <summary> 格子坐标转世界坐标 </summary>
		public static Vector3 GridToWorld(Vector2Int xy, float size) {
			return new Vector3(xy.x, 0, xy.y) * size;
		}
		/// <summary> 世界坐标转格子坐标 </summary>
		public static Vector2Int WorldToGrid(Vector3 worldPosition, float size) {
			int x = Mathf.RoundToInt(worldPosition.x / size);
			int y = Mathf.RoundToInt(worldPosition.z / size);
			return new Vector2Int(x, y);
		}
		/// <summary> 计算中心点 </summary>
		public static Vector3 CenterPoint(int wide, int high, float size) {
			return new Vector3(wide - 1, 0, high - 1) * size * -0.5f;
		}
		/// <summary> 邻近位置 </summary>
		public static List<Vector2Int> Neighbour(Vector2Int xy) {
			return new List<Vector2Int> {
				new Vector2Int(xy.x - 1, xy.y + 1),
				new Vector2Int(xy.x + 0, xy.y + 1),
				new Vector2Int(xy.x + 1, xy.y + 1),

				new Vector2Int(xy.x - 1, xy.y + 0),
				new Vector2Int(xy.x + 1, xy.y + 0),

				new Vector2Int(xy.x-  1, xy.y - 1),
				new Vector2Int(xy.x + 0, xy.y - 1),
				new Vector2Int(xy.x + 1, xy.y - 1)
			};
		}
		/// <summary> 邻近位置 </summary>
		public static List<Vector2Int> Connected(Vector2Int xy) {
			return new List<Vector2Int> {
				new Vector2Int(xy.x, xy.y + 1),
				new Vector2Int(xy.x, xy.y - 1),
				new Vector2Int(xy.x + 1, xy.y),
				new Vector2Int(xy.x - 1, xy.y),
			};
		}
	}
}
