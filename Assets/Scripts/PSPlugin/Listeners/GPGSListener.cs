using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class LeaderboadScore{

	public bool[] hasData=new bool[20];
	public string[] names=new string[20];
	public long[] scores=new long[20];
	public bool[] isUserData=new bool[20];
	public Texture2D[] icon;
    public int playerRank=-1;

}
public class GPGSListener:  PS_SingletonBehaviour<GPGSListener> {



    public int howManyRankToLoad=20;
    public string userName_string="";
    public delegate void Callback_scoreUpdatedEvent(LeaderboadScore scores);
    public  event Callback_scoreUpdatedEvent scoreUpdatedEvent;
    public delegate void Callback_saveDataComplete(bool s);
    public  event Callback_saveDataComplete saveDataCompletedEvent;
    public delegate void Callback_loadDataComplete(string s);
    public  event Callback_loadDataComplete loadDataCompletedEvent;


	public void LoadReaderBoadData(string ID,bool isFriend,Callback_scoreUpdatedEvent scoreUpdatedEvent){
		this.scoreUpdatedEvent=null;
		this.scoreUpdatedEvent=scoreUpdatedEvent;

		if(!isLogin() || isLoadingBoadData){
			scoreUpdatedEvent(null);
			return;
		}
        if(isDebugLog)Debug.Log( "リーダーボードのロード "+ID);
        isLoadingBoadData=true;
        StartCoroutine(LoadCurrentRank(ID,isFriend));

    }
    bool isLoadingBoadData=false;
    GPLeaderBoard loadedLeaderBoad;
    bool isLoading=false;
    bool isLoadingSuccessed=false;
    IEnumerator LoadCurrentRank(string ID,bool isFriend){
        isLoading=true;isLoadingSuccessed=false;
        if(isFriend){
            GooglePlayManager.Instance.LoadTopScores(ID, GPBoardTimeSpan.ALL_TIME,GPCollectionType.FRIENDS,howManyRankToLoad);
        }else{
            GooglePlayManager.Instance.LoadTopScores(ID, GPBoardTimeSpan.ALL_TIME,GPCollectionType.GLOBAL,howManyRankToLoad);
        }

         while(isLoading){
              yield return null;
         }
        if(isLoadingSuccessed){
            loadedLeaderBoad = GooglePlayManager.Instance.GetLeaderBoard(ID);
            if(loadedLeaderBoad == null) {
                OnLoadReaderBoadFinished(false,"ロード完了したが、GETできない場合",null);
                yield break;
            }
        }else{
            OnLoadReaderBoadFinished(false,"ロードに失敗した場合",null);
            yield break;
        }

        yield return null;

        List<GPScore> scoresLB;
        if(isFriend){
            scoresLB = loadedLeaderBoad.GetScoresList(GPBoardTimeSpan.ALL_TIME,GPCollectionType.FRIENDS);
        }else{
            scoresLB = loadedLeaderBoad.GetScoresList(GPBoardTimeSpan.ALL_TIME,GPCollectionType.GLOBAL);
        }
        if(scoresLB==null){
            OnLoadReaderBoadFinished(false,"ロード完了　GETできたが　スコアリストがnullの場合",null);
            yield break;
        }
        yield return null;
        OnLoadReaderBoadFinished(true,"ロード正常終了",GetLeaderBoadData(scoresLB));
    }

    //null fals 
    void OnLoadReaderBoadFinished(bool isSuccess,string log,LeaderboadScore scoreDatas){
        Debug.Log("リーダーボードのロード終了"+log);
        isLoadingBoadData=false;
        if(scoreUpdatedEvent!=null)scoreUpdatedEvent(scoreDatas);
    }


    LeaderboadScore GetLeaderBoadData(List<GPScore> scores){
        
        LeaderboadScore data=GetNewScore();
        int num=0;
        foreach(GPScore score in scores) {
            data.names[num]=score.Player.name;
            data.scores[num]=score.LongScore;
            data.hasData[num]=true;
            if(GooglePlayManager.Instance.player.playerId==score.PlayerId){
                data.isUserData[num]=true;
                data.playerRank=score.Rank;
            }
            if(score.Player.hasIconImage){
                data.icon[num]=score.Player.icon;
            }else{
                data.icon[num]=null;
            }
            num++;
        }
        if(isDebugLog){
            string str="";
            for(int i=0;i<howManyRankToLoad;i++){
                str="";
                str+="データ "+i.ToString()+" user? "+data.isUserData[i]+" 名前:"+data.names[i]+" スコア："+data.scores[i]+" アイコン:";
                str+=data.icon[i]==null?"null": "あり";
                Debug.Log(str);
            }
        }
        return data;
    }



    public void Init(){
        if(Application.isEditor)PS_Plugin.Instance.OnGpgInitComplete(true);


        GooglePlayConnection.ActionConnectionResultReceived += OnConnectionResult;
        GooglePlayManager.ActionScoreSubmited += OnScoreSbumitted;
        GooglePlayConnection.ActionPlayerConnected +=  OnPlayerConnected;
        GooglePlayConnection.ActionPlayerDisconnected += OnPlayerDisconnected;
        GooglePlaySavedGamesManager.ActionConflict += ActionConflict;
        GooglePlayManager.ActionScoresListLoaded += ActionScoreRequestReceived;
        GooglePlayConnection.Instance.Connect ();
    }


	public void ClearAllEventListeners(){
		if(scoreUpdatedEvent!=null)scoreUpdatedEvent=null;
		if(saveDataCompletedEvent!=null)saveDataCompletedEvent=null;
	}

    public bool isLogin(){
        if(GooglePlayConnection.State == GPConnectionState.STATE_CONNECTED){
            return true;
        }else{
            return false;
        }

    }
    public bool isDebugLog=false;
    public void SubmitScore(long score,string ID){
        if(!isLogin())return;
        if(isDebugLog)Debug.Log( "Invoke SubmitScore");
        GooglePlayManager.Instance.SubmitScoreById(ID, score);
    }


	public Texture2D getPlayerIcon(){
		if(!isLogin()){
			return null;
		}
		#if UNITY_ANDROID
		if(GooglePlayManager.Instance.player.icon != null) {
			return GooglePlayManager.Instance.player.icon;
		}else{
			return null;
		}
		#endif
		#if  UNITY_IPHONE

		#endif
		return null;
	}
	
	
    public void OpenWithUI(string ID) {
		if(!isLogin())return;
		if(isDebugLog)Debug.Log( "Invoke OpenWithUI");


        GooglePlayManager.Instance.ShowLeaderBoardById(ID);
	}

	public void Open(){
		GooglePlayManager.Instance.ShowLeaderBoardsUI();
	}

    void OnConnectionResult(GooglePlayConnectionResult result) {
        Debug.Log(result.code.ToString());
        if(isDebugLog)Debug.Log( "GooglePlayへのコネクト完了！: " + userName_string );
        PS_Plugin.Instance.OnGpgInitComplete(result.IsSuccess);
    }
    void OnPlayerConnected() {
        userName_string=GooglePlayManager.Instance.player.name;
       
    }
    void OnPlayerDisconnected() {
    }

    private void ActionScoreRequestReceived (GooglePlayResult obj) {
        if(obj.IsSucceeded && !obj.IsFailed){
            isLoadingSuccessed=true;
        }else{
            isLoadingSuccessed=false;
        }
        isLoading=false;
    }
	


	void OnScoreSbumitted (GP_LeaderboardResult result) {
		

	}



    //Save Game
	void ActionGameSaveLoaded (GP_SpanshotLoadResult result) {
		GooglePlaySavedGamesManager.ActionGameSaveLoaded-=ActionGameSaveLoaded;


		Debug.Log("ActionGameSaveLoaded: " + result.Message);
		if(result.IsSucceeded) {

			Debug.Log("Snapshot.Title: " 					+ result.Snapshot.meta.Title);
			Debug.Log("Snapshot.Description: " 				+ result.Snapshot.meta.Description);
			Debug.Log("Snapshot.CoverImageUrl): " 			+ result.Snapshot.meta.CoverImageUrl);
			Debug.Log("Snapshot.LastModifiedTimestamp: " 	+ result.Snapshot.meta.LastModifiedTimestamp);

			Debug.Log("Snapshot.stringData: " 				+ result.Snapshot.stringData);
			Debug.Log("Snapshot.bytes.Length: " 			+ result.Snapshot.bytes.Length);


            if(loadDataCompletedEvent!=null)loadDataCompletedEvent(result.Snapshot.stringData);
		}else{
			
            if(loadDataCompletedEvent!=null)loadDataCompletedEvent("");
		}


	}

	public void LoadSavedData(){
		GooglePlaySavedGamesManager.ActionGameSaveLoaded+=ActionGameSaveLoaded;
		GooglePlaySavedGamesManager.Instance.LoadSpanshotByName("PekaDat");
	}


	public void SaveSnapshot() {
		/*string str=DataManger.Instance.GenerateDataString();
		if(str.Length>4){
			StartCoroutine(MakeScreenshotAndSaveGameData(str));
		}*/

	}

	private IEnumerator MakeScreenshotAndSaveGameData(string data) {


		yield return new WaitForEndOfFrame();
		// Create a texture the size of the screen, RGB24 format
		int width = Screen.width;
		int height = Screen.height;
		Texture2D Screenshot = new Texture2D( width, height, TextureFormat.RGB24, false );
		// Read screen contents into the texture
		Screenshot.ReadPixels( new Rect(0, 0, width, height), 0, 0 );
		Screenshot.Apply();


		long TotalPlayedTime = 20000;
		string currentSaveName =  "PekaDat";
		string description  = "最終更新日: " + System.DateTime.Now.ToString("MM/dd/yyyy H:mm:ss");


		GooglePlaySavedGamesManager.ActionGameSaveResult += ActionGameSaveResult;

		GooglePlaySavedGamesManager.Instance.CreateNewSnapshot(currentSaveName,
			description,
			Screenshot,
			data,
			TotalPlayedTime);		
		Destroy(Screenshot);
	}


	private void ActionGameSaveResult (GP_SpanshotLoadResult result) {
		GooglePlaySavedGamesManager.ActionGameSaveResult -= ActionGameSaveResult;
		Debug.Log("ActionGameSaveResult: " + result.Message);

		if(saveDataCompletedEvent!=null)saveDataCompletedEvent(result.IsSucceeded);

	}	
	private void ActionConflict (GP_SnapshotConflict result) {

		GP_Snapshot snapshot = result.Snapshot;
		GP_Snapshot conflictSnapshot = result.ConflictingSnapshot;

		// Resolve between conflicts by selecting the newest of the conflicting snapshots.
		GP_Snapshot mResolvedSnapshot = snapshot;

		if (snapshot.meta.LastModifiedTimestamp < conflictSnapshot.meta.LastModifiedTimestamp) {
			mResolvedSnapshot = conflictSnapshot;
		}

		result.Resolve(mResolvedSnapshot);
	}





    void OnApplicationQuit()
    {
        GooglePlayConnection.ActionConnectionResultReceived -= OnConnectionResult;
        GooglePlayManager.ActionScoreSubmited -= OnScoreSbumitted;
        GooglePlayConnection.ActionPlayerConnected -=  OnPlayerConnected;
        GooglePlayConnection.ActionPlayerDisconnected -= OnPlayerDisconnected;
        GooglePlaySavedGamesManager.ActionConflict -= ActionConflict;
        GooglePlayManager.ActionScoresListLoaded -= ActionScoreRequestReceived;

    }
    LeaderboadScore GetNewScore(){
        LeaderboadScore data=new  LeaderboadScore();
        data.names=new string[howManyRankToLoad];
        data.scores=new long[howManyRankToLoad];
        data.hasData=new bool[howManyRankToLoad];
        data.isUserData=new bool[howManyRankToLoad];
        data.icon=new Texture2D[howManyRankToLoad];
        data.playerRank=-1;
        for(int i=0;i<howManyRankToLoad;i++){
            data.names[i]="";
            data.scores[i]=-1L;
            data.hasData[i]=false;
            data.isUserData[i]=false;
            data.icon[i]=null;
        }
        return data;
    }
}
