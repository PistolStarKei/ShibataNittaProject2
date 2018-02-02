using UnityEngine;
using System.Collections;
using System;

public class TimeManager : PS_SingletonBehaviour<TimeManager>  {


    public bool ISSameDayLogin(DateTime lastTimeLogin){
        
		if(IsSameMonthLogin(lastTimeLogin))return true;

		return false;
        
    }

	public float KeikaSecondSinceLast(DateTime lastTimeLogin){
		return GetKeikaSecondsSinceLast(lastTimeLogin);
	}


	public float GetKeikaTimeFromStart(){
		return KeikaSecondSinceLast(StringToDateTime(DataManager.Instance.envData.startTime));


	}

	public bool ISSameMonthLogin(DateTime lastTimeLogin){

		if(IsSameDayLogin(lastTimeLogin))return true;

		return false;

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
	public static bool IsSameMonthLogin(DateTime lastTimeOpen){
		
			if(DateTime.Now.Month==lastTimeOpen.Month){
				return true;
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
