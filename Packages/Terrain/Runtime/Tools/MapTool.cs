using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuHua {
	/// <summary>
	/// 地图工具
	/// </summary>
	public static class MapTool {
		/// <summary> 遍历地图 </summary>
		public static void Loop(this Map map, Action<int, int> action) {
			for (int y = 0; y < map.high; y++) { map.LoopX(y, action); }
		}
		/// <summary> 遍历地图X </summary>
		public static void LoopX(this Map map, int y, Action<int, int> action) {
			for (int x = 0; x < map.wide; x++) { action?.Invoke(x, y); }
		}
		/// <summary> 遍历地图Y </summary>
		public static void LoopY(this Map map, int x, Action<int, int> action) {
			for (int y = 0; y < map.high; y++) { action?.Invoke(x, y); }
		}
		/// <summary> 校验XY </summary>
		public static bool TryXY(this Map map, int x, int y) {
			return x >= 0 && x < map.wide && y >= 0 && y < map.high;
		}

		/// <summary> 初始化寻路成本 </summary>
		public static void InitializationCost(this MapUnit mapUnit) {
			mapUnit.GCost = int.MaxValue;
			mapUnit.CalculateFCost();
			mapUnit.cameFromNode = null;
		}
		/// <summary> 计算寻路成本 </summary>
		public static void CalculateFCost(this MapUnit mapUnit) {
			mapUnit.FCost = mapUnit.GCost + mapUnit.HCost;
		}
	}
}