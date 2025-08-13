using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace MuHua {
	/// <summary>
	/// 地图
	/// </summary>
	public abstract class Map {
		/// <summary> 地图宽 </summary>
		public readonly int wide;
		/// <summary> 地图高 </summary>
		public readonly int high;
		/// <summary> 地图大小 </summary>
		public readonly float size;
		/// <summary> 地图原点 </summary>
		public readonly Vector3 originPosition;
		/// <summary> 地图单元 </summary>
		public readonly MapUnit[,] unitArray;

		// 索引器
		public MapUnit this[int x, int y] { get => unitArray[x, y]; }

		public Map(int wide, int high, float size, Vector3 originPosition) {
			this.wide = wide;
			this.high = high;
			this.size = size;
			this.originPosition = originPosition;

			unitArray = new MapUnit[wide, high];
			this.Loop((x, y) => unitArray[x, y] = new MapUnit(x, y));
		}

		/// <summary> 世界坐标 </summary>
		public abstract bool TryWorldPosition(Vector3 worldPosition, out Vector3 position);
		/// <summary> 世界坐标 </summary>
		public abstract bool TryWorldPosition(Vector2Int xy, out Vector3 position);
		/// <summary> 获取地图单元 </summary>
		public abstract bool TryMapUnit(Vector3 worldPosition, out MapUnit unit);
		/// <summary> 查询地图路径 </summary>
		public abstract bool FindPath(Vector3 sp, Vector3 ep, out List<Vector3> vectorPath);
	}
	/// <summary>
	/// 地图单元
	/// </summary>
	public class MapUnit {
		/// <summary> X坐标 </summary>
		public readonly int x;
		/// <summary> Y坐标 </summary>
		public readonly int y;
		/// <summary> XY坐标 </summary>
		public readonly Vector2Int xy;

		/// <summary> 起点节点到当前节点的实际成本 </summary>
		public int GCost = 0;
		/// <summary> 当前节点到目标节点的预估成本 </summary>
		public int HCost = 0;
		/// <summary> 总成本 </summary>
		public int FCost = 0;
		/// <summary> 来自节点 </summary>
		public MapUnit cameFromNode = null;

		/// <summary> 地图单元空间 </summary>
		public MapUnitSpace mapSpace = null;

		/// <summary> 是否可以行走 </summary>
		public bool IsWalkable => mapSpace != null && mapSpace.IsWalkable;

		public MapUnit(int x, int y) { this.x = x; this.y = y; xy = new Vector2Int(x, y); }
	}
	/// <summary>
	/// 地图单元空间
	/// </summary>
	public abstract class MapUnitSpace {
		/// <summary> 是否可以行走 </summary>
		public abstract bool IsWalkable { get; }
	}
}