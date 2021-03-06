﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon;
using System.Linq;

public enum GameState {
	PRE_START, COUNTDOWN, FIGHTING, FINISHED
}

namespace PSPhoton {
	public class GameManager : PunBehaviour {


		public shipControl debugShip;
		public void Test(){
			
			Debug.Log(""+GetPlayerRank(1));
		}


		public static GameManager instance;

		#region item spawn
		//アイテムのスポーン
		public PS_Util.RandomPointGenerater randomPointGenerater;
	
		Pickup GetRandomPU_Kaifuku(){
			int rand=Random.Range(0,101);

			var sorted = PSParams.SpawnItemRates.Rate_Kaifuku.OrderBy((x) => x.Value);  
			int sum=0;
			foreach(var s in sorted){
				sum+=s.Value;
				if(rand<=sum){
					return s.Key;
				}
			}
			return Pickup.CureS;
		}
		Pickup GetRandomPU_Subweapon(){
			int rand=Random.Range(0,101);
			var sorted = PSParams.SpawnItemRates.Rate_Subweapon.OrderBy((x) => x.Value);  

			int sum=0;
			foreach(var s in sorted){
				sum+=s.Value;
				if(rand<=sum){
					return s.Key;
				}
			}

			return Pickup.NAPAM;
		}
		Pickup pu=Pickup.CureS;
		Vector3 pos=Vector3.zero;
		void SpawnKaihukus(int num){
			
			pu=Pickup.CureS;
			pos=Vector3.zero;
			for(int i=0;i<num;i++){
				pu=GetRandomPU_Kaifuku();
				pos=randomPointGenerater.GetRandomPoint();
				pos.y=0.0f;
				SpawnPUInvoke((int)pu,pos);

			}

		}
		void SpawnSubweapons(int num){
			
			pu=Pickup.CureS;
			pos=Vector3.zero;
			for(int i=0;i<num;i++){
				pu= GetRandomPU_Subweapon();
				pos=randomPointGenerater.GetRandomPoint();
				pos.y=0.0f;
				SpawnPUInvoke((int)pu,pos);
			}


		}

		void SpawnSubweaponsNR(int num){
			
			pu=Pickup.CureS;
			pos=Vector3.zero;
			int areaCount=randomPointGenerater.GetBoundsCount();
			Debug.Log("SpawnSubweapons on start area=="+areaCount);

			Debug.Log("Spawn item= "+num);

			int area=0;
			int area2=0;
			int areaBound=0;
			for(int i=0;i<num;i++){
				if(area>=areaCount)area=0;
				pu= GetRandomPU_Subweapon();
				pos=randomPointGenerater.GetRandomPoint(area,areaBound);
				pos.y=0.0f;
				SpawnPUInvoke((int)pu,pos);
				area++;
				area2++;
				if(area2>=areaCount){
					Debug.Log("oner areaCount!");
					area2=0;
					areaBound++;
				}
			}

		}

		void SpawnPUInvoke(int pu,Vector3 position){

			PhotonNetwork.InstantiateSceneObject("PU" + pu, position, Quaternion.identity,0,null);
			/*
			object[] args = new object[]{
				pu,
				position
			};
			photonView.RPC ("SpawnPUAt", PhotonTargets.AllBufferedViaServer,args);*/
		}

		/*[PunRPC]
		void SpawnPUAt(int pu,Vector3 position){
			PickupAndWeaponManager.Instance.SpawnPickUpItem((Pickup)pu,position,Quaternion.identity,null);
		}*/

		void OnItemSpawnUpdate(){
			if(gameTime>PSParams.SpawnItemRates.spawnTimeStart_Rate_Kaifuku && gameTime<PSParams.SpawnItemRates.spawnTimeEnd_Rate_Kaifuku){
				if(PhotonNetwork.isMasterClient)if(PSParams.SpawnItemRates.spawnNum_OnUpdatePerShip_Rate_Kaifuku>0)SpawnKaihukus(PSParams.SpawnItemRates.spawnNum_OnUpdatePerShip_Rate_Kaifuku*PhotonNetwork.playerList.Length);
			}

			if(gameTime>PSParams.SpawnItemRates.spawnTimeStart_Rate_Subweapon && gameTime<PSParams.SpawnItemRates.spawnTimeEnd_Rate_Subweapon){
				if(PhotonNetwork.isMasterClient)if(PSParams.SpawnItemRates.spawnNum_OnUpdatePerShip_Rate_Subweapon>0)SpawnSubweapons(PSParams.SpawnItemRates.spawnNum_OnUpdatePerShip_Rate_Subweapon*PhotonNetwork.playerList.Length);
			}
		}


		public Transform spawnPoints_Parent;

		Transform[] spawnPoints;
		void SetSpawnPoints(){
			spawnPoints=new Transform[spawnPoints_Parent.childCount];
			int i=0;
			foreach(Transform tr in spawnPoints_Parent){
				spawnPoints[i]=tr;
				i++;
			}
		}
		#endregion


		[Tooltip("ロード時点のプレイヤ数")]
		public int loadedPlayers=0;
		[Tooltip("生存時間")]
		public float gameTime = 0;

		[Tooltip("ゲーム開始時の時間")]
		public double startTimestamp = 0;
		private GameState state = GameState.PRE_START;

		#region all ships stateus
		List<shipControl> shipControllers=new List<shipControl>();

		//[SerializeField]
		public List<shipControl.PlayerData>   playerDatas=new List<shipControl.PlayerData>();
		void AddPlayerData(shipControl.PlayerData data){
			playerDatas.Add(data);
		}
		void RemovePlayerData(int id){
			foreach(shipControl.PlayerData data in playerDatas){
				if(data.playerID==id){
					playerDatas.Remove(data);
				}
			}
		}



		public bool GetPlayerConnected(int id){

			for(int i=0;i<playerDatas.Count;i++){
				if(playerDatas[i].playerID==id){
					return playerDatas[i].connected;
				}
			}

			return true;
		}


		void SetPlayerConnected(bool con,int id){

			shipControl.PlayerData temp;
			for(int i=0;i<playerDatas.Count;i++){
				if(playerDatas[i].playerID==id){
					temp=playerDatas[i];
					temp.connected=con;
					playerDatas[i]=temp;
				}
			}
		}
		void SetPlayerDead(int id){

			shipControl.PlayerData temp;
			for(int i=0;i<playerDatas.Count;i++){
				if(playerDatas[i].playerID==id){
					temp=playerDatas[i];
					temp.dead=true;
					playerDatas[i]=temp;
				}
			}
		}
		void SetPlayerAlive(int id,float alive){

			shipControl.PlayerData temp;
			for(int i=0;i<playerDatas.Count;i++){
				if(playerDatas[i].playerID==id){
					temp=playerDatas[i];
					temp.alive=alive;
					playerDatas[i]=temp;
				}
			}
		}	
		int GetAlivePlayerCount(){
			int count=0;
			foreach(shipControl.PlayerData data in playerDatas){
				if(!data.dead && data.connected){
					count++;
				}
			}
			//Debug.Log("GetAlivePlayerCount  "+count);
			return count;
		}
		int GetPlayerRank(int id){
			
			Debug.Log("順位の判定 総プレイヤ数　"+playerDatas.Count);

			playerDatas.Sort(delegate(shipControl.PlayerData a, shipControl.PlayerData b) {

				if(a.connected && b.connected){
					if (a.dead && b.dead){
						if(a.alive==b.alive){
							return 0;
						}else{
							if(a.alive>b.alive){
								return -1;
							}else{
								return 1;
							}
						}
					} 
					else if (a.dead) return 1;
					else if (b.dead) return -1;
				}else{
					if(a.connected)return -1;
					if(b.connected)return 1;
				}


				return 0;
			});

			int count=1;
			foreach(shipControl.PlayerData data in playerDatas){
				
				if(data.playerID==id){
					//同時一位かの判定
					if(count>1)if(isDouji(count-1)) return 1;
					return count;
				}
				count++;
			}

			Debug.Log("順位の判定 "+count+" 位");
			return count;
		}

		//同着順位かの判定
		bool isDouji(int playerNum){
			Debug.Log("同着順位の判定 ");
			if(playerDatas[0].connected == playerDatas[playerNum].connected){
				if(playerDatas[0].connected){
					//同じくコネクト
					if(playerDatas[0].dead == playerDatas[playerNum].dead){
						if(playerDatas[0].dead){
							//同じ死亡時間
							if(playerDatas[0].alive == playerDatas[playerNum].alive){
								Debug.Log("同着順位　同じ死亡時間 ");
								return true;
							}
						}else{
							//同じく生存
							Debug.Log("同着順位　同じく生存 ");
							return true;
						}
					}else{

					}
				}else{
					//同じく非接続
					Debug.Log("同着順位　同じく非接続 ");
					return true;
				}
			}
			Debug.Log("同着順位はない ");
			return false;

		}

		public string GetNameById(int id){
			string name="";
			foreach(shipControl.PlayerData data in playerDatas){
				if(id==data.playerID){
					return data.userName;
					break;

				}
			}
			return "";
		}


	
		int killNum=0;


		public Vector3 GetShipPositionById(int id){
			foreach(shipControl ship in shipControllers){
				if(ship && id==ship.playerData.playerID){
					return ship.transform.position;
					break;

				}
			}
			return Vector3.zero;
		}

		public shipControl GetShipById(int id){
			foreach(shipControl ship in shipControllers){
				if(ship && id==ship.playerData.playerID){
					return ship;
					break;

				}
			}
			return null;
		}
		#endregion




		public void OnPlayerDead(int shipID,int killed,float aliveTime){

			if(isLocalPlayerGameOver)return;
			SetPlayerDead(shipID);
			SetPlayerAlive(shipID,aliveTime);

			UpdateZanki();

			//kill数を追加
			if(killed==PhotonNetwork.player.ID){
				++killNum;
				GUIManager.Instance.SetKills(killNum);
			}

			if (PhotonNetwork.player.ID==shipID) {
				Debug.Log("プレイヤが死亡しました");
					isLocalPlayerGameOver=true;
					string info="";
					if(GetNameById(killed)!=""){
						info=Application.systemLanguage == SystemLanguage.Japanese?  "あなたが"+GetNameById(killed)+"にキルされました"
							:"You were killed by"+GetNameById(killed);
					}else{
						info=Application.systemLanguage == SystemLanguage.Japanese?  "あなたは危険エリアで死亡しました"
							:"You were dead in danger area";
					}

				GUIManager.Instance.Log(info);

				state = GameState.FINISHED;

				ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable() { { "spawn", (int)PhotonNetwork.player.CustomProperties["spawn"] },{ "countly",  (string)PhotonNetwork.player.CustomProperties["countly"]  }
					, {"shipBase", (int)PhotonNetwork.player.CustomProperties["shipBase"]}, {"shipColor", (int)PhotonNetwork.player.CustomProperties["shipColor"]},
					{"userName", (string)PhotonNetwork.player.CustomProperties["userName"]},{"isDead", true} };
				
				PhotonNetwork.player.SetCustomProperties(customProperties);

				if(PhotonNetwork.isMasterClient)PhotonNetwork.room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "map",(int)PhotonNetwork.room.CustomProperties["map"]}, { "state",3}  });

				Debug.Log("順位の判定");
				// enable panel
				GUIManager.Instance.OnGameOver(gameTime,killNum,GetPlayerRank(PhotonNetwork.player.ID),playerDatas.Count,playerShip);
			}else{
				Debug.Log("誰かが死にました"+GetNameById(shipID));
				string info="";
				if(GetNameById(killed)!=""){
					info=Application.systemLanguage == SystemLanguage.Japanese? GetNameById(shipID)+"が"+GetNameById(killed)+"にキルされました"
					:GetNameById(shipID)+" was killed by"+GetNameById(killed);
				}else{
					info=Application.systemLanguage == SystemLanguage.Japanese? GetNameById(shipID)+ "は危険エリアで死亡しました"
						:GetNameById(shipID)+"was dead in danger area";
				}
				GUIManager.Instance.Log(info);

				Debug.Log("アクティブシップの判定");
				if(GetAlivePlayerCount()<=1){
					isLocalPlayerGameOver=true;
					Debug.Log("勝ち残りゲームオーバー アクティブシップが存在しないため");
					playerShip.isDead=true;
					playerShip.isControllable=false;

					SetPlayerDead(playerShip.playerData.playerID);

					state = GameState.FINISHED;
					if(PhotonNetwork.isMasterClient)PhotonNetwork.room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "map",(int)PhotonNetwork.room.CustomProperties["map"]}, { "state",2}  });

					// 勝ち残りゲームオーバー
					GUIManager.Instance.OnGameOver(gameTime,killNum,1,playerDatas.Count,playerShip);
				}
			}


		}

		void UpdateZanki(){
			GUIManager.Instance.SetZanki(GetAlivePlayerCount().ToString()+"/"+shipControllers.Count);
		}

		public shipControl playerShip;

		#region init methods
		void Awake(){
			instance=this;

		}

		public bool isLocalPlayerGameOver=false;
		public bool isNetworkMode=true;
		// Use this for initialization
		public UILabel nameLB;
		public UISprite countlySP;
		void Start () {
			

			if(PhotonNetwork.room!=null){
				if((int)PhotonNetwork.room.CustomProperties["state"]>=2){
					//プレイヤ追加分をやる
					PhotonNetwork.LoadLevel ("LobbyScene");
					return;

				}
			}

			if(!isNetworkMode){
				shiphud.SetFollowhipHud(debugShip);
				return;
			}
			GUIManager.Instance.OnGameAwake();

			CreatePlayer();


			photonView.RPC ("ConfirmLoad", PhotonTargets.AllBuffered);
		}

		public mapControl mapControl;
		private void CreatePlayer() {

			nameLB.text=(string)PhotonNetwork.player.CustomProperties["userName"];
			countlySP.spriteName=(string)PhotonNetwork.player.CustomProperties["countly"];

			int pos = (int) PhotonNetwork.player.CustomProperties["spawn"];
			int shipBaseNumber = (int) PhotonNetwork.player.CustomProperties["shipBase"];
			int color=(int) PhotonNetwork.player.CustomProperties["shipColor"];
			int map=(int) PhotonNetwork.room.CustomProperties["map"];
			mapControl.setMapNumber(map);
			mapControl.attachMapData();
			SetSpawnPoints();
			Transform spawn = spawnPoints[pos];

			Debug.Log("プレイヤの作成　position "+pos +" ship="+shipBaseNumber+"カラー="+color);

			//TODO リソースを用意したら、ここを変更する
			GameObject go = PhotonNetwork.Instantiate("Ship" + shipBaseNumber+"c"+(color+1),new Vector3(spawn.position.x,0.0f,spawn.position.z) , Quaternion.Euler(new Vector3(0.0f,spawn.rotation.y,0.0f)), 0);
			go.transform.LookAt(Vector3.zero);
			playerShip=go.GetComponent<shipControl>();
			playerShip.InitPlayerData((string)PhotonNetwork.player.CustomProperties["userName"],(string)PhotonNetwork.player.CustomProperties["countly"],PhotonNetwork.player.ID);
			GUIManager.Instance.SetShipControll(playerShip);
		}

		[PunRPC]
		public void ConfirmLoad () {
			//みんなが1回ずつ呼ぶので、ロード待ち受けに使える
			loadedPlayers++;
			if((int)PhotonNetwork.room.CustomProperties["state"]>=2)SetPlayerHUD();
		}



		private void CheckCountdown() {
			
			bool takingTooLong = gameTime >= PSParams.GameParameters.spawnWaitTimeOnGameStart;
			bool finishedLoading = loadedPlayers == PhotonNetwork.playerList.Length;
			if (takingTooLong || finishedLoading) {
				photonView.RPC ("StartCountdown", PhotonTargets.AllBufferedViaServer, PhotonNetwork.time + 4);
			}
		}

		public PSGUI.WaitHUD waiter;
		[PunRPC]
		public void StartCountdown(double startTimestamp) {
			if(PhotonNetwork.isMasterClient)PhotonNetwork.room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "map",(int)PhotonNetwork.room.CustomProperties["map"]}, { "state",2}  });

			waiter.Hide();
			state = GameState.COUNTDOWN;
			// sets local timestamp to the desired server timestamp (will be checked every frame)
			this.startTimestamp = startTimestamp;
		}

		public AlertLog alert;


		void SetPlayerHUD(){
			GameObject[] ships = GameObject.FindGameObjectsWithTag ("Player");

			Debug.Log("SetPlayerHUD　カウント"+ships.Length);

			shipControl ship;
			foreach (GameObject go in ships) {
				ship=go.GetComponent<shipControl>();
				if(ship && !shipControllers.Contains(ship)){
					if(ship.playerData.playerID!=PhotonNetwork.player.ID){
						shiphud.SetFollowhipHud(ship);

					}
					shipControl.PlayerData data=new shipControl.PlayerData(ship.playerData.userName,ship.playerData.countlyCode,ship.playerData.playerID);
					AddPlayerData(data);
					shipControllers.Add(ship);

				}else{
					Debug.LogWarning("shipControllers added already");
				}

			}

			UpdateZanki();
		}

		public void StartGame() {


			if(PhotonNetwork.isMasterClient){
				if(PSParams.SpawnItemRates.spawnNum_OnStartPerShip_Rate_Kaifuku>0)SpawnKaihukus(PSParams.SpawnItemRates.spawnNum_OnStartPerShip_Rate_Kaifuku*PhotonNetwork.playerList.Length);
				if(PSParams.SpawnItemRates.spawnNum_OnStartPerShip_Rate_Subweapon>0)SpawnSubweaponsNR(PSParams.SpawnItemRates.spawnNum_OnStartPerShip_Rate_Subweapon);
			
			
			}


			//この時点では全てのプレイヤのインスタンスがローカルでも生成されているので、受け取れる

			this.itemSpawnTime=PSParams.SpawnItemRates.spawnRepeatRate;
			this.safeZone_SetDulation=PSParams.GameParameters.safeZone_SetDulation;
			this.safeZone_Dulation=PSParams.GameParameters.safeZone_Dulation;

			SetPlayerHUD();

			//ここで操作系をEnableする
			playerShip.isControllable=true;
			playerShip.StartShooting();


			GUIManager.Instance.OnGameStart();

			if(DataManager.Instance.gameData.gameTickets!=-100)DataManager.Instance.gameData.gameTickets--;

			state = GameState.FIGHTING;
			gameTime = 0;
		}
		#endregion




		#region 安全地帯
		public SafeZone safeZone;
		public SafeZoneMap safeZoneMap;
		float safeZone_SetDulation=30.0f;
		float safeZone_timer=0.0f;
		float safeZone_Dulation=30.0f;
		IntVector2 nextDanzerZone;

		public void OnSetNextDanzerZoneUpdate(){
			if (PhotonNetwork.isMasterClient) {

				IntVector2 invec=safeZone.GetNextDangerZoneRandom();
				if(invec!=null){
					
					object[] args = new object[]{
						invec.x,
						invec.y
					};
					photonView.RPC ("RPC_SetNextDanzerZone", PhotonTargets.AllBufferedViaServer,args);
				}

			}
		}
		public void OnNextDanzerZoneUpdate(){
			if (PhotonNetwork.isMasterClient) {
				photonView.RPC ("RPC_AffectNextDanzerZone", PhotonTargets.AllBufferedViaServer,null);
			}
		}
		[PunRPC]
		public void RPC_SetNextDanzerZone(int x,int y){
			
			safeZone_timer=0.0f;
			nextDanzerZone=new IntVector2();
			nextDanzerZone.x=x;nextDanzerZone.y=y;
			Debug.Log("NextDangerZone= "+x+" "+y);
			safeZoneMap.SetState(nextDanzerZone.x,nextDanzerZone.y,PSParams.GameParameters.mapMasuXY,ZoneState.Pending);
		}
		[PunRPC]
		public void RPC_AffectNextDanzerZone(){
			safeZone_timer=0.0f;
			//TODO ここで危険地帯をアフェクトする
			alert.Hide();
			safeZone.SetSafeZone(nextDanzerZone.x,nextDanzerZone.y,false);
			safeZoneMap.SetState(nextDanzerZone.x,nextDanzerZone.y,PSParams.GameParameters.mapMasuXY,ZoneState.Fixed);
			nextDanzerZone=null;
		}

		#endregion


		float itemSpawnTime=30.0f;
		float repeatedTime=0.0f;
		public UILabel timeLabel;
		int minutes;
		int seconds;
		void Update () {
			if(!isNetworkMode)return;
			gameTime += Time.deltaTime;

			minutes = Mathf.FloorToInt(gameTime / 60F);
			seconds = Mathf.FloorToInt(gameTime - minutes * 60);
			timeLabel.text=string.Format("{0:00}:{1:00}", minutes, seconds);

			switch (state) {
				case GameState.PRE_START:
					if (PhotonNetwork.isMasterClient) {
						CheckCountdown();
					}
					break;
				case GameState.COUNTDOWN:
				
					GUIManager.Instance.SetCountdown( (1 + (int) (startTimestamp - PhotonNetwork.time)));

					if (PhotonNetwork.time >= startTimestamp) {
						StartGame();
					}
					break;
				case GameState.FIGHTING:
					if (gameTime > 3) {

						

					if(PSParams.GameParameters.EndGameOnNoConnection &&GetAlivePlayerCount()<=1){

							playerShip.isDead=true;
							playerShip.isControllable=false;

							SetPlayerDead(playerShip.playerData.playerID);

							state = GameState.FINISHED;
							if(PhotonNetwork.isMasterClient)PhotonNetwork.room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "map",(int)PhotonNetwork.room.CustomProperties["map"]}, { "state",3}  });



					
							// 勝ち残りゲームオーバー
							GUIManager.Instance.OnGameOver(gameTime,killNum,1,playerDatas.Count,playerShip);
						}

						repeatedTime+=Time.deltaTime;
						if(repeatedTime>itemSpawnTime){
							repeatedTime=0.0f;
							OnItemSpawnUpdate();
						}
						
						safeZone_timer+=Time.deltaTime;
						if(nextDanzerZone==null){
							if(safeZone_timer>safeZone_SetDulation){
								safeZone_timer=0.0f;
								OnSetNextDanzerZoneUpdate();
							}
						}else{
							
							if(safeZone_timer>PSParams.GameParameters.safeZone_SetDulation){
								safeZone_timer=0.0f;
								OnNextDanzerZoneUpdate();
							}else{
								if(PSParams.GameParameters.safeZone_SetDulation>=safeZone_timer){
									string text=Mathf.FloorToInt(PSParams.GameParameters.safeZone_SetDulation-safeZone_timer).ToString();
									text+=Application.systemLanguage == SystemLanguage.Japanese? "秒後　安全地帯が制限されます。" :" sec before restriction on safe area";

									alert.UpdateLog(text);
								}
							}
						}
						

					} else {
						repeatedTime=0.0f;
						GUIManager.Instance.SetCountdown(0);
					}
					break;
				case GameState.FINISHED:
					//Show Result
					break;
			}


				
		}


		#region タイムアウトエラー

		public override void OnPhotonPlayerConnected (PhotonPlayer newPlayer)
		{
			Debug.Log ((string)newPlayer.CustomProperties["userName"] + " disconnected...");

			SetPlayerConnected(true,newPlayer.ID);

			base.OnPhotonPlayerConnected (newPlayer);
		}

		public override void OnPhotonPlayerDisconnected(PhotonPlayer disconnetedPlayer) {
			Debug.Log ((string)disconnetedPlayer.CustomProperties["userName"] + " disconnected...");

			SetPlayerConnected(false,disconnetedPlayer.ID);

			if((bool)disconnetedPlayer.CustomProperties["isDead"]==false){
				string info=Application.systemLanguage == SystemLanguage.Japanese? (string)disconnetedPlayer.CustomProperties["userName"] +"との通信が途絶えました"
					:(string)disconnetedPlayer.CustomProperties["userName"] +"was left rooom";

				GUIManager.Instance.Log(info);
			}
		}
		#endregion



		public ShipFollowHudManager shiphud;
		public void BackToMain(){
			//正常な終了
			state =GameState.FINISHED;
			DataManager.Instance.gameData.isConnectingRoom=false;
			DataManager.Instance.SaveAll();
			PhotonNetwork.LeaveRoom ();

		}

		public override void OnLeftRoom(){ 
			Debug.Log("OnLeftRoom was called");

			if(state == GameState.FINISHED){
				PhotonNetwork.LoadLevel ("LobbyScene");
			}else{
				
				if(isRoomExists(DataManager.Instance.gameData.lastRoomName)){
					//ゲームが終了していたら入れない
					if((int)PhotonNetwork.room.CustomProperties["state"]>=3){
						Debug.LogWarning("ゲーム終了のため入れない");
						PhotonNetwork.LoadLevel ("LobbyScene");
					}else{
						PhotonNetwork.ReconnectAndRejoin();
					}

				}else{
					//部屋が存在しない
					Debug.LogWarning("部屋が存在しない");
					PhotonNetwork.LoadLevel ("LobbyScene");
				}
			}

		}

		public override void OnFailedToConnectToPhoton (DisconnectCause cause)
		{
			Debug.Log("OnFailedToConnectToPhoton");
			//Do Nothing
			base.OnFailedToConnectToPhoton (cause);
		}

		bool isRoomExists(string roomName){
			Debug.Log("isRoomExists"+roomName);
			List<RoomInfo> roomList = PhotonNetwork.GetRoomList().ToList();
			RoomInfo room  = roomList.FirstOrDefault(r => r.Name == roomName);
			bool exists = (room != null);

			return exists;
		}



	}
}
