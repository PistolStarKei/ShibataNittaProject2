using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PSParams{
	public static class GameParameters {

		//ゲーム全体の
		#region Game
		//コネクトユーザーが０になったらゲームを終了するか
		public static readonly bool EndGameOnNoConnection=true;

		//デフォルトのチケットの個数
		public static readonly int DefaultTicketsNum=5;

		//次のチケット追加までの間隔　秒で
		public static readonly float TimeForNextTicket=600.0f;

		//平均ランクのランキングに参加できるまでのプレイ回数
		public static readonly int playNumToJoinAvgRanking=20;

		//何秒間ストリームが来ていなければデッドとするか。
		public static readonly float DisconnectionTime=3.0f;
		#endregion


		#region 安全地帯
		//安全地帯の分割数
		public static readonly int mapMasuXY=5;
		//安全地帯決定間隔
		public static readonly float safeZone_SetDulation=60.0f;
		//安全地帯決定後にアフェクトされるまでの猶予時間
		public static readonly float safeZone_Dulation=60.0f;
		//危険地帯でのダメージ間隔
		public static readonly float dangerDamage_Dulation=0.3f;
		//危険地帯でのダメージ間隔毎のダメージ
		public static readonly float dangerZoneDamageMin=1.0f;
		public static readonly float dangerZoneDamageMax=10.0f;
		#endregion

		//Shipの値
		#region ship
		//名前
		public static readonly string[] shipNames=new string[8]{"Name1","Name2","Name3","Name4","Name5","Name6","Name7","Name8"};

		//カラーリングの名前　Ship1 個数と一致させる
		//赤　青　白　クリア
		public static readonly string[] shipSubNamesShip1=new string[4]{"RED","BLUE","WHITE","SCLEAR"};
		//白　ライトブルー　ライトグリーン
		public static readonly string[] shipSubNamesShip2=new string[4]{"WHITE","LBLUE","LGREEN","HTEC"};
		//白　ライトブルー　ディープパープル　黄色青のダブルカラー
		public static readonly string[] shipSubNamesShip3=new string[4]{"WHITE","LBLUE","DPPL","SBLUE"};
		//白　青　カモフラ１　カモフラ２
		public static readonly string[] shipSubNamesShip4=new string[4]{"WHITE","BLUE","CAMO1","SCLEAR"};
		//白　青赤のダブル　クリア
		public static readonly string[] shipSubNamesShip5=new string[4]{"WHITE","WSLV","SIFI","SPPL"};
		//ライトグレー　カモフラ１　カモフラ２
		public static readonly string[] shipSubNamesShip6=new string[6]{"LGLAY","CAMO1","CAMO2","CAMO3","SAKURA","PLUTINUM"};
		//白　メタリック赤　メタリック黒　ゴールド
		public static readonly string[] shipSubNamesShip7=new string[4]{"WHITE","RMETAL","BMETAL","GOLD"};
		//青　パープル　チタン　クリア
		public static readonly string[] shipSubNamesShip8=new string[5]{"BLUE","PPL","SILVER","GOLD","SCLEAR"};

		//初期状態で搭乗可能か？　購入が必要か？
		public static Dictionary<string,bool[]> defaultAvaillabilityShip=new Dictionary<string, bool[]>(){
			{"Ship1",new  bool[4]{true,true,true,false}},
			{"Ship2",new  bool[4]{true,true,true,false}},
			{"Ship3",new  bool[4]{true,true,true,true}},
			{"Ship4",new  bool[4]{true,true,true,false}},
			{"Ship5",new  bool[4]{true,true,true,true}},
			{"Ship6",new  bool[6]{true,true,true,true,false,true}},
			{"Ship7",new  bool[4]{true,true,true,false}},
			{"Ship8",new  bool[5]{true,true,true,false,false}},

		};
			


		//シップの開放時間　プレイ開始から 何時間で？  1日 3日 7全開放
		public static Dictionary<string,float[]> unlockTimeShip=new Dictionary<string, float[]>(){
			{"Ship1",new float[4]{0f,0f,0f,24*1f}},
			{"Ship2",new float[4]{0f,0f,0.3f,24*1f}},
			{"Ship3",new float[4]{0f,0.3f,24*1f,24*3f}},
			{"Ship4",new float[4]{0.3f,24*1f,24*7*3f,24*7f}},
			{"Ship5",new float[4]{0.3f,24*1f,24*7*3f,24*7f}},
			{"Ship6",new float[6]{24*1f,24*7*3f,24*3f,24*3f,24*3f,24*7f}},
			{"Ship7",new float[4]{24*1f,24*7*3f,24*7f,24*7f}},
			{"Ship8",new float[5]{24*7*2f,24*7*3f,24*7f,24*7f,24*7f}},

		};


		#endregion


		//Shipの値
		#region shipControl
		//最大HP値
		public static readonly float MaxHP=1500.0f;

		public static readonly bool useHitDetectionOnHitter=true;

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
		public static readonly float curePersentageM=50.0f;
		//回復パーセンテージ L
		public static readonly float curePersentageL=100.0f;
		#endregion

		#region normal shots
		//通常弾の  WHITE BLUE YELLOW GREEN RED
		public static readonly float[] shot_damage=new float[5]{100.0f,100.0f,100.0f,120.0f,140.0f};
		public static readonly float[] shot_life=new float[5]{3.0f,3.0f,3.0f,3.0f,3.0f};
		public static readonly float[] shot_speed=new float[5]{2.0f,2.0f,2.0f,2.0f,2.0f};

		#endregion

		#region subweapons
		//サブウェポンの  NAPAM NUKE RAZER STEALTH WAVE YUDOU ZENHOUKOU
		//注意：レーザーはダメージ間隔に与えるダメージ
		public static readonly float[] sw_damage=new float[7]{100.0f,500.0f,5.0f,100.0f,200.0f,100.0f,200.0f};

		//ショットの生存時間
		public static readonly float[] sw_life=new float[7]{1.0f,1.0f,20.0f,20.0f,5.5f,5.5f,5.5f};

		//効果の継続時間
		public static readonly float[] sw_timer=new float[7]{3.0f,5.0f,10.0f,10.0f,5.5f,5.5f,5.5f};

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
			{ Pickup.NAPAM, 12 },
			{ Pickup.NUKE, 1 },
			{ Pickup.RAZER, 9},
			{ Pickup.STEALTH, 8 },
			{ Pickup.WAVE, 10 },
			{ Pickup.YUDOU, 8},
			{ Pickup.ZENHOUKOU, 12},
			{ Pickup.SHOT, 40}
		};

		//スタート後の　スポーン間隔
		public static readonly float spawnRepeatRate=30.0f;

		//プレイヤの数１に対するスポーン数　スタート時　回復
		public static readonly int spawnNum_OnStartPerShip_Rate_Kaifuku=0;
		//サブウェポン
		public static readonly int spawnNum_OnStartPerShip_Rate_Subweapon=5;

		//プレイヤの数１に対するスポーン数　ゲーム開始後の一定間隔　回復
		public static readonly int spawnNum_OnUpdatePerShip_Rate_Kaifuku=1;
		//サブウェポン
		public static readonly int spawnNum_OnUpdatePerShip_Rate_Subweapon=1;

		//一定間隔の追加を何秒後に開始するか  設けないのであれば 0.0fに
		public static readonly float spawnTimeStart_Rate_Kaifuku=600.0f;
		public static readonly float spawnTimeStart_Rate_Subweapon=600.0f;

		//一定間隔の追加を何秒後に終了するか　　設けないのであればとんでもなく大きな値に
		public static readonly float spawnTimeEnd_Rate_Kaifuku=600.0f;
		public static readonly float spawnTimeEnd_Rate_Subweapon=600.0f;
	}

	public static class AppData {

		//アプリのタイトル
		public static readonly string APP_TITTLE="";
		public static readonly string BASE_ENCODE="MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAk0A5/41OdhwKcWmaZP/o3gTNDuoyt7Clek4HU5SYk8jZg7kzpUARLsbQiLii6hLBlwUpB8fBJRRuhw8rGLCqTkT3eYwGVSkG+WU00qecgkso1uiyvU0guLBW4ATExZ/5fpVDaNtpQWDcTje7a6eCUTLNcaUA/rvkhk5umIiPZFg0hW6LbF22T6yXDFrowqAFohWpmeB4Tap2F232OTZ4oZQW14HoglTs93qUiNSZUB8Cc1LtRCPkWIayaMb/0mCaec4tqtOT9QQ8jVvClTRyJYcAZWkf0sMJQNORuc6I+0IET/NIR9b/nInunWX17du81Yc7MGiF6OXlhLJZfskqswIDAQAB";
		public static readonly string APPID="com.Pistolstarweb.spacekill";

		//アプリのURL
		public static readonly string APP_URL="https://play.google.com/store/apps/details?id=com.Pistolstarweb.spacekill";

		//お問い合わせメールアドレス
		public static readonly string MAIL="info@n2gdl.net";
		//特別商取引法の表示
		public static readonly string URL_TOKUSHO="http://smart4me.net/pistolstar/#!hyouji";

		//プライバシーポリシーページ
		public static readonly string URL_POLICY="https://smart4me.net/pistolstar/#!policySP";
		//ランキング用GPGS
		public static readonly string GPGAPPID="399714156908-5nd3ecp45jhv0uiumhfpbbl1gs2nmjfc.apps.googleusercontent.com";

		//Admob  バナー
		public static string BannerAppID="ca-app-pub-7316631869359037~2192184156";

		public static string BannerUnitID="ca-app-pub-7316631869359037/3559082622";
		public static string InterstitialUnitID="ca-app-pub-7316631869359037/9131683944";


		//ツイッターキー
		public static string TwitterAPIKey="1clvpB5q0hZeVGBsiGv6dNdw8";
		public static string TwitterAPISecret="nJRly4zuRxqU9D6Upyb8qTportXwig1G4WxL1DM2SHQIyoxFZl";
		public static string TwitterfollowPageName="Pistol Star";
		public static string TwitterfollowPageId="520988453";


		//Total Kill キル回数　Average Rank 平均順位　Total Winnings 優勝回数
		//1-12月まで
		public static string[] RankingTittels=new string[3]{"平均順位","１位獲得数","キル数"};

		//2018 1-12
		public static Dictionary<string,string[]> RankingIDs=new Dictionary<string, string[]>(){
			{"平均順位",new  string[12]{"CgkI7ILIhtELEAIQAQ","CgkI7ILIhtELEAIQBA","CgkI7ILIhtELEAIQBQ","CgkI7ILIhtELEAIQBg","CgkI7ILIhtELEAIQBw",
					"CgkI7ILIhtELEAIQCA","CgkI7ILIhtELEAIQCQ","CgkI7ILIhtELEAIQCg","CgkI7ILIhtELEAIQCw","CgkI7ILIhtELEAIQDA","CgkI7ILIhtELEAIQDQ","CgkI7ILIhtELEAIQDg"}},
			{"１位獲得数",new  string[12]{"CgkI7ILIhtELEAIQAg","CgkI7ILIhtELEAIQDw","CgkI7ILIhtELEAIQEA","CgkI7ILIhtELEAIQEQ","CgkI7ILIhtELEAIQEg","CgkI7ILIhtELEAIQEw"
					,"CgkI7ILIhtELEAIQFA","CgkI7ILIhtELEAIQFQ","CgkI7ILIhtELEAIQFg","CgkI7ILIhtELEAIQFw","CgkI7ILIhtELEAIQGA","CgkI7ILIhtELEAIQGQ"}},
			{"キル数",new  string[12]{"CgkI7ILIhtELEAIQAA","CgkI7ILIhtELEAIQGg","CgkI7ILIhtELEAIQGw","CgkI7ILIhtELEAIQHA","CgkI7ILIhtELEAIQHQ"
					,"CgkI7ILIhtELEAIQHg","CgkI7ILIhtELEAIQHw","CgkI7ILIhtELEAIQIA","CgkI7ILIhtELEAIQIQ","CgkI7ILIhtELEAIQIg"
					,"CgkI7ILIhtELEAIQIw","CgkI7ILIhtELEAIQJA"}}

		};
		public static Dictionary<string,string[]> PreviousRankingIDs=new Dictionary<string, string[]>(){
			{"平均順位",new  string[1]{"CgkI7ILIhtELEAIQDg"}},
			{"１位獲得数",new  string[1]{"CgkI7ILIhtELEAIQGQ"}},
			{"キル数",new  string[1]{"CgkI7ILIhtELEAIQJA"}}

		};

		public static string GetRankingID(int month,string rankingTittle){
			month--;
			string[] strs;
			if(month>=0){
				strs=RankingIDs[rankingTittle];
				if(strs.Length>month){
					return strs[month];
				}
			}else{
				//前年の12月のデータをだす
				month=0;
				strs=PreviousRankingIDs[rankingTittle];
				if(strs.Length>month){
					return strs[month];
				}
			}

			return "";
		}



		//課金アイテム
		public static readonly string[] IAP_SKUs=new string[11]{
			"5tickets",
			"museigen",
			"ship1sc",
			"ship2sc",
			"ship4sc",
			"ship6pan",
			"ship6sak",
			"ship7met",
			"ship7gld",
			"ship8gld",
			"ship8sc"
		};

		//課金アイテムは消費可能か
		public static readonly bool[] IAP_Comsumable=new bool[11]{
			true,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false

		};

	}

}
