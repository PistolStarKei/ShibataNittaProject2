using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// AppDataの説明
/// </summary>
namespace PSParams{
	public static class AppDatas {

		#region  AppIDs
		public static readonly string SHA1="";
		public static readonly string APPID="com.Pistolstarweb.spacekill";
		public static readonly string MailTo="info@pistolstarweb.com";
		#endregion

		#region  Banner And IAP
		public static readonly string[] SKUS=new string[4]{
			"com.Pistolstarweb.spacekill",
			"com.Pistolstarweb.spacekill",
			"com.Pistolstarweb.spacekill",
			"com.Pistolstarweb.spacekill"
		};
		public static string BannerID="";

		public static string TwitterID="";
		public static string TwitterID2="";
		#endregion
	}
}
