using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuHua {
	/// <summary>
	/// 六边形 - 地图
	/// </summary>
	public class MapHexagon : Map {

		public const int MOVE_COST = 10;

		public MapHexagon(int wide, int high, float size, Vector3 originPosition) : base(wide, high, size, originPosition) {

		}

		public override bool TryWorldPosition(Vector3 worldPosition, out Vector3 position) {
			Vector2Int xy = HexTool.WorldToHex(worldPosition - originPosition, size);
			return TryWorldPosition(xy, out position);
		}
		public override bool TryWorldPosition(Vector2Int xy, out Vector3 position) {
			position = GetWorldPosition(xy.x, xy.y);
			return this.TryXY(xy.x, xy.y);
		}
		public override bool FindUnit(Vector3 worldPosition, out MapUnit unit) {
			Vector2Int xy = HexTool.WorldToHex(worldPosition - originPosition, size);
			return FindUnit(xy.x, xy.y, out unit);
		}
		public override bool FindPath(Vector3 sp, Vector3 ep, out List<Vector3> vectorPath) {
			vectorPath = new List<Vector3>();
			bool isValidS = FindUnit(sp, out MapUnit sMapUnit);
			bool isValidE = FindUnit(ep, out MapUnit eMapUnit);
			if (!isValidS || !isValidE || !eMapUnit.IsWalkable) { return false; }
			vectorPath = FindPath(sMapUnit, eMapUnit);
			return vectorPath != null && vectorPath.Count > 0;
		}

		public Vector3 GetWorldPosition(int x, int y) {
			return HexTool.HexToWorld(new Vector2Int(x, y), size) + originPosition;
		}

		#region 路径查询
		/// <summary> 查询路径 </summary>
		public List<Vector3> FindPath(MapUnit sMapUnit, MapUnit eMapUnit) {
			this.Loop((x, y) => { unitArray[x, y].InitializationCost(); });

			sMapUnit.GCost = 0;
			sMapUnit.HCost = CalculateDistanceCost(sMapUnit, eMapUnit);
			sMapUnit.CalculateFCost();

			List<MapUnit> openList = new List<MapUnit> { sMapUnit };
			List<MapUnit> closeList = new List<MapUnit>();

			while (openList.Count > 0) {
				MapUnit currentNode = GetLowestFCostNode(openList);
				if (currentNode == eMapUnit) { return CalculatePath(eMapUnit); }
				openList.Remove(currentNode);
				closeList.Add(currentNode);
				CalculateNeighbour(openList, closeList, currentNode, eMapUnit);
			}
			return null;
		}
		public int CalculateDistanceCost(MapUnit a, MapUnit b) {
			// 六边形距离计算（曼哈顿距离）
			// int dx = Mathf.Abs(a.x - b.x);
			// int dy = Mathf.Abs(a.y - b.y);
			// int dz = Mathf.Abs((-a.x - a.y) - (-b.x - b.y));
			// return MOVE_COST * Mathf.Max(dx, dy, dz);
			return Mathf.RoundToInt(MOVE_COST * Vector2Int.Distance(a.xy, b.xy));
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
			neighbourList.ForEach(node => CalculateNeighbour(openList, closeList, node, currentNode, endNode));
		}
		/// <summary> 计算临近节点 </summary>
		public void CalculateNeighbour(List<MapUnit> openList, List<MapUnit> closeList, MapUnit neighbourNode, MapUnit currentNode, MapUnit endNode) {
			//如果临近节点在关闭列表则跳过
			if (closeList.Contains(neighbourNode)) { return; }
			//如果节点不可通行则添加到关闭列表
			if (!neighbourNode.IsWalkable && neighbourNode != endNode) { closeList.Add(neighbourNode); return; }
			//计算成本
			int tentativeGCost = currentNode.GCost + neighbourNode.MoveCost + MOVE_COST;
			if (tentativeGCost >= neighbourNode.GCost) { return; }
			neighbourNode.cameFromNode = currentNode;
			neighbourNode.GCost = tentativeGCost;
			neighbourNode.HCost = CalculateDistanceCost(neighbourNode, endNode);
			neighbourNode.CalculateFCost();
			if (!openList.Contains(neighbourNode)) { openList.Add(neighbourNode); }
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
		#endregion

		#region 查询单元
		/// <summary> 查询节点 </summary>
		public bool FindUnit(Vector2Int xy, out MapUnit unit) {
			return FindUnit(xy.x, xy.y, out unit);
		}
		/// <summary> 查询节点 </summary>
		public bool FindUnit(int x, int y, out MapUnit unit) {
			x = Mathf.Clamp(x, 0, wide - 1);
			y = Mathf.Clamp(y, 0, high - 1);
			unit = unitArray[x, y];
			return this.TryXY(x, y);
		}
		// 六边形相邻节点查找
		public List<MapUnit> FindNeighbour(int x, int y) {
			return FindUnits(HexTool.Neighbour(new Vector2Int(x, y)));
		}
		/// <summary> 查询节点 </summary>
		public List<MapUnit> FindUnits(List<Vector2Int> directions) {
			List<MapUnit> neighbourList = new List<MapUnit>();
			for (int i = 0; i < directions.Count; i++) {
				Vector2Int xy = directions[i];
				if (FindUnit(xy.x, xy.y, out MapUnit unit)) { neighbourList.Add(unit); }
			}
			return neighbourList;
		}
		#endregion
	}
}
