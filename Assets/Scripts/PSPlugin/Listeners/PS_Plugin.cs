using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PS_Plugin: PS_SingletonBehaviour<PS_Plugin> {
	
    public bool needToSingleton=false;
    void DestroyAll(){
        foreach (Transform childTransform in gameObject.transform) Destroy(childTransform.gameObject);
        Destroy(gameObject);
    }
    void Awake()
    {
        if(needToSingleton){
            if(this != Instance)
            {
                DestroyAll();
                return;
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }

	public bool initOnStart=false;
	void Start(){
		if(initOnStart)InitAll();
	}

	public bool isDebugMode=false;

	
	

	public void InitAll(){
        AndroidNotificationManager.Instance.CancelAllLocalNotifications();

        StartCoroutine(InitInvoker());

	}


    IEnumerator InitInvoker(){
        //最悪なくても良いもの
            isInitted_Store=false;
            storeListener.Init();

            while(!isInitted_Store){
                yield return null;
            }
        yield return new WaitForSeconds(0.2f);
            isInitted_Twitter=false;
            twListener.Init();

            //Tapjoyもここでやる
            //if(tjListener!=null)tjListener.Init();
            while(!isInitted_Twitter){
                yield return null;
            }
        yield return new WaitForSeconds(0.2f);
        isInitted_Readerboad=false;
        readerboadListener.Init();

        yield return null;

        while(!isInitted_Readerboad){
            yield return null;
        }
   
    }

	//ストア
    public StoreListener storeListener;
    public bool isInitted_Store=false;
    public bool isConnected_Store=false;
    public void OnStoreInitComplete(bool isSuccess){
        isInitted_Store=true;
        isConnected_Store=isSuccess;
    }

    //Twitter
    public TwitterListener twListener;
    public bool isInitted_Twitter=false;
    public bool isConnected_Twitter=false;
    public void OnTwitterInitComplete(bool isSuccess){
        isInitted_Twitter=true;
        isConnected_Twitter=isSuccess;
    }
		
    //Readerboad
    public GPGSListener readerboadListener;
    public bool isInitted_Readerboad=false;
    public bool isConnected_Readerboad=false;
    public void OnGpgInitComplete(bool isSuccess){
        isInitted_Readerboad=true;
        isConnected_Readerboad=isSuccess;
    }
 

	public void ClearAllCallbacks() {
		if(storeListener!=null)storeListener.ClearAllEventListeners();
        if(readerboadListener!=null)readerboadListener.ClearAllEventListeners();
		if(twListener!=null)twListener.ClearAllEventListeners();
	}



	public void SetNotification(string tittle,string desc,int sec){
		if(AndroidNotificationManager.Instance!=null)AndroidNotificationManager.Instance.ScheduleLocalNotification(tittle,desc,sec);
	}

    //SceneManager
    public string GetCurrentSceneName(){
        return SceneManager.GetActiveScene().name;
    }

    public void LoadScene(string sceneName){
        Debug.Log("LoadScene");
        SceneManager.LoadScene(sceneName);
        //SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
