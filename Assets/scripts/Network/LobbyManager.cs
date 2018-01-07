using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon;


namespace PSPhoton {
	
	[RequireComponent (typeof (PhotonView))]
	public class LobbyManager : PunBehaviour {

		private const string APP_VERSION="v1.0";

		public byte maxPlayers=4;

		[Tooltip("接続切れの場合何ミリ秒間プレイヤインスタンスを破棄せずに待つか")]
		public int playerTTL=60000;

		public bool useDebugLog=false;


		public List<RoomInfo> rooms = new List<RoomInfo>();

		public ShipSelecter shipsSelecter;
		public LobbyStateHUD stateHUD;
		public PS_GUI_DynamicInfo info;
		public PS_GUI_Cover cover;

		// Use this for initialization
		void Start () {
			stateHUD.SetStateHUD(NetworkState.DISCONNECTED);

			//ゲームから戻った時、すでにコネクトされている leaveroomあとは　severconnected状態に
			if (PhotonNetwork.connected) {
				
				Debug.LogWarning("コネクト済");
				stateHUD.SetStateHUD(NetworkState.SERVERCONNECTED);
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
			PhotonNetwork.player.NickName = DataManager.Instance.gameData.username;
			stateHUD.SetAnime(Application.systemLanguage == SystemLanguage.Japanese? "サーバに接続中" :"Connecting Server");
			PhotonNetwork.ConnectUsingSettings(APP_VERSION);
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
			base.OnDisconnectedFromPhoton();
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
			if(stateHUD.networkState!=NetworkState.LOBBYCONNECTED)return;
			Debug.LogWarning("ここでプレイボタンを消す");
			Debug.LogWarning("ここでタイマーを出す");

			//RandomJoinし、ダメなら部屋をつくる

			if(rooms.Count>0){
				//部屋がないので、作る
				CreateRoom(PhotonNetwork.player.NickName);
			}else{
				
			}

		}



		//自分が部屋を作った場合
		public void CreateRoom(string roomName){
			RoomOptions options = new RoomOptions();
			options.MaxPlayers = maxPlayers;
			options.PlayerTtl = playerTTL;
			PhotonNetwork.CreateRoom(roomName, options, TypedLobby.Default);
		}

		public override void OnPhotonCreateRoomFailed(object[] codeMessage) {
			if(useDebugLog)Debug.Log("OnPhotonCreateRoomFailed");

			if ((short) codeMessage [0] == ErrorCode.GameIdAlreadyExists) {
				//すでに同じ名前のルームあり
				if(useDebugLog)Debug.Log("すでに同じ名前のルームあり");
				PhotonNetwork.playerName += "-2";
				CreateRoom (PhotonNetwork.player.NickName);
			}
		}

		// (masterClient only) 作った人だけに呼ばれる
		public override void OnCreatedRoom () {
			if(useDebugLog)Debug.Log("OnCreatedRoom successed");
			SetCustomProperties(PhotonNetwork.player, shipsSelecter.currentSelect, PhotonNetwork.playerList.Length - 1);
		}
		// If master client, for every newly connected player, sets the custom properties for him
		// car = 0, position = last (size of player list)
		public override void OnPhotonPlayerConnected (PhotonPlayer newPlayer) {
			if (PhotonNetwork.isMasterClient) {
				SetCustomProperties (newPlayer, 0, PhotonNetwork.playerList.Length - 1);
				//photonView.RPC("UpdateTrack", PhotonTargets.All, trackIndex);
			}
		}

		// when a player disconnects from the room, update the spawn/position order for all
		public override void OnPhotonPlayerDisconnected(PhotonPlayer disconnetedPlayer) {
			if (PhotonNetwork.isMasterClient) {
				int playerIndex = 0;
				foreach (PhotonPlayer p in PhotonNetwork.playerList) {
					SetCustomProperties(p, (int) p.CustomProperties["ship"], playerIndex++);
				}
			}
		}

		// sets and syncs custom properties on a network player (including masterClient)
		private void SetCustomProperties(PhotonPlayer player, int ship, int position) {
			ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable() { { "spawn", position }, {"ship", ship} };
			player.SetCustomProperties(customProperties);
		}

		public override void OnPhotonPlayerPropertiesChanged (object[] playerAndUpdatedProps) {
			UpdatePlayerList ();
		}

		// self-explainable
		public void UpdatePlayerList() {
			foreach (PhotonPlayer p in PhotonNetwork.playerList) {
				// enable car choose option for local player
				if (p == PhotonNetwork.player) {
					
				}
				// updates icon based on car index (protected for early calls before a new player has own properties set)
				if (p.CustomProperties.ContainsKey("ship")) {
					//[(int) p.CustomProperties["car"]];
					//p.NickName.Trim();
				}
			}
		}




		public void JoinRandom(){
			PhotonNetwork.JoinRandomRoom();
		}

		public override void OnJoinedRoom ()
		{
			if(useDebugLog)Debug.Log("OnJoindRoom successed");
			SetCustomProperties(PhotonNetwork.player, shipsSelecter.currentSelect, PhotonNetwork.playerList.Length - 1);
			base.OnJoinedRoom ();
		}

		public override void OnPhotonRandomJoinFailed (object[] codeAndMsg)
		{
			if(useDebugLog)Debug.Log("OnJoindRoom failled");
			base.OnPhotonRandomJoinFailed (codeAndMsg);
		}





		//
		[PunRPC]
		public void UpdateTrack(int index) {
			
		}




		// masterClient only. Calls an RPC to start the race on all clients. Called from GUI
		public void CallStartGame() {
			PhotonNetwork.room.IsOpen = false;
			photonView.RPC("LoadGame", PhotonTargets.All);
		}

		[PunRPC]
		public void LoadGame () {
			//PhotonNetwork.LoadLevel("Race" + trackIndex);
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
		public void ResetToMenu () {
			PhotonNetwork.LeaveRoom ();
			//PhotonNetwork.LoadLevel ("Menu");
		}

	}
}
