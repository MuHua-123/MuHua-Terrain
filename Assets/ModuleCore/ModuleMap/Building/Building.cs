using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuHua;

/// <summary>
/// 建筑
/// </summary>
public class Building : MonoBehaviour {
	/// <summary> 占用单元 </summary>
	public List<Transform> occupyUnit;

	/// <summary> 初始化 </summary>
	public void Initial() {

	}
	/// <summary> 旋转 </summary>
	public void Rotate(float angle) {
		transform.eulerAngles += new Vector3(0, angle, 0);
	}
	/// <summary> 占用空间 </summary>
	public List<Vector3> Occupy() {
		List<Vector3> positions = new List<Vector3>();
		occupyUnit.ForEach(unit => positions.Add(unit.position));
		return positions;
	}
}
