using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuHua {
	/// <summary>
	/// 六边形 - 地图
	/// </summary>
	public class MapHexagon : Map {

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
		public override bool TryMapUnit(Vector3 worldPosition, out MapUnit unit) {
			Vector2Int xy = HexTool.WorldToHex(worldPosition - originPosition, size);
			return TryMapUnit(xy.x, xy.y, out unit);
		}
		public override bool FindPath(Vector3 sp, Vector3 ep, out List<Vector3> vectorPath) {
			vectorPath = new List<Vector3>();
			bool isValidS = TryMapUnit(sp, out MapUnit sMapUnit);
			bool isValidE = TryMapUnit(ep, out MapUnit eMapUnit);
			if (!isValidS || !isValidE || !eMapUnit.IsWalkable) { return false; }
			vectorPath = FindPath(sMapUnit, eMapUnit);
			return vectorPath != null && vectorPath.Count > 0;
		}

		public Vector3 GetWorldPosition(int x, int y) {
			return HexTool.HexToWorld(new Vector2Int(x, y), size) + originPosition;
		}
		public bool TryMapUnit(int x, int y, out MapUnit unit) {
			x = Mathf.Clamp(x, 0, wide - 1);
			y = Mathf.Clamp(y, 0, high - 1);
			unit = unitArray[x, y];
			return this.TryXY(x, y);
		}
		public List<Vector3> FindPath(MapUnit sMapUnit, MapUnit eMapUnit) {
			List<MapUnit> openList = new List<MapUnit> { sMapUnit };
			List<MapUnit> closeList = new List<MapUnit>();
			this.Loop((x, y) => { unitArray[x, y].InitializationCost(); });

			sMapUnit.GCost = 0;
			sMapUnit.HCost = CalculateDistanceCost(sMapUnit, eMapUnit);
			sMapUnit.CalculateFCost();

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
			int dx = Mathf.Abs(a.x - b.x);
			int dy = Mathf.Abs(a.y - b.y);
			int dz = Mathf.Abs((-a.x - a.y) - (-b.x - b.y));
			return 10 * Mathf.Max(dx, dy, dz);
		}
		public MapUnit GetLowestFCostNode(List<MapUnit> openList) {
			MapUnit lowestFCostNode = openList[0];
			for (int i = 0; i < openList.Count; i++) {
				if (openList[i].FCost >= lowestFCostNode.FCost) { continue; }
				lowestFCostNode = openList[i];
			}
			return lowestFCostNode;
		}
		public void CalculateNeighbour(List<MapUnit> openList, List<MapUnit> closeList, MapUnit currentNode, MapUnit endNode) {
			List<MapUnit> neighbourList = FindNeighbour(currentNode.x, currentNode.y);
			foreach (MapUnit neighbourNode in neighbourList) {
				if (closeList.Contains(neighbourNode)) { continue; }
				if (!neighbourNode.IsWalkable && neighbourNode != endNode) {
					closeList.Add(neighbourNode);
					continue;
				}
				int tentativeGCost = currentNode.GCost + CalculateDistanceCost(currentNode, neighbourNode);
				if (tentativeGCost >= neighbourNode.GCost) { continue; }
				neighbourNode.cameFromNode = currentNode;
				neighbourNode.GCost = tentativeGCost;
				neighbourNode.HCost = CalculateDistanceCost(neighbourNode, endNode);
				neighbourNode.CalculateFCost();
				if (!openList.Contains(neighbourNode)) { openList.Add(neighbourNode); }
			}
		}
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
		// 六边形相邻节点查找
		public List<MapUnit> FindNeighbour(int x, int y) {
			List<MapUnit> neighbourList = new List<MapUnit>();
			// 六边形六个方向，使用Vector2Int
			Vector2Int[] directions = new Vector2Int[] {
				new Vector2Int(+1, 0),
				new Vector2Int(-1, 0),
				new Vector2Int(0, +1),
				new Vector2Int(0, -1),
				new Vector2Int(+1, -1),
				new Vector2Int(-1, +1)
			};
			foreach (var dir in directions) {
				if (TryMapUnit(x + dir.x, y + dir.y, out MapUnit unit)) { neighbourList.Add(unit); }
			}
			return neighbourList;
		}
	}
}
