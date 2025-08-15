using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuHua;

/// <summary>
/// 建筑空间
/// </summary>
public class BuildingSpace : MapUnitSpace {
	/// <summary> 建筑物 </summary>
	public Transform building;

	public override bool IsWalkable => building == null;
}
