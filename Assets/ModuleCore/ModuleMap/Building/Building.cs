using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuHua;

/// <summary>
/// 建筑
/// </summary>
public class Building : MonoBehaviour {

	/// <summary> 设置 </summary>
	public void Settings(Vector3 worldPosition) {
		transform.position = worldPosition;
	}

}
