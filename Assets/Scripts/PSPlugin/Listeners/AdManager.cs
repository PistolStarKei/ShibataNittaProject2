using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/// <summary>
/// AdManagerの説明
/// </summary>
public class AdManager : PS_SingletonBehaviour<AdManager>  {

	#region  メンバ変数
	public GADBannerSize size = GADBannerSize.SMART_BANNER;
	public TextAnchor anchor = TextAnchor.LowerCenter;
	private static Dictionary<string, GoogleMobileAdBanner> _refisterdBanners = null;


	#endregion

	#region  初期化

	void Awake () {
	}

	void Start () {
		if(AndroidAdMobController.Instance.IsInited) {
			if(!AndroidAdMobController.Instance.BannersUunitId.Equals(PSParams.AppData.BannerUnitID)) {
				AndroidAdMobController.Instance.SetBannersUnitID(PSParams.AppData.BannerUnitID);
			} 
			if(!AndroidAdMobController.Instance.InterstisialUnitId.Equals(PSParams.AppData.InterstitialUnitID)) {
				AndroidAdMobController.Instance.SetInterstisialsUnitID(PSParams.AppData.InterstitialUnitID);
			}
		} else {
			AndroidAdMobController.Instance.Init(PSParams.AppData.BannerUnitID);

			AndroidAdMobController.Instance.Init(PSParams.AppData.InterstitialUnitID);
		}

	}
	#endregion


	#region  Update
	
	void Update(){
	
	}

	#endregion


	


	#region  Public関数
	public void ShowBanner(){
		GoogleMobileAdBanner banner;
		if(registerdBanners.ContainsKey(sceneBannerId)) {
			banner = registerdBanners[sceneBannerId];
		}  else {
			banner = AndroidAdMobController.Instance.CreateAdBanner(anchor, size);
			registerdBanners.Add(sceneBannerId, banner);
		}

		if(banner.IsLoaded && !banner.IsOnScreen) {
			banner.Show();
		}

	}
	public void HideBanner() {
		if(registerdBanners.ContainsKey(sceneBannerId)) {
			GoogleMobileAdBanner banner = registerdBanners[sceneBannerId];
			if(banner.IsLoaded) {
				if(banner.IsOnScreen) {
					banner.Hide();
				}
			} else {
				banner.ShowOnLoad = false;
			}
		}
	}

	public void ShowInterstitial() {
		AndroidAdMobController.Instance.StartInterstitialAd();
	}
	#endregion
	

	#region  メンバ関数
	public static Dictionary<string, GoogleMobileAdBanner> registerdBanners {
		get {
			if(_refisterdBanners == null) {
				_refisterdBanners = new Dictionary<string, GoogleMobileAdBanner>();
			}

			return _refisterdBanners;
		}
	}
	public string sceneBannerId {
		get {
			#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
			return Application.loadedLevelName + "_" + this.gameObject.name;
			#else
			return SceneManager.GetActiveScene().name + "_" + this.gameObject.name;
			#endif
		}
	}
	#endregion
}
