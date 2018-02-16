using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon;


namespace PSPhoton {
	
	[RequireComponent (typeof (PhotonView))]
	public class LobbyManager : PunBehaviour{

		public static LobbyManager instance;

		private const string APP_VERSION="v1.0";

		public byte maxPlayers=4;
		public byte minPlayers=4;

		[Tooltip("接続切れの場合何ミリ秒間プレイヤインスタンスを破棄せずに待つか")]
		public int playerTTL=60000;
		public int roomTTL=0;
		public float checkTimeOnRoom;
		public PS_GUI_TimerSlider timer;

		public bool useDebugLog=false;


		public List<RoomInfo> rooms = new List<RoomInfo>();

		public LobbyShipSwitcher shipsSelecter;
		public LobbyStateHUD stateHUD;
		public PS_GUI_DynamicInfo info;
		public PS_GUI_Cover cover;

		public PS_GUI_InputValidater nameInput;
		// Use this for initialization




		void Awake(){
			GameObject audio=GameObject.FindGameObjectWithTag("AudioController");
			if(!audio){
				GameObject.Instantiate(audioControllerObj,Vector3.zero,Quaternion.identity,null);
			}
			instance=this;

			if(InternetReachabilityVerifier.Instance==null){
				GameObject.Instantiate(internetCheker,Vector3.zero,Quaternion.identity,null);
			}
		}

		public GameObject internetCheker;
		public GameObject audioControllerObj;
		void Start () {

			if(DataManager.Instance.gameData.gameTickets!=-100)AdManager.Instance.ShowBanner();

			AudioController.PlayMusic("LobbbyMusic");
			PhotonNetwork.CrcCheckEnabled = true;

			stateHUD.SetStateHUD(NetworkState.DISCONNECTED);

			Debug.LogWarning("ここでランキングがあれば見せる");

			//ゲームから戻った時、すでにコネクトされている leaveroomあとは　severconnected状態に
			if (PhotonNetwork.connected) {

				if(DataManager.Instance.gameData.gameTickets!=-100 && Random.Range(0,3)==0)AdManager.Instance.ShowInterstitial();

				stateHUD.SetStateHUD(NetworkState.SERVERCONNECTED);
				if(useDebugLog)Debug.Log("ロビーに再接続");

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

			stateHUD.SetAnime(Application.systemLanguage == SystemLanguage.Japanese? "接続中" :"Connecting");
			//PhotonNetwork.ConnectUsingSettings(APP_VERSION);
			//PhotonNetwork.ConnectToBestCloudServer(APP_VERSION);

			PhotonNetwork.ConnectToRegion(Countly.ServerRegion[DataManager.Instance.envData.serverRegion],APP_VERSION);

		}
		public override void OnConnectedToPhoton ()
		{
			if(useDebugLog)Debug.Log("OnConnectedToPhoton succcess");

			stateHUD.SetStateHUD(NetworkState.SERVERCONNECTED);
			base.OnConnectedToPhoton ();
		}
		public override void OnFailedToConnectToPhoton (DisconnectCause cause)
		{
			if(useDebugLog)Debug.Log("OnFailedToConnectToPhoton retry in seconds");

			stateHUD.SetStateHUD(NetworkState.DISCONNECTED);
			stateHUD.SetAnime(Application.systemLanguage == SystemLanguage.Japanese? "接続中" :"Reconneting");
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

			stateHUD.SetAnime(Application.systemLanguage == SystemLanguage.Japanese? "接続中" :"Reconneting");
			Invoke("ConnectToPUN",2.0f);

			base.OnDisconnectedFromPhoton();
		}

		public override void OnPhotonMaxCccuReached ()
		{
			info.Log(Application.systemLanguage == SystemLanguage.Japanese? "サーバ混雑のため接続できませんでした" :"server is too busy now");
			
			base.OnPhotonMaxCccuReached ();
		}




		public void JoinLobby(){
			if(useDebugLog)Debug.Log("JoinLobby");
			stateHUD.SetAnime(Application.systemLanguage == SystemLanguage.Japanese? "ロビー接続中" :"Entering Lobby");
			PhotonNetwork.JoinLobby ();
		}


		public override void OnJoinedLobby () {
			if(useDebugLog)Debug.Log("Joined Lobby succcess");
			stateHUD.SetStateHUD(NetworkState.LOBBYCONNECTED);

			//ロビーからの再入場は無し
			if(DataManager.Instance.gameData.isConnectingRoom){
				DataManager.Instance.gameData.isConnectingRoom=true;
				DataManager.Instance.SaveAll();
				info.Log(Application.systemLanguage == SystemLanguage.Japanese? "前回のゲームは終了しています。" :"Previous game has finished already");
			}
		}

		public YesNoPU yesnoPU;
		public GameTicketSetter gameTicketSetter;
		public void OnClickPlayBtn(){

			if(stateHUD.networkState!=NetworkState.LOBBYCONNECTED || inRoom){
				if(useDebugLog)Debug.LogWarning("ロビーにコネクトされていないので不可");
				return;
			}

			if(DataManager.Instance.gameData.gameTickets<=0){
				info.Log(Application.systemLanguage == SystemLanguage.Japanese? "プレイチケットがありません" :"Your game tickets is empty");

				return;
			}

			float keikaSec=TimeManager.Instance.GetKeikaTimeFromStart();

			if(DataManager.Instance.gameData.gameTickets!=-100 && keikaSec<5*3600f){
				Debug.LogWarning("これもある程度のプレイ時間で終了");
				yesnoPU.Show(Localization.Get("PUStartGame"),OnResponce);
			}else{
				OnResponce(true);
			}
		

		}

		public void OnResponce(bool isYes){
			if(isYes){

				GameObject go=GameObject.FindGameObjectWithTag("TicketSetter");
				if(go){
					GameTicketSetter setter=go.GetComponent<GameTicketSetter>();
					if(setter){
						setter.MinusTicketsNoSave();
					}
				}

				AudioController.Play("Enter");
				HUDOnROOM(true);
				//RandomJoinし、ダメなら部屋をつくる

				if(rooms.Count<=0){
					//部屋がないので、作る
					CreateRoom(PhotonNetwork.player.NickName);
				}else{
					PhotonNetwork.JoinRandomRoom();
				}
			}else{
				
			}
		}


		//自分が部屋を作った場合
		public void CreateRoom(string roomName){
			RoomOptions options = new RoomOptions();
			options.MaxPlayers = maxPlayers;
			options.PlayerTtl = playerTTL;
			options.EmptyRoomTtl=roomTTL;
			//ここでマップをランダムに設定する
			int mapNum=0;
			options.CustomRoomProperties=new ExitGames.Client.Photon.Hashtable() { { "map",mapNum},{ "state",0} };

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
			DataManager.Instance.gameData.lastRoomName=PhotonNetwork.room.Name;
			DataManager.Instance.SaveAll();
		
		}
		// If master client, for every newly connected player, sets the custom properties for him
		// car = 0, position = last (size of player list)
		public override void OnPhotonPlayerConnected (PhotonPlayer newPlayer) {
			if(useDebugLog)Debug.Log("OnPhotonPlayerConnected "+(string)newPlayer.CustomProperties["userName"]);
			/*if (PhotonNetwork.isMasterClient) {
				SetCustomProperties (newPlayer, 0,"JP", PhotonNetwork.playerList.Length - 1);
				//photonView.RPC("UpdateTrack", PhotonTargets.All, trackIndex);
			}*/



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
					SetCustomProperties(p, (int) p.CustomProperties["shipBase"],(int) p.CustomProperties["shipColor"],(string)p.CustomProperties["countly"], playerIndex++,(string)p.CustomProperties["userName"]);
				}
			}

			info.Log(Application.systemLanguage == SystemLanguage.Japanese? (string)disconnetedPlayer.CustomProperties["userName"]+"が撤退しました" :(string)disconnetedPlayer.CustomProperties["userName"]+" leaves this battle");

		}

		// sets and syncs custom properties on a network player (including masterClient)
		private void SetCustomProperties(PhotonPlayer player, int ship,int color,string countly, int position,string name) {
			if(useDebugLog)Debug.Log("SetCustomProperties "+name+" ship" + ship+" "+countly+" poisition "+position);
			ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable() { { "spawn", position },{ "countly", countly }, {"shipBase", ship}, {"shipColor", color},{"userName", name} };
			player.SetCustomProperties(customProperties);
		}

		public override void OnPhotonPlayerPropertiesChanged (object[] playerAndUpdatedProps) {
			if(useDebugLog)Debug.Log("OnPhotonPlayerPropertiesChanged ");
			UpdatePlayerList ();
		}
			


		// self-explainable
		public void UpdatePlayerList() {
			if(useDebugLog)Debug.Log("UpdatePlayerList "+PhotonNetwork.playerList.Length);
			stateHUD.SetLabel(PhotonNetwork.playerList.Length.ToString()+"/"+maxPlayers.ToString(),false);
			lobbyList.ClearList();
			int i=0;
			foreach (PhotonPlayer p in PhotonNetwork.playerList) {


				if(!lobbyList.isUserContains(p.ID))info.Log(Application.systemLanguage == SystemLanguage.Japanese? (string)p.CustomProperties["userName"]+"が参戦決定しました" :(string)p.CustomProperties["userName"]+" joins this battle");

				// enable car choose option for local player
				//if (p == PhotonNetwork.player) {
					//自分です
				//}else{
					


					if (p.CustomProperties.ContainsKey("userName") && p.CustomProperties.ContainsKey("countly")) {
						if(useDebugLog)Debug.Log("Add Lists ");
						lobbyList.AddList((string)p.CustomProperties["userName"],i,(string) p.CustomProperties["countly"],p.ID);

					}else{
						lobbyList.AddList("",i,"UN",p.ID);
					}

					i++;
				//}
			}
			if(isMasterClient){
				if(PhotonNetwork.playerList.Length==maxPlayers){
					if(useDebugLog)Debug.Log("最大人数に達したため、プレイ開始します");

					info.Log(Application.systemLanguage == SystemLanguage.Japanese? "最大人数でバトル開始します" :"battle starts with max users");


					isMasterClient=false;
					PhotonNetwork.room.IsOpen = false;
					Invoke("CallStartGame",2.0f);
					return;
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
			DataManager.Instance.gameData.lastRoomName=PhotonNetwork.room.Name;
			DataManager.Instance.SaveAll();


			if((int)PhotonNetwork.room.CustomProperties["state"]==-1){
				//すでにゲームが終わっている
				LeaveRoom();
				info.Log("すでにゲームは終了しています");
			}else if((int)PhotonNetwork.room.CustomProperties["state"]==1){
				//すでに始まっている
				LeaveRoom();
				info.Log("すでにゲームは終了しています");
			}


			SetCustomProperties(PhotonNetwork.player, shipsSelecter.currentSelect,shipsSelecter.colorLists.mCurrentSelected,DataManager.Instance.gameData.country, PhotonNetwork.playerList.Length - 1,DataManager.Instance.gameData.username);
			base.OnJoinedRoom ();
		}


		public override void OnPhotonRandomJoinFailed (object[] codeAndMsg)
		{
			if(useDebugLog)Debug.Log("OnJoindRoom failled");

			HUDOnROOM(false);

			base.OnPhotonRandomJoinFailed (codeAndMsg);
		}

		bool inRoom=false;
		void HUDOnROOM(bool isInRoom){

			if(isInRoom){
				inRoom=true;
				_timerFlag=0.0f;
				isMasterClient=false;
				lobbyList.ClearList();
				DataManager.Instance.gameData.isConnectingRoom=true;

				timer.SetTime(checkTimeOnRoom,checkTimeOnRoom);
			}else{
				inRoom=false;
				lobbyList.ClearList();
				DataManager.Instance.gameData.isConnectingRoom=false;
			}
		}



		public void OnShipChanged(int num,int color){
			if(stateHUD.networkState==NetworkState.ROOMCONNECTED){
				if(PhotonNetwork.player.CustomProperties.ContainsKey("spawn")){
					SetCustomProperties(PhotonNetwork.player, num,color,(string)PhotonNetwork.player.CustomProperties["countly"], (int)PhotonNetwork.player.CustomProperties["spawn"],(string)PhotonNetwork.player.CustomProperties["userName"]);
				}
			}

		}

		public void OnUserNameChanged(string name){
			if(stateHUD.networkState==NetworkState.ROOMCONNECTED){
				if(PhotonNetwork.player.CustomProperties.ContainsKey("spawn")){
					SetCustomProperties(PhotonNetwork.player, (int)PhotonNetwork.player.CustomProperties["shipBase"],(int)PhotonNetwork.player.CustomProperties["shipColor"],(string)PhotonNetwork.player.CustomProperties["countly"], (int)PhotonNetwork.player.CustomProperties["spawn"],name);
				}
			}

		}

		public void OnCountlyChanged(string countly){
			if(stateHUD.networkState==NetworkState.ROOMCONNECTED){
				if(PhotonNetwork.player.CustomProperties.ContainsKey("spawn")){
					Debug.Log("OnCountlyChanged "+countly);
					SetCustomProperties(PhotonNetwork.player, (int)PhotonNetwork.player.CustomProperties["shipBase"],(int)PhotonNetwork.player.CustomProperties["shipColor"],countly, (int)PhotonNetwork.player.CustomProperties["spawn"],(string)PhotonNetwork.player.CustomProperties["userName"]);
				}
			}

		}


		float _timerFlag=0.0f;
		float _timerFrequency=0.0f;

		void Update(){
			
			if(!timer.gameObject.activeSelf)return;


			if(!isMasterClient)return;



			_timerFlag+=Time.deltaTime;
			_timerFrequency+=Time.deltaTime;

			if(_timerFlag>checkTimeOnRoom){
				_timerFlag=0.0f;
				if(PhotonNetwork.playerList.Length>=minPlayers){
					if(useDebugLog)Debug.Log("最低人数に達したため、プレイ開始します");

					info.Log(Application.systemLanguage == SystemLanguage.Japanese? "最小人数でバトル開始します" :"battle starts with min users");


					isMasterClient=false;
					PhotonNetwork.room.IsOpen = false;
					Invoke("CallStartGame",2.0f);
					return;
				}
			}

			if(_timerFrequency>0.3f){
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

		public int numberOfMaps=3;
		// masterClient only. Calls an RPC to start the race on all clients. Called from GUI
		public void CallStartGame() {

			if(DataManager.Instance.gameData.gameTickets!=-100)AdManager.Instance.HideBanner();
			//ここでマップをランダムに設定する
			int mapNum=Random.Range(0,numberOfMaps);
			PhotonNetwork.room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "map",mapNum}, { "state",1}  });
			photonView.RPC("LoadGame", PhotonTargets.AllBufferedViaServer);
		}
		public string GameSceneName;

		[PunRPC]
		public void LoadGame () {
			AdManager.Instance.HideBanner();

			GameSceneName=GameSceneName;

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




		public void OnClickLeaveRoom(){
			Debug.Log("OnClickLeaveRoom");
			sceneFader.FadeOut(LeaveRoom,true);

		}


		//ロビーは出ずに戻る
		public void LeaveRoom () {
			DataManager.Instance.gameData.isConnectingRoom=false;
			DataManager.Instance.SaveAll();
			PhotonNetwork.LeaveRoom ();
			PhotonNetwork.LoadLevel ("LobbyScene");
		}

		public void OnChangedServer(){
			if(stateHUD.networkState==NetworkState.ROOMCONNECTED){
				LeaveRoom();
			}else{
				PhotonNetwork.Disconnect();
			}
		}

	}
}
