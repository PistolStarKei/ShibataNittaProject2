using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class SpawnItemRates {

	//出現割合　合計で100%
	public static Dictionary<Pickup,int> Rate_Kaifuku = new Dictionary<Pickup,int>()
	{
		{ Pickup.CureS, 60 },
		{ Pickup.CureM, 30 },
		{ Pickup.CureL, 10}
	};

	//出現割合　合計で100%
	public static readonly Dictionary<Pickup,int> Rate_Subweapon = new Dictionary<Pickup,int>()
	{
		{ Pickup.NAPAM, 60 },
		{ Pickup.NUKE, 30 },
		{ Pickup.RAZER, 10},
		{ Pickup.STEALTH, 60 },
		{ Pickup.WAVE, 30 },
		{ Pickup.YUDOU, 10},
		{ Pickup.ZENHOUKOU, 10}
	};

	//スタート後の　スポーン間隔
	public static readonly float spawnRepeatRate=30.0f;

	//プレイヤの数１に対するスポーン数　スタート時
	public static readonly int spawnNum_OnStartPerShip_Rate_Kaifuku=0;
	public static readonly int spawnNum_OnStartPerShip_Rate_Subweapon=5;

	//プレイヤの数１に対するスポーン数　ゲーム開始後の一定間隔
	public static readonly int spawnNum_OnUpdatePerShip_Rate_Kaifuku=1;
	public static readonly int spawnNum_OnUpdatePerShip_Rate_Subweapon=1;

	//一定間隔の追加を何秒後に開始するか  設けないのであれば 0.0fに
	public static readonly float spawnTimeStart_Rate_Kaifuku=600.0f;
	public static readonly float spawnTimeStart_Rate_Subweapon=600.0f;

	//一定間隔の追加を何秒後に終了するか　　設けないのであればとんでもなく大きな値に
	public static readonly float spawnTimeEnd_Rate_Kaifuku=600.0f;
	public static readonly float spawnTimeEnd_Rate_Subweapon=600.0f;
}
