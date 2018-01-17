using UnityEngine;
using System.Collections;
using PathologicalGames;

public class shipControl : MonoBehaviour {
	
	public struct PlayerData {
		public string userName;
		public string countlyCode;
		public int playerID;
		public PlayerData(string name,string countly,int id){
			userName=name;
			countlyCode=countly;
			playerID=id;
		}
	}

	public PlayerData playerData;
	public PhotonView photonView;
	public void InitPlayerData(string name,string countly,int id){
		if(!photonView)photonView= GetComponent<PhotonView>();

		object[] args = new object[]{
			name,
			countly,
			id
		};
		photonView.RPC ("RPCInitPlayerData", PhotonTargets.AllBufferedViaServer,args);
	}
	[PunRPC]
	public void RPCInitPlayerData(string name,string countly,int id) {
		//みんなが1回ずつ呼ぶので、ロード待ち受けに使える
		playerData=new PlayerData(name,countly,id);
	}

	//[HideInInspector]
	public float MaxHP=1500.0f;
	//[HideInInspector]
	public float currentHP=1500.0f;

	public bool isDead=false;
	public bool isOwnersShip(){
		return GUIManager.Instance.shipControll==this?true:false;
	}
	public bool isControllable=false;

	Vector3 newRotation = new Vector3(0,0,0);


	void Start(){
		rd = GetComponent<Rigidbody> (); 
		razerLine=GetComponent<LineRenderer>();
		photonView= GetComponent<PhotonView>();
		photonTransformView = GetComponent<PhotonTransformView>();
		gameObject.tag="Player";
		isPressed=false;


		//パラメータを参照してセットする
		this.MaxHP=PSParams.GameParameters.MaxHP;
		this.shotDulation=PSParams.GameParameters.shotDulation;
		this.shotOffset=PSParams.GameParameters.shotOffset;
		this.shotOffsetX=PSParams.GameParameters.shotOffsetX;
		this.maxSpeed=PSParams.GameParameters.maxSpeed;
		this.speed=PSParams.GameParameters.speed;


		currentHP=MaxHP;
		if(isOwnersShip() && GUIManager.Instance.isDebugMode){
			GUIManager.Instance.hpSlider.SetDebugVal(currentHP.ToString()+"/"+MaxHP);
		}

		if(!photonTransformView){
			GUIManager.Instance.SetShipControll(this);
			isControllable=true;
		}
		isDead=false;

		isShooting=true;
		if(isOwnersShip())StartCoroutine(Shot());
	}
		

	void OnDead(shipControl killedBy){
		//ここで判定
		if(usingLog)Debug.Log("OnDead  killed by"+killedBy.playerData.userName);

		if(!isOwnersShip())return;

			isDead=true;
			isControllable=false;
			
			if(photonView){
				photonView.RPC ("Dead", PhotonTargets.AllViaServer,new object[]{GUIManager.Instance.GetSubweaponInHolder(),killedBy.playerData.playerID});
			}else{
				Dead(GUIManager.Instance.GetSubweaponInHolder(),killedBy.playerData.playerID);
			}

	}

	[PunRPC]
	public void Dead(int[] subweaponInHolder,int killer){

		if(usingLog)Debug.Log("[RPC]Dead hold killer "+killer);

		if(subweaponInHolder!=null){
			//ウェポンをばらまく
			ScatterWeapons(subweaponInHolder);
		}
		ParticleManager.Instance.ShowExplosionBigAt(transform.position,Quaternion.identity,this.transform);

		isDead=true;
		isControllable=false;
		gameObject.SetActive(false);

		if(photonView){
			if(PSPhoton.GameManager.instance){
				PSPhoton.GameManager.instance.OnPlayerDead(this,killer);
			}

		}

	}

	void ScatterWeapons(int[] subweaponInHolder){
		
		Debug.LogWarning("ここでサブウェポンをばらまく");

		for(int i=0;i<subweaponInHolder.Length;i++){
			
		}

	}

	//[HideInInspector]
	public float shotDulation=0.2f;
	//[HideInInspector]
	public float shotOffset=0.2f;
	//[HideInInspector]
	public float shotOffsetX=0.1f;

	public int shot_rensou=1;
	public DanmakuColor shotCol;
	bool stopShot=false;
	bool isShooting=false;



	IEnumerator Shot(){
		while(true && !isDead){
			if(currentUsing==Subweapon.YUDOU){
				if(weaponNum%2==0){
					if(photonView){
						photonView.RPC ("RPC_SpawnSubweapon_Yudoudan", PhotonTargets.AllViaServer,new object[]{
							this.transform.position+ transform.right *0.1f,this.transform.rotation.eulerAngles,PSGameUtils.GameUtils.ConvertToFloat((float)PhotonNetwork.time)});
					}else{
						RPC_SpawnSubweapon_Yudoudan(this.transform.position+ transform.right *0.1f,this.transform.rotation.eulerAngles,Time.realtimeSinceStartup);
					}
				}else{
					if(photonView){
						photonView.RPC ("RPC_SpawnSubweapon_Yudoudan", PhotonTargets.AllViaServer,new object[]{
							this.transform.position- transform.right *0.1f,this.transform.rotation.eulerAngles,PSGameUtils.GameUtils.ConvertToFloat((float)PhotonNetwork.time)});
					}else{
						RPC_SpawnSubweapon_Yudoudan(this.transform.position- transform.right *0.1f,this.transform.rotation.eulerAngles,Time.realtimeSinceStartup);
					}
				}

				weaponNum--;
				if(weaponNum<=0){
					OnWeaponTimerOver();
				}
			}

			if(currentUsing==Subweapon.WAVE){
				if(photonView){
					photonView.RPC ("RPC_SpawnSubweapon_Wave", PhotonTargets.AllViaServer,new object[]{
						this.transform.position+ transform.forward *shotOffset,this.transform.rotation.eulerAngles,PSGameUtils.GameUtils.ConvertToFloat((float)PhotonNetwork.time)});
				}else{
					RPC_SpawnSubweapon_Wave(this.transform.position+ transform.forward *shotOffset,this.transform.rotation.eulerAngles,Time.realtimeSinceStartup);
				}

			}

			if(currentUsing==Subweapon.ZENHOUKOU){
				Vector3[] pos=new Vector3[8];
				Vector3[] rot=new Vector3[8];
					pos[0]=this.transform.position+ transform.forward *shotOffset;
					rot[0]=this.transform.rotation.eulerAngles;
					pos[1]=this.transform.position+ -transform.forward *shotOffset;
					rot[1]=Quaternion.LookRotation(-transform.forward,transform.up).eulerAngles;
					pos[2]=this.transform.position+ (transform.right *shotOffsetX);
					rot[2]=Quaternion.LookRotation(transform.right,transform.up).eulerAngles;
					pos[3]=this.transform.position+ (-transform.right *shotOffsetX);
					rot[3]=Quaternion.LookRotation(-transform.right,transform.up).eulerAngles;
					pos[4]=this.transform.position+ (transform.forward *shotOffset)+ (-transform.right *shotOffsetX);
					rot[4]=Quaternion.LookRotation(-transform.right+transform.forward,transform.up).eulerAngles;
					pos[5]=this.transform.position+ (transform.forward *shotOffset)+ (transform.right *shotOffsetX);
					rot[5]=Quaternion.LookRotation(transform.right+transform.forward,transform.up).eulerAngles;
					pos[6]=this.transform.position+ (-transform.forward *shotOffset)+ (transform.right *shotOffsetX);
					rot[6]=Quaternion.LookRotation(transform.right-transform.forward,transform.up).eulerAngles;
					pos[7]=this.transform.position+ (-transform.forward *shotOffset)+ (-transform.right *shotOffsetX);
					rot[7]=Quaternion.LookRotation(-transform.right-transform.forward,transform.up).eulerAngles;

				if(photonView){
					photonView.RPC ("RPC_SpawnSubweapon_Zenhoukou", PhotonTargets.AllViaServer,new object[]{
						pos,rot,PSGameUtils.GameUtils.ConvertToFloat((float)PhotonNetwork.time)});
				}else{
					RPC_SpawnSubweapon_Zenhoukou(pos,rot,Time.realtimeSinceStartup);
				}

			
			}

			if(stopShot || !isShooting){
				//通常弾の処理を停止
			}else{
				switch(shot_rensou){
					case 1:
						if(photonView){
							photonView.RPC ("RPC_SpawnShot", PhotonTargets.AllViaServer,new object[]{
							this.transform.position+ transform.forward *shotOffset,this.transform.rotation.eulerAngles,PSGameUtils.GameUtils.ConvertToFloat((float)PhotonNetwork.time)});
						}else{
							RPC_SpawnShot(this.transform.position+ transform.forward *shotOffset,this.transform.rotation.eulerAngles,Time.realtimeSinceStartup);
						}
						
						break;
					case 2:
						if(photonView){
							photonView.RPC ("RPC_SpawnShotDouble", PhotonTargets.AllViaServer,new object[]{
								this.transform.position+ transform.forward *shotOffset,this.transform.rotation.eulerAngles,PSGameUtils.GameUtils.ConvertToFloat((float)PhotonNetwork.time)});
						}else{
							RPC_SpawnShotDouble(this.transform.position+ transform.forward *shotOffset,this.transform.rotation.eulerAngles,Time.realtimeSinceStartup);
						}
						break;
					case 3:
						if(photonView){
							photonView.RPC ("RPC_SpawnShotTripple", PhotonTargets.AllViaServer,new object[]{
								this.transform.position+ transform.forward *shotOffset,this.transform.rotation.eulerAngles,PSGameUtils.GameUtils.ConvertToFloat((float)PhotonNetwork.time)});
						}else{
							RPC_SpawnShotTripple(this.transform.position+ transform.forward *shotOffset,this.transform.rotation.eulerAngles,Time.realtimeSinceStartup);
						}
						break;
				}
			}

			yield return new WaitForSeconds(shotDulation);
		}

	}




	//ナパーム弾
	public void ShotNapam(){
		if(photonView){
			photonView.RPC ("RPC_Subweapon_Napam", PhotonTargets.AllViaServer,new object[]{
				this.transform.position+ transform.forward *shotOffset,this.transform.rotation.eulerAngles,PSGameUtils.GameUtils.ConvertToFloat((float)PhotonNetwork.time)});
		}else{
			RPC_Subweapon_Napam(this.transform.position+ transform.forward *shotOffset,this.transform.rotation.eulerAngles,Time.realtimeSinceStartup);
		}
	}

	//ヌーク弾
	public void ShotNuke(){

		if(photonView){
			photonView.RPC ("RPC_Subweapon_Nuke", PhotonTargets.AllViaServer,new object[]{
				this.transform.position+ transform.forward *shotOffset,this.transform.rotation.eulerAngles,PSGameUtils.GameUtils.ConvertToFloat((float)PhotonNetwork.time)});
		}else{
			RPC_Subweapon_Nuke(this.transform.position+ transform.forward *shotOffset,this.transform.rotation.eulerAngles,Time.realtimeSinceStartup);
		}


	}


	#region RPCCall for shots and subweapons
		[PunRPC]
		public void RPC_Subweapon_Napam(Vector3 position,Vector3 rotation,float spawnTime){
			PickupAndWeaponManager.Instance.SpawnSubweapon_Napam(this,position,Quaternion.Euler(rotation),spawnTime);
		}
		[PunRPC]
		public void RPC_Subweapon_Nuke(Vector3 position,Vector3 rotation,float spawnTime){
			PickupAndWeaponManager.Instance.SpawnSubweapon_Nuke(this,position,Quaternion.Euler(rotation),spawnTime);
		}
		[PunRPC]
		public void RPC_SpawnShot(Vector3 position,Vector3 rotation,float spawnTime){
			PickupAndWeaponManager.Instance.SpawnShot(this,shotCol,position,Quaternion.Euler(rotation),spawnTime);
		}

		[PunRPC]
		public void RPC_SpawnShotDouble(Vector3 position,Vector3 rotation,float spawnTime){
			PickupAndWeaponManager.Instance.SpawnShot(this,shotCol,position+ (transform.right *shotOffsetX),Quaternion.Euler(rotation),spawnTime);
			PickupAndWeaponManager.Instance.SpawnShot(this,shotCol,position+ (-transform.right *shotOffsetX),Quaternion.Euler(rotation),spawnTime);
		}
		[PunRPC]
		public void RPC_SpawnShotTripple(Vector3 position,Vector3 rotation,float spawnTime){
			PickupAndWeaponManager.Instance.SpawnShot(this,shotCol,position+ (transform.right *shotOffsetX),Quaternion.Euler(rotation),spawnTime);
			PickupAndWeaponManager.Instance.SpawnShot(this,shotCol,position+ (-transform.right *shotOffsetX),Quaternion.Euler(rotation),spawnTime);
			PickupAndWeaponManager.Instance.SpawnShot(this,shotCol,position,Quaternion.Euler(rotation),spawnTime);
		}
		[PunRPC]
		public void RPC_SpawnSubweapon_Yudoudan(Vector3 position,Vector3 rotation,float spawnTime){
			PickupAndWeaponManager.Instance.SpawnSubweapon_Yudoudan(this,position,Quaternion.Euler(rotation),spawnTime);
		}
		[PunRPC]
		public void RPC_SpawnSubweapon_Wave(Vector3 position,Vector3 rotation,float spawnTime){
			PickupAndWeaponManager.Instance.SpawnSubweapon_Wave(this,position,Quaternion.Euler(rotation),spawnTime);
		}
		[PunRPC]
		public void RPC_SpawnSubweapon_Zenhoukou(Vector3[] position,Vector3[] rotation,float spawnTime){
			if(position.Length!=rotation.Length){
				Debug.LogError("全方向の配列の個数が違う");
			}else{
				for(int i=0;i<position.Length;i++){
					PickupAndWeaponManager.Instance.SpawnSubweapon_Wave(this,position[i],Quaternion.Euler(rotation[i]),spawnTime);
				}
			}
		}

	#endregion




	public bool usingLog=true;

	public bool OnShotToggle(bool val){
		
		if(isDead || isShooting==val)return false;

		if(usingLog)Debug.Log("OnShotToggle "+val);

		if(val){
			if(currentUsing!=Subweapon.NONE && currentUsing!=Subweapon.STEALTH)return false;

			if(currentUsing==Subweapon.STEALTH)OnWeaponTimerOver();

			/*if(photonView){
				photonView.RPC ("StopShot", PhotonTargets.AllViaServer,new object[]{true});
			}else{*/
				StartShot(true);
			//}
		}else{
			/*if(photonView){
				photonView.RPC ("StopShot", PhotonTargets.AllViaServer,new object[]{false});
			}else{*/
				StartShot(false);
			//}
		}

		return true;
	}


	//[PunRPC]
	public void StartShot(bool isShot){
		//if(usingLog)Debug.Log("[RPC]StartShot  "+isShot);
		isShooting=isShot;
	}



	public StealthEffecter stealthEffecter;
	public GameObject engine;

	float weapontimer=0.0f;
	int weaponNum=0;

	[HideInInspector]
	public Subweapon currentUsing=Subweapon.NONE;

	void OnWeaponTimerOver(){
		if(usingLog)Debug.Log("サブウェポンの有効時間終了");

		if(currentUsing==Subweapon.STEALTH){
			if(photonView){
				photonView.RPC ("StealthMode", PhotonTargets.AllViaServer,new object[]{false});
			}else{
				StealthMode(false);
			}
			StealthMode(false);
		}else if(currentUsing==Subweapon.RAZER){
			if(photonView){
				photonView.RPC ("RazerMode", PhotonTargets.AllViaServer,new object[]{false});
			}else{
				RazerMode(false);
			}
		}

		currentUsing=Subweapon.NONE;
		stopShot=false;

		if(isOwnersShip())GUIManager.Instance.EnableSubweapon();
	}

	public void OnUseSubWeapon(Subweapon weaponType){
		if(isDead || currentUsing!=Subweapon.NONE)return;

		if(usingLog)Debug.Log("サブウェポンの使用 "+weaponType.ToString());

		if(isOwnersShip()){
			//if(photonView){
				//photonView.RPC ("UseSubWeapon", PhotonTargets.AllViaServer,new object[]{(int)weaponType});
			//}else{
				UseSubWeapon((int)weaponType);
			//}
		}
	}

	//[PunRPC]
	public void UseSubWeapon(int weaponType){

		//if(usingLog)Debug.Log("[RPC]UseSubWeapon "+((Subweapon)weaponType).ToString());

		currentUsing=(Subweapon)weaponType;

		weaponNum=0;
		stopShot=false;
		switch(currentUsing){
		case Subweapon.WAVE:

			weapontimer=3.0f;
			stopShot=true;
			break;
		case Subweapon.ZENHOUKOU:
			weapontimer=3.0f;
			stopShot=true;
			break;
		case Subweapon.NAPAM:
			weapontimer=3.0f;
			stopShot=true;
			ShotNapam();
			break;
		case Subweapon.NUKE:
			weapontimer=5.0f;
			stopShot=true;
			ShotNuke();
			break;
		case Subweapon.YUDOU:
			weapontimer=10.0f;
			stopShot=true;
			//発射個数
			weaponNum=3;
			break;
		case Subweapon.STEALTH:
			GUIManager.Instance.SetShotTgl(false);
			OnShotToggle(false);
			if(photonView){
				photonView.RPC ("StealthMode", PhotonTargets.AllViaServer,new object[]{true});
			}else{
				StealthMode(true);
			}

			break;
		case Subweapon.RAZER:
			
			weapontimer=10.0f;
			stopShot=false;
			if(photonView){
				photonView.RPC ("RazerMode", PhotonTargets.AllViaServer,new object[]{true});
			}else{
				RazerMode(true);
			}
			break;
		}

	}

	[HideInInspector]
	public bool isStealthMode=false;
	[PunRPC]
	public void StealthMode(bool isOn){
		isStealthMode=isOn;
		if(isOwnersShip()){
			//プレイヤのシップにはステルスマテリアル
			if(isOn){
				engine.SetActive(false);
				stealthEffecter.StealthMode(true);
			}else{
				engine.SetActive(true);
				stealthEffecter.StealthMode(false);
			}
		}else{
			//相手のシップには消す
			if(isOn){
				engine.SetActive(false);
				stealthEffecter.gameObject.SetActive(false);
			}else{
				engine.SetActive(true);
				stealthEffecter.gameObject.SetActive(true);
			}
		}

	}

	LineRenderer razerLine;
	[PunRPC]
	public void RazerMode(bool isOn){
		if(isOn){
			razerLine.enabled=true;
		}else{
			razerLine.enabled=false;
		}
	}

	public float razerMaxdistance=5.0f;
	void SetRazerTarget(Vector3 pos){
		razerLine.SetPosition(1,pos);
	}



	//通常弾　サブウェポンからのダメージを受けた時
	public void OnHit(Subweapon type,float damage,Vector3 hitPosition,shipControl launcher){
		if(isDead)return;

		if(usingLog)Debug.Log(playerData.userName+" にダメージ "+damage);


		//実際は、音とエフェクトを出すだけ
		switch(type){
			case Subweapon.NONE:
				//通常弾
				//AudioController.Play ("Explosion");
				ParticleManager.Instance.ShowExplosionSmallAt(new Vector3(hitPosition.x,hitPosition.y+0.5f,hitPosition.z),Quaternion.identity,null);
				break;
		}

		if(isOwnersShip()){
			if(currentHP-damage<=0.0f){
				GUIManager.Instance.Damage (damage, MaxHP);
				OnDead(launcher);
				return;
			}
			if(photonView){
				photonView.RPC ("TakeDamage", PhotonTargets.AllViaServer,new object[]{damage});
			}else{
				TakeDamage(damage);
			}
		}




	}

	[PunRPC]
	public void TakeDamage(float damage){
		if(usingLog)Debug.Log("[RPC] TakeDamage"+playerData.userName+" にダメージ "+damage);

		currentHP-=damage;
		//これは、プレイヤーオブジェクトのみでやる
		if(isOwnersShip()){
			if(GUIManager.Instance.isDebugMode){
				GUIManager.Instance.hpSlider.SetDebugVal(currentHP.ToString()+"/"+MaxHP);
			}
			GUIManager.Instance.Damage (damage, MaxHP);
		}
	}

		


	//回復を拾った時
	public void OnPickUp_Cure(Pickup pu){
		if(isDead)return;

		if(usingLog)Debug.Log(playerData.userName+" が回復をひろいました ");

		float cureVal=0.0f;
		switch(pu){
			case Pickup.CureS:
				AudioController.Play ("Powerup");
				ParticleManager.Instance.ShowCureSAt(transform.position,Quaternion.identity,transform);
				cureVal=PSParams.GameParameters.curePersentageS;
				break;
			case Pickup.CureM:
				AudioController.Play ("Powerup");
				ParticleManager.Instance.ShowCureSAt(transform.position,Quaternion.identity,transform);
				cureVal=PSParams.GameParameters.curePersentageM;
				break;
			case Pickup.CureL:
				AudioController.Play ("Powerup");
				ParticleManager.Instance.ShowCureSAt(transform.position,Quaternion.identity,transform);
				cureVal=PSParams.GameParameters.curePersentageL;
				break;
		}

		if(photonView){
			photonView.RPC ("CureSelf", PhotonTargets.All,new object[]{cureVal});
		}else{
			CureSelf(cureVal);
		}

	}

	[PunRPC]
	public void CureSelf(float percentage){

		if(usingLog)Debug.Log("[RPC] CureSelf "+percentage+"%");

		currentHP+=MaxHP*(percentage/100.0f);
		if(currentHP>MaxHP)currentHP=MaxHP;

		if(isOwnersShip() && GUIManager.Instance.isDebugMode){
			GUIManager.Instance.hpSlider.SetDebugVal(currentHP.ToString()+"/"+MaxHP);
		}

		if(isOwnersShip())GUIManager.Instance.Cure (percentage,MaxHP);
	}


	//サブウェポンを拾った時
	public void OnPickUp_Subweapon(Pickup pu){
		if(isDead)return;

		if(usingLog)Debug.Log(playerData.userName+" がサブウェポンをひろいました ");

		//PickUpのエフェクトはここで出す

		if(isOwnersShip()){
			int num=(int)pu;
			num=num-3;
			GUIManager.Instance.OnGetSubWeapon((Subweapon)num);
		}
	}





	//操作系統
	Vector3 temp;
	bool isPressed=false;
	Vector3 currentTappedPos;

	// 移動スピード
	//[HideInInspector]
	public float maxSpeed = 2.0f;
	//[HideInInspector]
	public float speed = 0.01f;


	Vector3 tr;
	Rigidbody rd;
	Vector3 velocity;
	PhotonTransformView photonTransformView;
	//GUIManagerからの入力受け取りメソッド
	public void OnPressTapLayer(bool isPress,Vector3 worldPos){
		isPressed=isPress;
		currentTappedPos=worldPos;
	}

	public void OnUpdateTapLayer(Vector3 worldPos){
		currentTappedPos=worldPos;
	}

	void Update () {

		if(isDead || !isControllable)return;

		if(isOwnersShip()){
			//PlayerのShipのみで呼ばれる
			if(currentUsing!=Subweapon.NONE && currentUsing!=Subweapon.STEALTH){
				weapontimer-=Time.deltaTime;
				if(weapontimer<0.0f){
					OnWeaponTimerOver();
				}
				if(currentUsing==Subweapon.RAZER){
					shipControl razerTarget=GUIManager.Instance.GetNearestShip(transform.position,razerMaxdistance);
					if(razerTarget){
						SetRazerTarget(razerTarget.gameObject.transform.position);
						//ここでターゲットになったshipにダメージを与える
					}else{
						SetRazerTarget(transform.position+transform.forward*0.5f);
					}

				}
			}


			tr = transform.position.normalized;

			if (isPressed) {
				// タップの方向に向く
				newRotation = Quaternion.LookRotation(currentTappedPos - transform.position).eulerAngles;
				newRotation.x = 0;
				newRotation.z = 0;

				// 回転
				transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (newRotation), Time.deltaTime * 4.0f);

			} else {

				// 徐々に止める
				velocity = new Vector3(rd.velocity.x, 0, rd.velocity.z);
				velocity = velocity - (velocity / 40);
				rd.velocity = velocity;
			}




			if(rd.velocity.magnitude > maxSpeed)
			{
				//Debug.LogWarning("Maxspeedを超えた");
				rd.velocity = rd.velocity.normalized * maxSpeed;
			}

			tr = transform.forward * speed;

			rd.AddForce (tr, ForceMode.VelocityChange);

			if(photonTransformView)photonTransformView.SetSynchronizedValues(speed: rd.velocity, turnSpeed: 0);
		}
	}



}
