using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PSParams{
	public static class GameParameters {

		//ゲーム全体の
		#region Game
		//デフォルトのチケットの個数
		public static readonly int DefaultTicketsNum=3;
		//次のチケット追加までの間隔　秒で
		public static readonly float TimeForNextTicket=600.0f;

		//安全地帯の分割数
		public static readonly int mapMasuXY=4;
		//安全地帯決定間隔
		public static readonly float safeZone_SetDulation=60.0f;
		//安全地帯決定後にアフェクトされるまでの猶予時間
		public static readonly float safeZone_Dulation=30.0f;
		//危険地帯でのダメージ間隔
		public static readonly float dangerDamage_Dulation=0.3f;
		//危険地帯でのダメージ間隔毎のダメージ
		public static readonly float dangerZoneDamage=10.0f;
		#endregion

		//Shipの値
		#region ship
		//名前
		public static readonly string[] shipNames=new string[8]{"Name1","Name2","Name3","Name4","Name5","Name6","Name7","Name8"};

		#endregion


		//Shipの値
		#region shipControl
		//最大HP値
		public static readonly float MaxHP=1500.0f;

		//ショット
		public static readonly float shotDulation=0.5f;
		public static readonly float shotOffset=0.2f;
		public static readonly float shotOffsetX=0.1f;

		//スピード
		public static readonly float maxSpeed = 1.0f;
		public static readonly float speed = 0.01f;

		#endregion

		//回復の効果
		#region Pickup_Cure
		//回復パーセンテージ S
		public static readonly float curePersentageS=10.0f;
		//回復パーセンテージ M
		public static readonly float curePersentageM=20.0f;
		//回復パーセンテージ L
		public static readonly float curePersentageL=30.0f;
		#endregion

		#region normal shots
		//通常弾の  WHITE BLUE YELLOW GREEN RED
		public static readonly float[] shot_damage=new float[5]{100.0f,100.0f,100.0f,100.0f,100.0f};
		public static readonly float[] shot_life=new float[5]{5.0f,5.0f,5.0f,5.0f,5.0f};
		public static readonly float[] shot_speed=new float[5]{2.0f,2.0f,2.0f,2.0f,2.0f};
		#endregion

		#region subweapons
		//サブウェポンの  NAPAM NUKE RAZER STEALTH WAVE YUDOU ZENHOUKOU
		//注意：レーザーはダメージ間隔に与えるダメージ
		public static readonly float[] sw_damage=new float[7]{100.0f,100.0f,1.0f,100.0f,100.0f,100.0f,100.0f};

		//ショットの生存時間
		public static readonly float[] sw_life=new float[7]{1.0f,1.0f,20.0f,20.0f,20.0f,20.0f,20.0f};

		//効果の継続時間
		public static readonly float[] sw_timer=new float[7]{3.0f,5.0f,10.0f,10.0f,3.0f,10.0f,3.0f};

		//継続時間内でショットをオフにするか？
		public static readonly bool[] sw_isShotOff=new bool[7]{true,true,true,true,true,true,true};

		//誘導弾の発射個数
		public static readonly int yudoudanShots=5;
		public static readonly float yudoudanMaxDistance=5.0f;

		//レーザーの有効射程距離
		public static readonly float razerMaxDistance=5.0f;
		//レーザーのダメージ間隔
		public static readonly float razerDamageDulation=0.3f;
		#endregion
	}
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

}
