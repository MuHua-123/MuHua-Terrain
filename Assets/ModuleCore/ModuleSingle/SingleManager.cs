using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuHua;

/// <summary>
/// 全局管理器
/// </summary>
public class SingleManager : ModuleSingle<SingleManager> {

	protected override void Awake() => NoReplace();

}
