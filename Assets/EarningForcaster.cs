using UnityEngine;
using System.Collections;

public class EarningForcaster : MonoBehaviour {

	public float ecpm=2.19f;
	public int mau=1000;

	[Tooltip("広告のリフレッシュレート")]
	public int refleshRateInSec=30;

	[Tooltip("月間アクティブユーザーの平均ログイン回数")]
	public int loginNumInMonth=10;

	[Tooltip("アクティブユーザーの１回あたりの平均滞在時間")]
	public int activeTimePerLogin=180;

	[Tooltip("日本円に換算")]
	public int exchangeRate=110;

	public UILabel lb;

	public void ConPute(){
		float earning=0.0f;

		//総プレイ時間を計算する 秒数である
		int timeSum=mau*loginNumInMonth*activeTimePerLogin;
		Debug.Log("総プレイ時間 "+timeSum);

		//広告の総表示回数
		int showNum=timeSum/refleshRateInSec;
		Debug.Log("広告の総表示回数 "+showNum);

		//1回表示あたりの収益
		float earnignPerOne=ecpm/1000.0f;


		earning=showNum*earnignPerOne;

		earning=earning*exchangeRate;
		lb.text=earning.ToString("F0")+"円";
	}
}
