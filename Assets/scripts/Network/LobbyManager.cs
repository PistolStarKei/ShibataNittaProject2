﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon;


namespace PSPhoton {
	
	[RequireComponent (typeof (PhotonView))]
	public class LobbyManager : PunBehaviour {

		public static LobbyManager instance;

		private const string APP_VERSION="v1.0";

		public byte maxPlayers=4;
		public byte minPlayers=4;

		[Tooltip("接続切れの場合何ミリ秒間プレイヤインスタンスを破棄せずに待つか")]
		public int playerTTL=60000;
		public float checkTimeOnRoom;
		public PS_GUI_TimerSlider timer;
		public GameObject TimerBtn;

		public bool useDebugLog=false;


		public List<RoomInfo> rooms = new List<RoomInfo>();

		public ShipSelecter shipsSelecter;
		public LobbyStateHUD stateHUD;
		public PS_GUI_DynamicInfo info;
		public PS_GUI_Cover cover;

		public PS_GUI_InputValidater nameInput;
		// Use this for initialization




		void Awake(){
			instance=this;
		}
		void Start () {

			stateHUD.SetStateHUD(NetworkState.DISCONNECTED);



			//ゲームから戻った時、すでにコネクトされている leaveroomあとは　severconnected状態に
			if (PhotonNetwork.connected) {
				
				Debug.LogWarning("コネクト済");
				stateHUD.SetStateHUD(NetworkState.SERVERCONNECTED);
				JoinLobby();
				return;
			}

			ConnectToPUN();
		}



		public override void OnReceivedRoomListUpdate () {
			rooms.Clear ();

			int i = 0;
			foreach (RoomInfo room in PhotonNetwork.GetRoomList()) {
				if (room.IsOpen)rooms.Add(room);
				//room.Name;
				//room.PlayerCount
				//room.MaxPlayers
			}
			if(useDebugLog)Debug.Log("OnReceivedRoomListUpdate has"+rooms.Count);

		}


		//ニックネームを入力してからでは
		public void ConnectToPUN(){
			PhotonNetwork.player.NickName = DataManager.Instance.gameData.userID;

			stateHUD.SetAnime(Application.systemLanguage == SystemLanguage.Japanese? "サーバに接続中" :"Connecting Server");
			//PhotonNetwork.ConnectUsingSettings(APP_VERSION);
			PhotonNetwork.ConnectToBestCloudServer(APP_VERSION);

		}
		public override void OnConnectedToPhoton ()
		{
			if(useDebugLog)Debug.Log("OnConnectedToPhoton succcess");

			stateHUD.SetStateHUD(NetworkState.SERVERCONNECTED);
			base.OnConnectedToPhoton ();
		}
		public override void OnFailedToConnectToPhoton (DisconnectCause cause)
		{
			stateHUD.SetStateHUD(NetworkState.DISCONNECTED);
			base.OnFailedToConnectToPhoton (cause);
		}

		//autojoin =falseの時だけ呼ばれる
		public override void OnConnectedToMaster ()
		{
			if(useDebugLog)Debug.Log("OnConnectedToMaster");
			JoinLobby();
			base.OnConnectedToMaster();
		}

		public override void OnDisconnectedFromPhoton ()
		{
			if(useDebugLog)Debug.Log("OnDisconnectedFromPhoton");
			stateHUD.SetStateHUD(NetworkState.DISCONNECTED);

			stateHUD.SetAnime(Application.systemLanguage == SystemLanguage.Japanese? "再接続中" :"Reconneting");
			Invoke("ConnectToPUN",2.0f);

			base.OnDisconnectedFromPhoton();
		}

		public override void OnPhotonMaxCccuReached ()
		{
			info.Log(Application.systemLanguage == SystemLanguage.Japanese? "サーバ混雑のため接続できませんでした" :"server is too busy now");
			
			base.OnPhotonMaxCccuReached ();
		}




		public void JoinLobby(){
			stateHUD.SetAnime(Application.systemLanguage == SystemLanguage.Japanese? "ロビーに接続中" :"Entering Lobby");
			PhotonNetwork.JoinLobby ();
		}


		public override void OnJoinedLobby () {
			if(useDebugLog)Debug.Log("Joined Lobby succcess");
			stateHUD.SetStateHUD(NetworkState.LOBBYCONNECTED);
		}



		public void OnClickPlayBtn(){

			if(stateHUD.networkState!=NetworkState.LOBBYCONNECTED){
				if(useDebugLog)Debug.LogWarning("ロビーにコネクトされていないので不可");
				return;
			}


			if(DataManager.Instance.gameData.isConnectingRoom){
					Debug.LogWarning("ここで再接続をなんとかする");

			}
		

			HUDOnROOM(true);
			//RandomJoinし、ダメなら部屋をつくる

			if(rooms.Count<=0){
				//部屋がないので、作る
				CreateRoom(PhotonNetwork.player.NickName);
			}else{
				PhotonNetwork.JoinRandomRoom();
			}

		}



		//自分が部屋を作った場合
		public void CreateRoom(string roomName){
			RoomOptions options = new RoomOptions();
			options.MaxPlayers = maxPlayers;
			options.PlayerTtl = playerTTL;

			//ここでマップをランダムに設定する
			int mapNum=0;
			options.CustomRoomProperties=new ExitGames.Client.Photon.Hashtable() { { "map",mapNum} };

			PhotonNetwork.CreateRoom(roomName, options, TypedLobby.Default);
		}

		public override void OnPhotonCreateRoomFailed(object[] codeMessage) {
			if(useDebugLog)Debug.Log("OnPhotonCreateRoomFailed");

			if ((short) codeMessage [0] == ErrorCode.GameIdAlreadyExists) {
				//すでに同じ名前のルームあり
				if(useDebugLog)Debug.Log("すでに同じ名前のルームあり");
				PhotonNetwork.playerName += "-2";
				CreateRoom (PhotonNetwork.player.NickName);
			}else{
				
				HUDOnROOM(false);
			}
		}
		public bool isMasterClient=false;
		// (masterClient only) 作った人だけに呼ばれる
		public override void OnCreatedRoom () {
			isMasterClient=true;
			if(useDebugLog)Debug.Log("OnCreatedRoom successed");
			_timerFlag=0.0f;
			_timerFrequency=0.0f;

			SetCustomProperties(PhotonNetwork.player, shipsSelecter.currentSelect,DataManager.Instance.gameData.country, PhotonNetwork.playerList.Length - 1,DataManager.Instance.gameData.username);

		}
		// If master client, for every newly connected player, sets the custom properties for him
		// car = 0, position = last (size of player list)
		public override void OnPhotonPlayerConnected (PhotonPlayer newPlayer) {
			if(useDebugLog)Debug.Log("OnPhotonPlayerConnected "+(string)newPlayer.CustomProperties["userName"]);
			/*if (PhotonNetwork.isMasterClient) {
				SetCustomProperties (newPlayer, 0,"JP", PhotonNetwork.playerList.Length - 1);
				//photonView.RPC("UpdateTrack", PhotonTargets.All, trackIndex);
			}*/



			info.Log(Application.systemLanguage == SystemLanguage.Japanese? (string)newPlayer.CustomProperties["userName"]+"が参戦決定しました" :(string)newPlayer.CustomProperties["userName"]+" joins this battle");
		}

		public override void OnMasterClientSwitched (PhotonPlayer newMasterClient)
		{
			if(newMasterClient==PhotonNetwork.player){
				if(useDebugLog)info.Log("MC変更：MCになりました");
				isMasterClient=true;
			}
			base.OnMasterClientSwitched (newMasterClient);
		}
		// when a player disconnects from the room, update the spawn/position order for all
		public override void OnPhotonPlayerDisconnected(PhotonPlayer disconnetedPlayer) {
			if(useDebugLog)Debug.Log("OnPhotonPlayerDisconnected "+(string)disconnetedPlayer.CustomProperties["userName"]);
			if (PhotonNetwork.isMasterClient) {
				int playerIndex = 0;
				foreach (PhotonPlayer p in PhotonNetwork.playerList) {
					SetCustomProperties(p, (int) p.CustomProperties["shipBase"],(string)p.CustomProperties["countly"], playerIndex++,(string)p.CustomProperties["userName"]);
				}
			}

			info.Log(Application.systemLanguage == SystemLanguage.Japanese? (string)disconnetedPlayer.CustomProperties["userName"]+"が撤退しました" :(string)disconnetedPlayer.CustomProperties["userName"]+" leaves this battle");

		}

		// sets and syncs custom properties on a network player (including masterClient)
		private void SetCustomProperties(PhotonPlayer player, int ship,string countly, int position,string name) {
			if(useDebugLog)Debug.Log("SetCustomProperties "+name+" ship" + ship+" "+countly+" poisition "+position);
			ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable() { { "spawn", position },{ "countly", countly }, {"shipBase", ship},{"userName", name} };
			player.SetCustomProperties(customProperties);
		}

		public override void OnPhotonPlayerPropertiesChanged (object[] playerAndUpdatedProps) {
			if(useDebugLog)Debug.Log("OnPhotonPlayerPropertiesChanged ");
			UpdatePlayerList ();
		}
			


		// self-explainable
		public void UpdatePlayerList() {
			if(useDebugLog)Debug.Log("UpdatePlayerList "+PhotonNetwork.playerList.Length);

			int i=0;
			foreach (PhotonPlayer p in PhotonNetwork.playerList) {
				// enable car choose option for local player
				if (p == PhotonNetwork.player) {
					//自分です
				}else{
					lobbyList.ClearList();


					if (p.CustomProperties.ContainsKey("userName") && p.CustomProperties.ContainsKey("countly")) {
						lobbyList.AddList((string)p.CustomProperties["userName"],i,(string) p.CustomProperties["countly"]);

					}else{
						lobbyList.AddList("",i,"UN");
					}

					i++;
				}

				if(isMasterClient){
					if(PhotonNetwork.playerList.Length==maxPlayers){
						if(useDebugLog)Debug.Log("最大人数に達したため、プレイ開始します");
						isMasterClient=false;
						CallStartGame();
						return;
					}
				}

			}
		}

		public LobbyListManager lobbyList;



		public void JoinRandom(){
			PhotonNetwork.JoinRandomRoom();
		}

		public override void OnJoinedRoom ()
		{
			if(useDebugLog)Debug.Log("OnJoindRoom successed");
			stateHUD.SetStateHUD(NetworkState.ROOMCONNECTED);
			SetCustomProperties(PhotonNetwork.player, shipsSelecter.currentSelect,DataManager.Instance.gameData.country, PhotonNetwork.playerList.Length - 1,DataManager.Instance.gameData.username);
			base.OnJoinedRoom ();
		}


		public override void OnPhotonRandomJoinFailed (object[] codeAndMsg)
		{
			if(useDebugLog)Debug.Log("OnJoindRoom failled");

			HUDOnROOM(false);

			base.OnPhotonRandomJoinFailed (codeAndMsg);
		}

		void HUDOnROOM(bool isInRoom){

			if(isInRoom){
				_timerFlag=0.0f;
				isMasterClient=false;
				lobbyList.ClearList();
				DataManager.Instance.gameData.isConnectingRoom=true;
				TimerBtn.SetActive(true);
				timer.SetTime(checkTimeOnRoom,checkTimeOnRoom);
				timer.SetTittle(Application.systemLanguage == SystemLanguage.Japanese? "参戦受付中" :"Matching");
			}else{
				lobbyList.ClearList();
				DataManager.Instance.gameData.isConnectingRoom=false;
				TimerBtn.SetActive(false);
			}
		}



		public void OnShipChanged(int num){
			if(stateHUD.networkState==NetworkState.ROOMCONNECTED){
				if(PhotonNetwork.player.CustomProperties.ContainsKey("spawn")){
					SetCustomProperties(PhotonNetwork.player, num,(string)PhotonNetwork.player.CustomProperties["countly"], (int)PhotonNetwork.player.CustomProperties["spawn"],(string)PhotonNetwork.player.CustomProperties["userName"]);
				}
			}

		}

		public void OnUserNameChanged(string name){
			if(stateHUD.networkState==NetworkState.ROOMCONNECTED){
				if(PhotonNetwork.player.CustomProperties.ContainsKey("spawn")){
					SetCustomProperties(PhotonNetwork.player, (int)PhotonNetwork.player.CustomProperties["shipBase"],(string)PhotonNetwork.player.CustomProperties["countly"], (int)PhotonNetwork.player.CustomProperties["spawn"],name);
				}
			}

		}


		float _timerFlag=0.0f;
		float _timerFrequency=0.0f;

		void Update(){
			if(!isMasterClient || !timer.gameObject.activeSelf)return;


			_timerFlag+=Time.deltaTime;
			_timerFrequency+=Time.deltaTime;

			if(_timerFlag>checkTimeOnRoom){
				_timerFlag=0.0f;
				if(PhotonNetwork.playerList.Length>=minPlayers){
					if(useDebugLog)Debug.Log("最低人数に達したため、プレイ開始します");
					isMasterClient=false;
					CallStartGame();
					return;
				}
			}

			if(_timerFrequency>0.5f){
				_timerFrequency=0.0f;
				CallUpdateTimer(_timerFlag);
			}

		}



		public void CallUpdateTimer(float time) {
			object[] args = new object[]{
				checkTimeOnRoom-time,
				_timerFlag
			};

			photonView.RPC("UpdateTimer", PhotonTargets.All,args);
		}
		[PunRPC]
		public void UpdateTimer (float lastTime,float timerFlag) {
			_timerFlag=timerFlag;
			timer.SetTime(lastTime,checkTimeOnRoom);
		}


		// masterClient only. Calls an RPC to start the race on all clients. Called from GUI
		public void CallStartGame() {
			PhotonNetwork.room.IsOpen = false;
			photonView.RPC("LoadGame", PhotonTargets.All);
		}
		public string GameSceneName;

		[PunRPC]
		public void LoadGame () {
			int map=(int)PhotonNetwork.room.CustomProperties["map"];
			GameSceneName=GameSceneName;
			Debug.LogWarning("ここでMapに応じて出し分ける");

			sceneFader.FadeOut(OnSceneFaded,true);
		}

		public PSGUI.SceneFader sceneFader;

		public void OnSceneFaded(){
			PhotonNetwork.LoadLevel(GameSceneName);
		}




		private bool checkSameNameOnPlayersList(string name) {
			foreach (PhotonPlayer pp in PhotonNetwork.otherPlayers) {
				if (pp.NickName.Equals(name)) {
					return true;
				}
			}

			return false;
		}

		//ロビーは出ずに戻る
		public void LeaveRoom () {
			PhotonNetwork.LeaveRoom ();
			//PhotonNetwork.LoadLevel ("Menu");
		}



	}
}
