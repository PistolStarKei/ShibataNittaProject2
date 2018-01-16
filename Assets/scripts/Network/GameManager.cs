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

		public static GameManager instance;


		//アイテムのスポーン
		public PS_Util.RandomPointGenerater randomPointGenerater;
	
		Pickup GetRandomPU_Kaifuku(){
			int rand=Random.Range(0,101);

			var sorted = SpawnItemRates.Rate_Kaifuku.OrderBy((x) => x.Value);  
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
			var sorted = SpawnItemRates.Rate_Subweapon.OrderBy((x) => x.Value);  

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

		void SpawnPUInvoke(int pu,Vector3 position){
			object[] args = new object[]{
				pu,
				position
			};
			photonView.RPC ("SpawnPUAt", PhotonTargets.AllBufferedViaServer,args);
		}

		[PunRPC]
		void SpawnPUAt(int pu,Vector3 position){
			PickupAndWeaponManager.Instance.SpawnPickUpItem((Pickup)pu,position,Quaternion.identity,null);
		}

		void OnItemSpawnUpdate(){
			if(gameTime>SpawnItemRates.spawnTimeStart_Rate_Kaifuku && gameTime<SpawnItemRates.spawnTimeEnd_Rate_Kaifuku){
				if(PhotonNetwork.isMasterClient)if(SpawnItemRates.spawnNum_OnUpdatePerShip_Rate_Kaifuku>0)SpawnKaihukus(SpawnItemRates.spawnNum_OnUpdatePerShip_Rate_Kaifuku*PhotonNetwork.playerList.Length);
			}

			if(gameTime>SpawnItemRates.spawnTimeStart_Rate_Subweapon && gameTime<SpawnItemRates.spawnTimeEnd_Rate_Subweapon){
				if(PhotonNetwork.isMasterClient)if(SpawnItemRates.spawnNum_OnUpdatePerShip_Rate_Subweapon>0)SpawnSubweapons(SpawnItemRates.spawnNum_OnUpdatePerShip_Rate_Subweapon*PhotonNetwork.playerList.Length);
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

		[Tooltip("ロード時点のプレイヤ数")]
		public int loadedPlayers=0;
		[Tooltip("生存時間")]
		public float gameTime = 0;

		[Tooltip("ゲーム開始時の時間")]
		public double startTimestamp = 0;
		private GameState state = GameState.PRE_START;

		public List<shipControl> shipControllers=new List<shipControl>();

		int killNum=0;

		string GetNameById(int id){
			string name="";
			foreach(shipControl ship in shipControllers){
				if(id==ship.playerData.playerID){
					return ship.playerData.userName;
					break;

				}
			}
			return "";
		}

		public List<shipControl> deadShips=new List<shipControl>();
		public void OnPlayerDead(shipControl ship,int killed){
			
			deadShips.Add(ship);
			UpdateZanki();

			//kill数を追加
			if(killed==PhotonNetwork.player.ID){
				++killNum;
				GUIManager.Instance.SetKills(killNum);
			}

			if (playerShip==ship) {
				Debug.Log("プレイヤが死亡しました");

				string info=Application.systemLanguage == SystemLanguage.Japanese?  "Playerが"+GetNameById(killed)+"にキルされました"
					:"Player was killed by"+GetNameById(killed);

				GUIManager.Instance.Log(info);

				state = GameState.FINISHED;
				// enable panel
				GUIManager.Instance.OnGameOver(gameTime,killNum,deadShips,shipControllers,playerShip);
			}else{
				Debug.Log("誰かが死にました"+ship.name);

				string info=Application.systemLanguage == SystemLanguage.Japanese? ship.playerData.userName+"が"+GetNameById(killed)+"にキルされました"
					:ship.playerData.userName+" was killed by"+GetNameById(killed);

				GUIManager.Instance.Log(info);

				if(shipControllers.Count-deadShips.Count<=1){
					
					playerShip.isDead=true;
					playerShip.isControllable=false;

					deadShips.Add(playerShip);

					state = GameState.FINISHED;
					// 勝ち残りゲームオーバー
					GUIManager.Instance.OnGameOver(gameTime,killNum,deadShips,shipControllers,playerShip);
				}
			}


		}

		void UpdateZanki(){
			GUIManager.Instance.SetZanki((shipControllers.Count-deadShips.Count)+"/"+shipControllers.Count);
		}

		shipControl playerShip;

		void Awake(){
			instance=this;
			SetSpawnPoints();
		}

		public bool isNetworkMode=true;
		// Use this for initialization
		void Start () {
			

			if(PhotonNetwork.isMasterClient){
				if(SpawnItemRates.spawnNum_OnStartPerShip_Rate_Kaifuku>0)SpawnKaihukus(SpawnItemRates.spawnNum_OnStartPerShip_Rate_Kaifuku*PhotonNetwork.playerList.Length);
				if(SpawnItemRates.spawnNum_OnStartPerShip_Rate_Subweapon>0)SpawnSubweapons(SpawnItemRates.spawnNum_OnStartPerShip_Rate_Subweapon*PhotonNetwork.playerList.Length);
			}

			AudioController.PlayMusic("gameBGM");

			if(!isNetworkMode)return;
			GUIManager.Instance.OnGameAwake();

			CreatePlayer();


			photonView.RPC ("ConfirmLoad", PhotonTargets.All);
		}
		
		private void CreatePlayer() {

			int pos = (int) PhotonNetwork.player.CustomProperties["spawn"];
			int shipBaseNumber = (int) PhotonNetwork.player.CustomProperties["shipBase"];
			Transform spawn = spawnPoints[pos];

			GameObject go = PhotonNetwork.Instantiate("Ship" + shipBaseNumber, spawn.position, spawn.rotation, 0);
			playerShip=go.GetComponent<shipControl>();
			playerShip.InitPlayerData(PhotonNetwork.player.NickName,(string)PhotonNetwork.player.CustomProperties["countly"],PhotonNetwork.player.ID);
			GUIManager.Instance.SetShipControll(playerShip);
		}

		[PunRPC]
		public void ConfirmLoad () {
			//みんなが1回ずつ呼ぶので、ロード待ち受けに使える
			loadedPlayers++;
		}
		public float itemSpawnTime=30.0f;
		float repeatedTime=0.0f;
		void Update () {
			gameTime += Time.deltaTime;

			switch (state) {
				case GameState.PRE_START:
					if (PhotonNetwork.isMasterClient) {
						CheckCountdown();
					}
					break;
				case GameState.COUNTDOWN:
				
					GUIManager.Instance.SetCountdown("" + (1 + (int) (startTimestamp - PhotonNetwork.time)));

					if (PhotonNetwork.time >= startTimestamp) {
						StartGame();
					}
					break;
				case GameState.FIGHTING:
					if (gameTime > 3) {
						repeatedTime+=Time.deltaTime;
						if(repeatedTime>itemSpawnTime){
							repeatedTime=0.0f;
							OnItemSpawnUpdate();
						}
						GUIManager.Instance.SetCountdown("");
					} else {
						repeatedTime=0.0f;
						GUIManager.Instance.SetCountdown("Fight!");
					}
					break;
				case GameState.FINISHED:
					//Show Result
					break;
			}



		}

		private void CheckCountdown() {
			bool takingTooLong = gameTime >= 5;
			bool finishedLoading = loadedPlayers == PhotonNetwork.playerList.Length;
			if (takingTooLong || finishedLoading) {
				photonView.RPC ("StartCountdown", PhotonTargets.All, PhotonNetwork.time + 4);
			}
		}

		[PunRPC]
		public void StartCountdown(double startTimestamp) {
			state = GameState.COUNTDOWN;
			// sets local timestamp to the desired server timestamp (will be checked every frame)
			this.startTimestamp = startTimestamp;
		}




		public void StartGame() {


			if (PhotonNetwork.isMasterClient) {
				//ここでアイテムをスポーンする

			}

			//この時点では全てのプレイヤのインスタンスがローカルでも生成されているので、受け取れる


			GameObject[] ships = GameObject.FindGameObjectsWithTag ("Player");

			shipControl ship;
			foreach (GameObject go in ships) {
				ship=go.GetComponent<shipControl>();
				if(ship){
					if(ship!=playerShip)shiphud.SetFollowhipHud(ship);
					shipControllers.Add(ship);
				}else{
					Debug.LogError("shipControllerがアタッチされていない");
				}

			}

			UpdateZanki();

			//ここで操作系をEnableする
			playerShip.isControllable=true;
			//playerShip.StartShooting();

			GUIManager.Instance.OnGameAwake();
			GUIManager.Instance.OnGameStart();

			state = GameState.FIGHTING;
			gameTime = 0;
		}
		public ShipFollowHudManager shiphud;

		public override void OnMasterClientSwitched (PhotonPlayer newMasterClient)
		{
			
		}

		public override void OnPhotonPlayerDisconnected(PhotonPlayer disconnetedPlayer) {
			Debug.Log (disconnetedPlayer.NickName + " disconnected...");

			/*shipControl toRemove = null;
			foreach (shipControl rc in carControllers) {
				//Debug.Log (rc.photonView.owner);
				if (rc.photonView.owner == null) {
					toRemove = rc;
				}
			}
			// remove car controller of disconnected player from the list
			shipControllers.Remove (toRemove);
			*/
		}

		// Use this to go back to the menu, without leaving the lobby
		public void ResetToMenu () {
			PhotonNetwork.LeaveRoom ();
			//PhotonNetwork.LoadLevel ("Menu");
		}

		public void BackToMain(){
			PhotonNetwork.LoadLevel ("LobbyScene");
		}

	}
}
