using UnityEngine;
using System.Collections;
using System;

public class TimeManager : PS_SingletonBehaviour<TimeManager>  {


	public bool isTimer=false;

	void Update(){
		if(isTimer){
			DataManager.Instance.gameData.timeForNextTickets-=Time.deltaTime;
			if(DataManager.Instance.gameData.timeForNextTickets<=0.0f){
				OnAddTickets();
				CheckTickets();

			}
		}
	}

	void Start(){
		CheckTickets();
	}

	public void OnAddTickets(){
		//ここでチケットを追加する。
		DataManager.Instance.gameData.gameTickets++;
		DataManager.Instance.gameData.timeForNextTickets=-1.0f;
		DataManager.Instance.SaveAll();
		isTimer=false;
	}

	public void CheckTickets(){
		
		if(DataManager.Instance.gameData.gameTickets<PSParams.GameParameters.DefaultTicketsNum){
			if(DataManager.Instance.gameData.timeForNextTickets<0.0f){
				
				DataManager.Instance.gameData.timeForNextTickets=PSParams.GameParameters.TimeForNextTicket;
				DataManager.Instance.SaveAll();
			}
			//差分を減少させる
			float keika=KeikaSecondSinceLast(StringToDateTime(DataManager.Instance.gameData.lastTime));
			DataManager.Instance.gameData.timeForNextTickets-=keika;

			Debug.Log("前回からの経過時間　"+keika);

			isTimer=true;
		}
	}





    public bool ISSameDayLogin(DateTime lastTimeLogin){
        
		if(IsSameDayLogin(lastTimeLogin))return true;

		return false;
        
    }

	public float KeikaSecondSinceLast(DateTime lastTimeLogin){
		return GetKeikaSecondsSinceLast(lastTimeLogin);
	}

	#region static methods
	public static string GetCurrentTime(){
		return DateTimeToString(DateTime.Now);
	}
	public static string DateTimeToString(DateTime time){
		return time.ToString();
	}
	public static DateTime StringToDateTime(string timeString){

		DateTime date;
		DateTime.TryParse(timeString,out date);
		return date;

	}
	public static int GetKeikaSecondsSinceLast(DateTime lastTimeOpen){

		TimeSpan ts = DateTime.Now - lastTimeOpen;
		return Mathf.FloorToInt((float)ts.TotalSeconds);

	}
	public static bool IsRenzoku(DateTime lastTimeOpen){

		TimeSpan ts = DateTime.Now - lastTimeOpen;
		if(Mathf.FloorToInt((float)ts.TotalSeconds)<=172800){
			if(DateTime.Now.AddDays(-1).Day==lastTimeOpen.Day){
				return true;
			}else{
				return false;
			}
		}else{
			return false;
		}
	}
	public static bool IsSameDayLogin(DateTime lastTimeOpen){
		TimeSpan ts = DateTime.Now - lastTimeOpen;
		if(Mathf.FloorToInt((float)ts.TotalSeconds)<=172800){
			if(DateTime.Now.Day==lastTimeOpen.Day){
				return true;
			}else{
				return false;
			}
		}else{
			return false;
		}
	}
	#endregion

	public bool isPersistantBetweenScenes=false;
	void Awake(){

		if(isPersistantBetweenScenes){

			DontDestroyOnLoad(this.gameObject);

			if(this != Instance)
			{
				DestroyAll();
				return;
			}

		}
	}
	private void DestroyAll(){
		foreach (Transform childTransform in gameObject.transform) Destroy(childTransform.gameObject);
		Destroy(gameObject);
	}
}
