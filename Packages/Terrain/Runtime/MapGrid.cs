using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuHua {
	/// <summary>
	/// 格子 - 地图
	/// </summary>
	public class MapGrid : Map {

		public const int MOVE_STRAIGHT_COST = 10;
		public const int MOVE_DIAGONAL_COST = 14;

		/// <summary> 拐角处是否可步行 </summary>
		public readonly bool isCornerWalkable;

		public MapGrid(int wide, int high, float size, Vector3 originPosition, bool isCornerWalkable) : base(wide, high, size, originPosition) {
			this.isCornerWalkable = isCornerWalkable;
		}

		public override bool TryWorldPosition(Vector3 worldPosition, out Vector3 position) {
			Vector2Int xy = GridTool.WorldToGrid(worldPosition - originPosition, size);
			return TryWorldPosition(xy, out position);
		}
		public override bool TryWorldPosition(Vector2Int xy, out Vector3 position) {
			position = GetWorldPosition(xy.x, xy.y);
			return this.TryXY(xy.x, xy.y);
		}
		public override bool TryMapUnit(Vector3 worldPosition, out MapUnit unit) {
			Vector2Int xy = GridTool.WorldToGrid(worldPosition - originPosition, size);
			return TryMapUnit(xy.x, xy.y, out unit);
		}
		public override bool FindPath(Vector3 sp, Vector3 ep, out List<Vector3> vectorPath) {
			vectorPath = new List<Vector3>();
			// 查询起点和终点
			bool isValidS = TryMapUnit(sp, out MapUnit sMapUnit);
			bool isValidE = TryMapUnit(ep, out MapUnit eMapUnit);
			if (!isValidS || !isValidE && eMapUnit.IsWalkable) { return false; }
			// 查询路径
			vectorPath = FindPath(sMapUnit, eMapUnit);
			return vectorPath != null && vectorPath.Count > 0;
		}

		/// <summary> 获取世界坐标 </summary>
		public Vector3 GetWorldPosition(int x, int y) {
			return GridTool.GridToWorld(new Vector2Int(x, y), size) + originPosition;
		}
		/// <summary> 获取地图单元 </summary>
		public bool TryMapUnit(int x, int y, out MapUnit unit) {
			x = Mathf.Clamp(x, 0, wide - 1);
			y = Mathf.Clamp(y, 0, high - 1);
			unit = unitArray[x, y];
			return this.TryXY(x, y);
		}
		/// <summary> 查询路径 </summary>
		public List<Vector3> FindPath(MapUnit sMapUnit, MapUnit eMapUnit) {
			List<MapUnit> openList = new List<MapUnit> { sMapUnit };
			List<MapUnit> closeList = new List<MapUnit>();
			this.Loop((x, y) => { unitArray[x, y].InitializationCost(); });

			sMapUnit.GCost = 0;
			sMapUnit.HCost = CalculateDistanceCost(sMapUnit, eMapUnit);
			sMapUnit.CalculateFCost();

			while (openList.Count > 0) {
				MapUnit currentNode = GetLowestFCostNode(openList);
				//以达到最终目的地
				if (currentNode == eMapUnit) { return CalculatePath(eMapUnit); }
				openList.Remove(currentNode);
				closeList.Add(currentNode);
				CalculateNeighbour(openList, closeList, currentNode, eMapUnit);
			}
			return null;
		}
		/// <summary> 计算距离h成本 </summary>
		public int CalculateDistanceCost(MapUnit a, MapUnit b) {
			int xDistance = Mathf.Abs(a.x - b.x);
			int yDistance = Mathf.Abs(a.y - b.y);
			int mDistance = Mathf.Min(xDistance, yDistance);
			int remaining = Mathf.Abs(xDistance - yDistance);
			return MOVE_DIAGONAL_COST * mDistance + MOVE_STRAIGHT_COST * remaining;
		}
		/// <summary> 获得最小f成本 </summary>
		public MapUnit GetLowestFCostNode(List<MapUnit> openList) {
			MapUnit lowestFCostNode = openList[0];
			for (int i = 0; i < openList.Count; i++) {
				if (openList[i].FCost >= lowestFCostNode.FCost) { continue; }
				lowestFCostNode = openList[i];
			}
			return lowestFCostNode;
		}
		/// <summary> 计算临近节点 </summary>
		public void CalculateNeighbour(List<MapUnit> openList, List<MapUnit> closeList, MapUnit currentNode, MapUnit endNode) {
			List<MapUnit> neighbourList = FindNeighbour(currentNode.x, currentNode.y);
			foreach (MapUnit neighbourNode in neighbourList) {
				//如果临近节点在关闭列表则跳过
				if (closeList.Contains(neighbourNode)) { continue; }
				//如果节点不可通行则添加到关闭列表
				if (!neighbourNode.IsWalkable && neighbourNode != endNode) {
					closeList.Add(neighbourNode);
					continue;
				}
				//计算阻挡
				if (!isCornerWalkable && CornerWalkable(currentNode, neighbourNode)) { continue; }

				//计算成本
				int tentativeGCost = currentNode.GCost + CalculateDistanceCost(currentNode, neighbourNode);
				if (tentativeGCost >= neighbourNode.GCost) { continue; }
				neighbourNode.cameFromNode = currentNode;
				neighbourNode.GCost = tentativeGCost;
				neighbourNode.HCost = CalculateDistanceCost(neighbourNode, endNode);
				neighbourNode.CalculateFCost();
				if (!openList.Contains(neighbourNode)) { openList.Add(neighbourNode); }
			}
		}
		/// <summary> 计算阻挡 </summary>
		public bool CornerWalkable(MapUnit currentNode, MapUnit neighbourNode) {
			if (CalculateDistanceCost(currentNode, neighbourNode) != MOVE_DIAGONAL_COST) { return false; }
			int x = neighbourNode.x - currentNode.x;
			int y = neighbourNode.y - currentNode.y;
			MapUnit a = unitArray[currentNode.x + x, currentNode.y];
			MapUnit b = unitArray[currentNode.x, currentNode.y + y];
			return !a.IsWalkable || !b.IsWalkable;
		}
		/// <summary> 返回最终路径 </summary>
		public List<Vector3> CalculatePath(MapUnit endNode) {
			List<Vector3> finalPath = new List<Vector3>();
			MapUnit currentNode = endNode;
			while (currentNode.cameFromNode != null) {
				finalPath.Add(GetWorldPosition(currentNode.x, currentNode.y));
				currentNode = currentNode.cameFromNode;
			}
			finalPath.Add(GetWorldPosition(currentNode.x, currentNode.y));
			finalPath.Reverse();
			return finalPath;
		}
		/// <summary> 获取相邻的节点 </summary>
		public List<MapUnit> FindNeighbour(int x, int y) {
			List<MapUnit> neighbourList = new List<MapUnit>();
			neighbourList.AddRange(FindConnected(x, y));
			if (TryMapUnit(x + 1, y + 1, out MapUnit unit5)) { neighbourList.Add(unit5); }
			if (TryMapUnit(x + 1, y - 1, out MapUnit unit6)) { neighbourList.Add(unit6); }
			if (TryMapUnit(x - 1, y + 1, out MapUnit unit7)) { neighbourList.Add(unit7); }
			if (TryMapUnit(x - 1, y - 1, out MapUnit unit8)) { neighbourList.Add(unit8); }
			return neighbourList;
		}
		/// <summary> 获取相连的节点 </summary>
		public List<MapUnit> FindConnected(int x, int y) {
			List<MapUnit> neighbourList = new List<MapUnit>();
			if (TryMapUnit(x, y + 1, out MapUnit unit1)) { neighbourList.Add(unit1); }
			if (TryMapUnit(x, y - 1, out MapUnit unit2)) { neighbourList.Add(unit2); }
			if (TryMapUnit(x + 1, y, out MapUnit unit3)) { neighbourList.Add(unit3); }
			if (TryMapUnit(x - 1, y, out MapUnit unit4)) { neighbourList.Add(unit4); }
			return neighbourList;
		}



		/*-------------------------------------------------遍历地图--------------------------------------------------------------*/
		// public void Loop(Action<int, int> action) {
		// 	for (int y = 0; y < high; y++) {
		// 		for (int x = 0; x < wide; x++) { action?.Invoke(x, y); }
		// 	}
		// }
		/*-------------------------------------------------校验范围--------------------------------------------------------------*/
		// public bool TryWorldPosition(Vector3 worldPosition, out int x, out int y) {
		// 	GetXY(worldPosition, out x, out y);
		// 	return TryGetXY(x, y);
		// }
		// public bool TryGetXY(int x, int y) {
		// 	return x >= 0 && x < wide && y >= 0 && y < high;
		// }
		/*-------------------------------------------------坐标转换--------------------------------------------------------------*/
		// public override bool TryWorldPosition(Vector3 worldPosition, out Vector3 position) {
		// 	GetXY(worldPosition, out int x, out int y);
		// 	position = GetWorldPosition(x, y);
		// 	return TryGetXY(x, y);
		// }
		// public Vector3 GetWorldPosition(Vector3 worldPosition) {
		// 	GetXY(worldPosition, out int x, out int y);
		// 	return GetWorldPosition(x, y);
		// }
		// public Vector3 GetWorldPosition(int x, int y) {
		// 	Vector3 offset = new Vector3(0.5f, 0, 0.5f);
		// 	return new Vector3(x, 0, y) + originPosition + offset;
		// }
		// public void GetXY(Vector3 worldPosition, out int x, out int y) {
		// 	x = Mathf.FloorToInt((worldPosition - originPosition).x);
		// 	y = Mathf.FloorToInt((worldPosition - originPosition).z);
		// }
		/*-------------------------------------------------单元操作--------------------------------------------------------------*/
		// public MMapUnit GetMapUnit(Vector3 worldPosition) {
		// 	GetXY(worldPosition, out int x, out int y);
		// 	return GetMapUnit(x, y);
		// }
		// public MMapUnit GetMapUnit(int x, int y) {
		// 	x = Mathf.Clamp(x, 0, wide - 1);
		// 	y = Mathf.Clamp(y, 0, high - 1);
		// 	return unitArray[x, y];
		// }
		// public void SetMapUnit(Vector3 worldPosition, MMapUnit mapUnit) {
		// 	GetXY(worldPosition, out int x, out int y);
		// 	SetMapUnit(x, y, mapUnit);
		// }
		// public void SetMapUnit(int x, int y, MMapUnit mapUnit) {
		// 	x = Mathf.Clamp(x, 0, wide - 1);
		// 	y = Mathf.Clamp(y, 0, high - 1);
		// 	unitArray[x, y] = mapUnit;
		// }
		/*-------------------------------------------------校验单元--------------------------------------------------------------*/
		// public override bool TryGetMapUnit(Vector3 worldPosition, out MMapUnit unit) {
		// 	GetXY(worldPosition, out int x, out int y);
		// 	return TryGetMapUnit(x, y, out unit);
		// }
		// public bool TryGetMapUnit(int x, int y, out MMapUnit unit) {
		// 	unit = GetMapUnit(x, y);
		// 	return TryGetXY(x, y);
		// }
		// public bool TrySetMapUnit(Vector3 worldPosition, MMapUnit mapUnit) {
		// 	GetXY(worldPosition, out int x, out int y);
		// 	return TrySetMapUnit(x, y, mapUnit);
		// }
		// public bool TrySetMapUnit(int x, int y, MMapUnit mapUnit) {
		// 	if (TryGetXY(x, y)) { unitArray[x, y] = mapUnit; return true; }
		// 	else { return false; }
		// }
	}
}