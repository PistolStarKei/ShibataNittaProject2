using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PathologicalGames;

public enum ShipOffset{Forward,ForwardRight,ForwardLeft,Right,Left,Back,BackLeft,BackRight};
public class shipControl : Photon.MonoBehaviour, IPunObservable {
	[System.Serializable]
	public struct PlayerData {
		public string userName;
		public string countlyCode;
		public int playerID;
		public bool connected;
		public bool dead;
		public float alive;
		public void SetConnected(bool connect){
			connected=connect;
		}
		public void SetAlive(float time){
			alive=time;
		}
		public void SetDead(bool isDead){
			dead=isDead;
		}
		public PlayerData(string name,string countly,int id){
			userName=name;
			countlyCode=countly;
			playerID=id;
			connected=true;
			dead=false;
			alive=0.0f;
		}
	}
	//[HideInInspector]
	public float MaxHP=1500.0f;
	//[HideInInspector]
	public float currentHP=1500.0f;
	public bool usingLog=true;
	public bool isControllable=false;

	Vector3 newRotation = new Vector3(0,0,0);


	#region Init Data
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

	public bool debugPlayership=false;
	void Start(){
		rd = GetComponent<Rigidbody> (); 
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
		this.razerMaxdistance=PSParams.GameParameters.razerMaxDistance;

		currentHP=MaxHP;
		if(isOwnersShip() && GUIManager.Instance.isDebugMode){
			GUIManager.Instance.hpSlider.SetDebugVal(currentHP.ToString()+"/"+MaxHP);
		}

		if(!photonTransformView && debugPlayership){
			GUIManager.Instance.SetShipControll(this);
			isControllable=true;
			StartShooting();
		}

		isDead=false;

		isShooting=true;
	}
	#endregion

	public void StartShooting(){
		if(isOwnersShip())StartCoroutine(Shot());
	}

	#region On Dead
	public bool isDead=false;
	[PunRPC]
	void OnDead(int killedBy,float aliveTime){

		if(isDead)return;
		//ここで判定
		if(usingLog)Debug.Log("OnDead  killed by"+PSPhoton.GameManager.instance.GetNameById(killedBy));

		isDead=true;
		isControllable=false;
		rd.isKinematic=true;

		ParticleManager.Instance.ShowExplosionBigAt(transform.position,Quaternion.identity,null);

		gameObject.SetActive(false);
		if(photonView){
			if(PSPhoton.GameManager.instance){
				PSPhoton.GameManager.instance.OnPlayerDead(playerData.playerID,killedBy,aliveTime);
			}

		}

		if(!isOwnersShip())return;

			if(photonView){
				photonView.RPC ("ScatterWeapons", PhotonTargets.AllViaServer,new object[]{GUIManager.Instance.GetSubweaponInHolder()});
			}else{
				ScatterWeapons(GUIManager.Instance.GetSubweaponInHolder());
			}

	}
		
	[PunRPC]
	public void ScatterWeapons(int[] subweaponInHolder){
		
		Debug.LogWarning("ここでサブウェポンをばらまく");
		if(subweaponInHolder==null)return;

		for(int i=0;i<subweaponInHolder.Length;i++){
			
		}

	}
	#endregion


	#region shots and subweapons
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

	double lastShotTime=0.0d;
	double photonTime=0.0d;

	double RTTExpectation=0.1d;

	IEnumerator Shot(){
		Debug.Log("ローカル発射コルーチン　"+this.playerData.userName);

		while(true && !isDead){

			if(photonView){
				photonTime=PhotonNetwork.time+RTTExpectation;
				if(lastShotTime!=0.0f){
					//shotCoroutineのdulationと合わせるため、時間を調整する。
					//結果、shotで受け取った時に、実際よりも時間が進んでいる可能性がある。
					double sabun=photonTime-lastShotTime;

					if(sabun!=(double)shotDulation){
						photonTime+=(double)shotDulation-sabun;

					}
				}
			}




			if(currentUsing==Subweapon.YUDOU){
				if(weaponNum%2==0){
					shipControl ship=GUIManager.Instance.GetRandomNearShip(PSParams.GameParameters.yudoudanMaxDistance);

					if(photonView){
						photonView.RPC ("RPC_SpawnSubweapon_YudoudanRight", PhotonTargets.AllViaServer,new object[]{
							this.transform.position+ GetShotOffset(ShipOffset.Right),this.transform.rotation.eulerAngles,PSGameUtils.GameUtils.ConvertToFloat((float)photonTime),PSGameUtils.GameUtils.uniqueID(),ship?ship.playerData.playerID:-1});
					}else{
						RPC_SpawnSubweapon_YudoudanRight(this.transform.position+ GetShotOffset(ShipOffset.Right),this.transform.rotation.eulerAngles,Time.realtimeSinceStartup,PSGameUtils.GameUtils.uniqueID(),ship?ship.playerData.playerID:-1);
					}
				}else{
					shipControl ship=GUIManager.Instance.GetRandomNearShip(PSParams.GameParameters.yudoudanMaxDistance);

					if(photonView){
						photonView.RPC ("RPC_SpawnSubweapon_YudoudanLeft", PhotonTargets.AllViaServer,new object[]{
							this.transform.position+ GetShotOffset(ShipOffset.Left),this.transform.rotation.eulerAngles,PSGameUtils.GameUtils.ConvertToFloat((float)photonTime),PSGameUtils.GameUtils.uniqueID(),ship?ship.playerData.playerID:-1});
					}else{
						RPC_SpawnSubweapon_YudoudanLeft(this.transform.position+ GetShotOffset(ShipOffset.Left),this.transform.rotation.eulerAngles,Time.realtimeSinceStartup,PSGameUtils.GameUtils.uniqueID(),ship?ship.playerData.playerID:-1);
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
						this.transform.position+ GetShotOffset(ShipOffset.Forward),this.transform.rotation.eulerAngles,PSGameUtils.GameUtils.ConvertToFloat((float)photonTime),PSGameUtils.GameUtils.uniqueID()});
				}else{
					RPC_SpawnSubweapon_Wave(this.transform.position+ GetShotOffset(ShipOffset.Forward),this.transform.rotation.eulerAngles,Time.realtimeSinceStartup,PSGameUtils.GameUtils.uniqueID());
				}

			}

			if(currentUsing==Subweapon.ZENHOUKOU){
				Vector3[] pos=new Vector3[8];
				Vector3[] rot=new Vector3[8];
				int[] offsets=new int[8];
				string[] IDs=new string[8];
				for(int i=0;i<IDs.Length;i++){
					IDs[i]=PSGameUtils.GameUtils.uniqueID();
				}
					pos[0]=this.transform.position+ GetShotOffset(ShipOffset.Forward);
					rot[0]=this.transform.rotation.eulerAngles;
					offsets[0]=(int)ShipOffset.Forward;
					pos[1]=this.transform.position+ GetShotOffset(ShipOffset.Back);
					rot[1]=Quaternion.LookRotation(-transform.forward,transform.up).eulerAngles;
					offsets[1]=(int)ShipOffset.Back;
					pos[2]=this.transform.position+ GetShotOffset(ShipOffset.Right);
					rot[2]=Quaternion.LookRotation(transform.right,transform.up).eulerAngles;
					offsets[2]=(int)ShipOffset.Right;
					pos[3]=this.transform.position+ GetShotOffset(ShipOffset.Left);
					rot[3]=Quaternion.LookRotation(-transform.right,transform.up).eulerAngles;
					offsets[3]=(int)ShipOffset.Left;

					pos[4]=this.transform.position+ GetShotOffset(ShipOffset.ForwardLeft);
					rot[4]=Quaternion.LookRotation(-transform.right+transform.forward,transform.up).eulerAngles;
					offsets[4]=(int)ShipOffset.ForwardLeft;

					pos[5]=this.transform.position+ GetShotOffset(ShipOffset.ForwardRight);
					rot[5]=Quaternion.LookRotation(transform.right+transform.forward,transform.up).eulerAngles;
					offsets[5]=(int)ShipOffset.ForwardRight;

					pos[6]=this.transform.position+ GetShotOffset(ShipOffset.BackRight);
					rot[6]=Quaternion.LookRotation(transform.right-transform.forward,transform.up).eulerAngles;
					offsets[6]=(int)ShipOffset.BackRight;

					pos[7]=this.transform.position+ GetShotOffset(ShipOffset.BackLeft);
					rot[7]=Quaternion.LookRotation(-transform.right-transform.forward,transform.up).eulerAngles;
					offsets[7]=(int)ShipOffset.BackLeft;

				if(photonView){
					photonView.RPC ("RPC_SpawnSubweapon_Zenhoukou", PhotonTargets.AllViaServer,new object[]{
						pos,rot,PSGameUtils.GameUtils.ConvertToFloat((float)photonTime),offsets,IDs});
				}else{
					RPC_SpawnSubweapon_Zenhoukou(pos,rot,Time.realtimeSinceStartup,offsets,IDs);
				}

			
			}

			if(stopShot || !isShooting){
				//通常弾の処理を停止
			}else{
				switch(shot_rensou){
					case 1:
						if(photonView){
							
							photonView.RPC ("RPC_SpawnShot", PhotonTargets.AllViaServer,new object[]{
							this.transform.position+ GetShotOffset(ShipOffset.Forward),this.transform.rotation.eulerAngles,PSGameUtils.GameUtils.ConvertToFloat((float)photonTime),PSGameUtils.GameUtils.uniqueID()});

						}else{
						
							RPC_SpawnShot(this.transform.position+ GetShotOffset(ShipOffset.Forward),this.transform.rotation.eulerAngles,Time.realtimeSinceStartup,PSGameUtils.GameUtils.uniqueID());
						}
						
						break;
					case 2:
						if(photonView){
							photonView.RPC ("RPC_SpawnShotDouble", PhotonTargets.AllViaServer,new object[]{
							this.transform.position+ GetShotOffset(ShipOffset.ForwardRight),
							this.transform.position+ GetShotOffset(ShipOffset.ForwardLeft),this.transform.rotation.eulerAngles,PSGameUtils.GameUtils.ConvertToFloat((float)photonTime),PSGameUtils.GameUtils.uniqueID()});
						}else{
							RPC_SpawnShotDouble(this.transform.position+ GetShotOffset(ShipOffset.ForwardRight),
							this.transform.position+ GetShotOffset(ShipOffset.ForwardLeft),this.transform.rotation.eulerAngles,Time.realtimeSinceStartup,PSGameUtils.GameUtils.uniqueID());
						}
						break;
					case 3:
						if(photonView){
							photonView.RPC ("RPC_SpawnShotTripple", PhotonTargets.AllViaServer,new object[]{
							this.transform.position+ GetShotOffset(ShipOffset.Forward),this.transform.position+ GetShotOffset(ShipOffset.ForwardRight),this.transform.position+ GetShotOffset(ShipOffset.ForwardLeft)
							,this.transform.rotation.eulerAngles,PSGameUtils.GameUtils.ConvertToFloat((float)photonTime),PSGameUtils.GameUtils.uniqueID()});
						}else{
						RPC_SpawnShotTripple(this.transform.position+ GetShotOffset(ShipOffset.Forward),this.transform.position+ GetShotOffset(ShipOffset.ForwardRight),this.transform.position+ GetShotOffset(ShipOffset.ForwardLeft)
							,this.transform.rotation.eulerAngles,Time.realtimeSinceStartup,PSGameUtils.GameUtils.uniqueID());
						}
						break;
				}
			}

			lastShotTime=PhotonNetwork.time+RTTExpectation;
			yield return new WaitForSeconds(shotDulation);
		}

	}


	public Vector3 GetShotOffset(ShipOffset offset){
		Vector3 off=Vector3.zero;
		switch(offset){
			case ShipOffset.Forward:
				off=transform.forward *shotOffset;
				break;
			case ShipOffset.ForwardRight:
				off=transform.forward *shotOffset+(transform.right *shotOffsetX);
				break;
			case ShipOffset.ForwardLeft:
				off=transform.forward *shotOffset-(transform.right *shotOffsetX);
				break;
			case ShipOffset.Right:
				off=transform.right *shotOffsetX;
				break;
			case ShipOffset.Left:
				off=-transform.right *shotOffsetX;
				break;
			case ShipOffset.Back:
				off=-transform.forward *shotOffset;
				break;
			case ShipOffset.BackRight:
				off=-transform.forward *shotOffset+(transform.right *shotOffsetX);
				break;
			case ShipOffset.BackLeft:
				off=-transform.forward *shotOffset-(transform.right *shotOffsetX);
				break;
		}
		return off;
	}



	//ナパーム弾
	public void ShotNapam(){
		if(photonView){
			photonView.RPC ("RPC_Subweapon_Napam", PhotonTargets.AllViaServer,new object[]{
				this.transform.position+ GetShotOffset(ShipOffset.Forward),this.transform.rotation.eulerAngles,PSGameUtils.GameUtils.ConvertToFloat((float)(PhotonNetwork.time+RTTExpectation)),PSGameUtils.GameUtils.uniqueID()});
		}else{
			RPC_Subweapon_Napam(this.transform.position+ GetShotOffset(ShipOffset.Forward),this.transform.rotation.eulerAngles,Time.realtimeSinceStartup,PSGameUtils.GameUtils.uniqueID());
		}
	}
	public void ShotNapam_Effect(Vector3 position){
		if(photonView){
			photonView.RPC ("RPC_SpawnNapamEffecter", PhotonTargets.AllViaServer,new object[]{
				position});
		}else{
			RPC_SpawnNapamEffecter(position);
		}


	}

	//ヌーク弾
	public void ShotNuke(){
		
		if(photonView){
			photonView.RPC ("RPC_Subweapon_Nuke", PhotonTargets.AllViaServer,new object[]{
				this.transform.position+ GetShotOffset(ShipOffset.Forward),this.transform.rotation.eulerAngles,PSGameUtils.GameUtils.ConvertToFloat((float)(PhotonNetwork.time+RTTExpectation)),PSGameUtils.GameUtils.uniqueID()});
		}else{
			RPC_Subweapon_Nuke(this.transform.position+ GetShotOffset(ShipOffset.Forward),this.transform.rotation.eulerAngles,Time.realtimeSinceStartup,PSGameUtils.GameUtils.uniqueID());
		}
	}

	public void ShotNuke_Effect(Vector3 position){
		if(photonView){
			photonView.RPC ("RPC_SpawnNukeEffecter", PhotonTargets.AllViaServer,new object[]{
				position});
		}else{
			RPC_SpawnNukeEffecter(position);
		}


	}


	public List<SubweaponShot> shottedWeaponsHolder=new List<SubweaponShot>();
	[PunRPC]
	public void RPCDestroyWeaponByID(string ID,int playerID){

		for(int i=0;i<shottedWeaponsHolder.Count;i++){
			if(shottedWeaponsHolder[i]!=null){
				if(shottedWeaponsHolder[i].ID==ID){
					shottedWeaponsHolder[i].EffectAndDead(PSPhoton.GameManager.instance.GetShipById(playerID));
				}
			}
		}
	}
	public void RemoveWeaponHolder(SubweaponShot shot){
		if(shottedWeaponsHolder.Contains(shot)){
			shottedWeaponsHolder.Remove(shot);
		}
	}
	void AddWeaponHolder(Transform shot){
		SubweaponShot sws=shot.gameObject.GetComponent<SubweaponShot>();
		if(sws){
			shottedWeaponsHolder.Add(sws);
		}
	}

		[PunRPC]
		public void RPC_SpawnSubweapon_YudoudanRight(Vector3 position,Vector3 rotation,float spawnTime,string ID,int shipID){
			Transform tr=PickupAndWeaponManager.Instance.SpawnSubweapon_Yudoudan(this,position,Quaternion.Euler(rotation),spawnTime,ShipOffset.Right,ID,shipID);
			AddWeaponHolder(tr);
		}
		[PunRPC]
		public void RPC_SpawnSubweapon_YudoudanLeft(Vector3 position,Vector3 rotation,float spawnTime,string ID,int shipID){
			Transform tr=PickupAndWeaponManager.Instance.SpawnSubweapon_Yudoudan(this,position,Quaternion.Euler(rotation),spawnTime,ShipOffset.Left,ID,shipID);
			AddWeaponHolder(tr);
		}


		[PunRPC]
		public void RPC_Subweapon_Napam(Vector3 position,Vector3 rotation,float spawnTime,string ID){
			Transform tr=PickupAndWeaponManager.Instance.SpawnSubweapon_Napam(this,position,Quaternion.Euler(rotation),spawnTime,ShipOffset.Forward,ID);
			AddWeaponHolder(tr);
		}
		[PunRPC]
		public void RPC_Subweapon_Nuke(Vector3 position,Vector3 rotation,float spawnTime,string ID){
		
			Transform tr=PickupAndWeaponManager.Instance.SpawnSubweapon_Nuke(this,position,Quaternion.Euler(rotation),spawnTime,ShipOffset.Forward,ID);
			AddWeaponHolder(tr);
		}

		[PunRPC]
		public void RPC_SpawnShot(Vector3 position,Vector3 rotation,float spawnTime,string ID){
			Transform tr=PickupAndWeaponManager.Instance.SpawnShot(this,shotCol,position,Quaternion.Euler(rotation),spawnTime,ShipOffset.Forward,ID);
			AddWeaponHolder(tr);
			tr=PickupAndWeaponManager.Instance.SpawnShot(this,DanmakuColor.Blue,position,Quaternion.Euler(rotation),spawnTime,ShipOffset.Forward,ID);
			AddWeaponHolder(tr);
		}

		[PunRPC]
		public void RPC_SpawnShotDouble(Vector3 position1,Vector3 position2,Vector3 rotation,float spawnTime,string ID){
			Transform  tr=PickupAndWeaponManager.Instance.SpawnShot(this,shotCol,position1,Quaternion.Euler(rotation),spawnTime,ShipOffset.ForwardRight,ID);
			AddWeaponHolder(tr);
			tr=PickupAndWeaponManager.Instance.SpawnShot(this,shotCol,position2,Quaternion.Euler(rotation),spawnTime,ShipOffset.ForwardLeft,ID);
			AddWeaponHolder(tr);
		}

		[PunRPC]
		public void RPC_SpawnShotTripple(Vector3 position1,Vector3 position2,Vector3 position3,Vector3 rotation,float spawnTime,string ID){
			Transform tr=PickupAndWeaponManager.Instance.SpawnShot(this,shotCol,position1,Quaternion.Euler(rotation),spawnTime,ShipOffset.Forward,ID);
			AddWeaponHolder(tr);
			tr=PickupAndWeaponManager.Instance.SpawnShot(this,shotCol,position2,Quaternion.Euler(rotation),spawnTime,ShipOffset.ForwardRight,ID);
			AddWeaponHolder(tr);
			tr=PickupAndWeaponManager.Instance.SpawnShot(this,shotCol,position3,Quaternion.Euler(rotation),spawnTime,ShipOffset.ForwardLeft,ID);
			AddWeaponHolder(tr);
		}

		
		[PunRPC]
		public void RPC_SpawnSubweapon_Wave(Vector3 position,Vector3 rotation,float spawnTime,string ID){
			Transform tr=PickupAndWeaponManager.Instance.SpawnSubweapon_Wave(this,position,Quaternion.Euler(rotation),spawnTime,ShipOffset.Forward,ID);
			AddWeaponHolder(tr);
		}
		[PunRPC]
		public void RPC_SpawnSubweapon_Zenhoukou(Vector3[] position,Vector3[] rotation,float spawnTime,int[] shipOffset,string[] IDs){
			if(position.Length!=rotation.Length){
				Debug.LogError("全方向の配列の個数が違う");
			}else{
				Transform tr=null;
				for(int i=0;i<position.Length;i++){
					tr=PickupAndWeaponManager.Instance.SpawnSubweapon_Zenhoukou(this,position[i],Quaternion.Euler(rotation[i]),spawnTime,(ShipOffset)shipOffset[i],IDs[i]);
					AddWeaponHolder(tr);
				}
			}
		}
		[PunRPC]
		public void RPC_SpawnNapamEffecter(Vector3 position){
			PickupAndWeaponManager.Instance.SpawnSubweapon_NapamEffecter(this,position,Quaternion.identity,null);
		}
		[PunRPC]
		public void RPC_SpawnNukeEffecter(Vector3 position){
			PickupAndWeaponManager.Instance.SpawnSubweapon_NukeEffecter(this,position,Quaternion.identity,null);
		}


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
	#endregion

	#region use subweapons
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
				currentUsing=Subweapon.NONE;
				if(isShooting){
					OnShotToggle(true);
					GUIManager.Instance.SetShotTgl(true);
				}
				stopShot=false;
			}else{
				StealthMode(false);
				currentUsing=Subweapon.NONE;
				if(isShooting){
					OnShotToggle(true);
					GUIManager.Instance.SetShotTgl(true);
				}
				stopShot=false;
			}

		}else if(currentUsing==Subweapon.RAZER){
			if(photonView){
				if(razerTarget && razerTarget.photonView){
					razerTarget.photonView.RPC("OnRazerTargeted", PhotonTargets.AllViaServer,new object[]{false,playerData.playerID});
				}
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
		if(usingLog)Debug.Log("サブウェポンの使用 "+weaponType.ToString());
		if(isDead || currentUsing!=Subweapon.NONE)return;

		if(usingLog)Debug.Log("サブウェポンの使用 "+weaponType.ToString());

		if(isOwnersShip()){
				UseSubWeapon((int)weaponType);
		}
	}

	public void UseSubWeapon(int weaponType){

		currentUsing=(Subweapon)weaponType;

		weaponNum=0;
		stopShot=false;
		switch(currentUsing){
		case Subweapon.WAVE:

			weapontimer=PSParams.GameParameters.sw_timer[(int)Subweapon.WAVE];
			stopShot=PSParams.GameParameters.sw_isShotOff[(int)Subweapon.WAVE];
			break;
		case Subweapon.ZENHOUKOU:
			weapontimer=PSParams.GameParameters.sw_timer[(int)Subweapon.ZENHOUKOU];
			stopShot=PSParams.GameParameters.sw_isShotOff[(int)Subweapon.ZENHOUKOU];
			break;
		case Subweapon.NAPAM:


			weapontimer=PSParams.GameParameters.sw_timer[(int)Subweapon.NAPAM];
			stopShot=PSParams.GameParameters.sw_isShotOff[(int)Subweapon.NAPAM];
			ShotNapam();
			break;
		case Subweapon.NUKE:
			weapontimer=PSParams.GameParameters.sw_timer[(int)Subweapon.NUKE];
			stopShot=PSParams.GameParameters.sw_isShotOff[(int)Subweapon.NUKE];
			ShotNuke();
			break;
		case Subweapon.YUDOU:
			weapontimer=PSParams.GameParameters.sw_timer[(int)Subweapon.YUDOU];
			stopShot=PSParams.GameParameters.sw_isShotOff[(int)Subweapon.YUDOU];
			//発射個数
			weaponNum=PSParams.GameParameters.yudoudanShots;
			break;
		case Subweapon.STEALTH:
			GUIManager.Instance.SetShotTgl(false);
			weapontimer=PSParams.GameParameters.sw_timer[(int)Subweapon.STEALTH];
			OnShotToggle(false);
			if(photonView){
				photonView.RPC ("StealthMode", PhotonTargets.AllViaServer,new object[]{true});
			}else{
				StealthMode(true);
			}

			break;
		case Subweapon.RAZER:
			
			weapontimer=PSParams.GameParameters.sw_timer[(int)Subweapon.RAZER];
			stopShot=PSParams.GameParameters.sw_isShotOff[(int)Subweapon.RAZER];
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

	public Razer razerSysytem;
	public shipControl razerTarget;
	[PunRPC]
	public void RazerMode(bool isOn){
		if(isOn){
			razerTarget=null;
			razerSysytem.ShowLine(null,this);
		}else{
			razerTarget=null;
			razerSysytem.HideLine();
		}
	}

	float razerMaxdistance=1.0f;

	[PunRPC]
	public void OnRazerTargetChanged(int id){
		
		razerTarget=PSPhoton.GameManager.instance.GetShipById(id);
	}
	[PunRPC]
	public void OnRazerTargetNull(){

		razerTarget=null;

	}

	#endregion

	#region damage


	public bool isInDanzerZone=false;
	public void OnEnterDangerZone(bool isOn){
		if(isOwnersShip()){
			isInDanzerZone=isOn;
			if(isOn){

				InvokeRepeating("ConstantDangerZoneDamage",PSParams.GameParameters.razerDamageDulation,PSParams.GameParameters.razerDamageDulation);
			}else{
				CancelInvoke("ConstantDangerZoneDamage");
			}
		}
	}

	public void ConstantDangerZoneDamage(){
		//ダメージと死亡判定、プレイヤーオブジェクトのみでやる
		if(isOwnersShip() && !isDead){

			if(photonView){

				//HPバーの更新、プレイヤーオブジェクトのみでやる

				currentHP-=PSParams.GameParameters.dangerZoneDamage;
				if(GUIManager.Instance.isDebugMode){
					GUIManager.Instance.hpSlider.SetDebugVal((currentHP<0.0f?0.0f:currentHP).ToString()+"/"+MaxHP);
				}
				GUIManager.Instance.Damage (PSParams.GameParameters.dangerZoneDamage, MaxHP);

				if(currentHP<=0.0f){
					photonView.RPC ("OnDead", PhotonTargets.AllBufferedViaServer,new object[]{null,PSPhoton.GameManager.instance.gameTime});
				}

			}else{
				//HPバーの更新、プレイヤーオブジェクトのみでやる
				currentHP-=PSParams.GameParameters.dangerZoneDamage;
				if(GUIManager.Instance.isDebugMode){
					GUIManager.Instance.hpSlider.SetDebugVal((currentHP<0.0f?0.0f:currentHP).ToString()+"/"+MaxHP);
				}
				GUIManager.Instance.Damage (PSParams.GameParameters.dangerZoneDamage, MaxHP);
				if(currentHP<=0.0f){
					Debug.Log("shipは死亡！");
				}


			}
		}
	}


	public bool isRazerTArgeted=false;

	int targettedBy=0;
	[PunRPC]
	public void OnRazerTargeted(bool isOn,int tagettedBy){
		if(isOwnersShip()){
			this.targettedBy=tagettedBy;
			if(isRazerTArgeted==isOn)return;
			isRazerTArgeted=isOn;
			Debug.Log("OnRazerTargeted"+isOn);
			if(isOn){
				InvokeRepeating("ConstantRazerDamage",PSParams.GameParameters.razerDamageDulation,PSParams.GameParameters.razerDamageDulation);
			}else{
				CancelInvoke("ConstantRazerDamage");
			}
		}
	}

	public void ConstantRazerDamage(){
		//ダメージと死亡判定、プレイヤーオブジェクトのみでやる
		if(isOwnersShip() && !isDead){

			if(photonView){

				//HPバーの更新、プレイヤーオブジェクトのみでやる

				currentHP-=PSParams.GameParameters.sw_damage[(int)Subweapon.RAZER];
				if(GUIManager.Instance.isDebugMode){
					GUIManager.Instance.hpSlider.SetDebugVal((currentHP<0.0f?0.0f:currentHP).ToString()+"/"+MaxHP);
				}
				GUIManager.Instance.Damage (PSParams.GameParameters.sw_damage[(int)Subweapon.RAZER], MaxHP);

				if(currentHP<=0.0f){
					photonView.RPC ("OnDead", PhotonTargets.AllBufferedViaServer,new object[]{targettedBy,PSPhoton.GameManager.instance.gameTime});
				}

			}else{
				//HPバーの更新、プレイヤーオブジェクトのみでやる
				currentHP-=PSParams.GameParameters.sw_damage[(int)Subweapon.RAZER];
				if(GUIManager.Instance.isDebugMode){
					GUIManager.Instance.hpSlider.SetDebugVal((currentHP<0.0f?0.0f:currentHP).ToString()+"/"+MaxHP);
				}
				GUIManager.Instance.Damage (PSParams.GameParameters.sw_damage[(int)Subweapon.RAZER], MaxHP);
				if(currentHP<=0.0f){
					Debug.Log("shipは死亡！");
				}


			}
		}
	}

	//通常弾　サブウェポン食らった場合
	public void OnHit(shipControl enemy,Subweapon type,float damage,string ID){
		
		if(usingLog)Debug.Log(enemy.playerData.userName+" の弾幕がヒット！"+ID);

		if(isDead)return;

		//当たった人間が自己申告して、発弾者経由で　弾幕を消す指令をだす。
		if(ID!="" && enemy.photonView){
			enemy.photonView.RPC ("RPCDestroyWeaponByID", PhotonTargets.AllViaServer,new object[]{
				ID,playerData.playerID});
		}


		//デバグ用　
		if(!PSPhoton.GameManager.instance.isNetworkMode && ID!=""){
	
			for(int i=0;i<enemy.shottedWeaponsHolder.Count;i++){
				
				if(enemy.shottedWeaponsHolder[i]!=null){

					if(enemy.shottedWeaponsHolder[i].ID==ID){
						
						enemy.shottedWeaponsHolder[i].EffectAndDead(this);
					}
				}
			}

		}

		//ダメージと死亡判定、プレイヤーオブジェクトのみでやる
		if(isOwnersShip()){

		
			if(photonView){

				//HPバーの更新、プレイヤーオブジェクトのみでやる
					GUIManager.Instance.ShakeCamera();
					currentHP-=damage;
					if(GUIManager.Instance.isDebugMode){
					GUIManager.Instance.hpSlider.SetDebugVal((currentHP<0.0f?0.0f:currentHP).ToString()+"/"+MaxHP);
					}
					GUIManager.Instance.Damage (damage, MaxHP);
				
				if(currentHP-damage<=0.0f){
					photonView.RPC ("OnDead", PhotonTargets.AllBufferedViaServer,new object[]{playerData.playerID,PSPhoton.GameManager.instance.gameTime});
				}

					


			}else{
				//HPバーの更新、プレイヤーオブジェクトのみでやる
					GUIManager.Instance.ShakeCamera();
					currentHP-=damage;
					if(GUIManager.Instance.isDebugMode){
					GUIManager.Instance.hpSlider.SetDebugVal((currentHP<0.0f?0.0f:currentHP).ToString()+"/"+MaxHP);
					}
					GUIManager.Instance.Damage (damage, MaxHP);
					if(currentHP-damage<=0.0f){
						Debug.Log("shipは死亡！");
					}


			}
		}
	}
	#endregion
		

	#region on pick up st
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
	#endregion


	#region sync manually   HP isDead
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		// read the description in SecondsBeforeRespawn

		if (stream.isWriting)
		{
			stream.SendNext(this.currentHP);
			stream.SendNext(this.isDead);
		}
		else
		{
			// this will directly apply the last received position for this PickupItem. No smoothing. Usually not needed though.
			float  current = (float)stream.ReceiveNext();
			this.currentHP = current;
			bool lastIsDead = (bool)stream.ReceiveNext();
			this.isDead = lastIsDead ;
		}
	}
	#endregion




	#region controll
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




		if(!isOwnersShip())return;

			//PlayerのShipのみで呼ばれる
			if(currentUsing!=Subweapon.NONE){
				weapontimer-=Time.deltaTime;
				if(weapontimer<0.0f){
					OnWeaponTimerOver();
				}
				
				if(currentUsing==Subweapon.RAZER){
				
					if(razerTarget==null){
						shipControl near=GUIManager.Instance.GetNearestShip(transform.position,razerMaxdistance);
						if(near){
							if(photonView){
								photonView.RPC ("OnRazerTargetChanged", PhotonTargets.AllViaServer,new object[]{near.playerData.playerID});
							near.photonView.RPC("OnRazerTargeted", PhotonTargets.AllViaServer,new object[]{true,playerData.playerID});
							}else{
								OnRazerTargetChanged(near.playerData.playerID);
							}
						}else{
						}
					}else{
						if(razerTarget.isDead){
							if(photonView){
							if(razerTarget.photonView)razerTarget.photonView.RPC("OnRazerTargeted", PhotonTargets.AllViaServer,new object[]{false,playerData.playerID});
								photonView.RPC ("OnRazerTargetNull", PhotonTargets.AllViaServer,null);
							}else{
								OnRazerTargetNull();
							}
						}else{
							//ここで射程範囲の確認
							if(Vector3.Distance(razerTarget.transform.position,transform.position)>razerMaxdistance){
								if(photonView){
								if(razerTarget.photonView)razerTarget.photonView.RPC("OnRazerTargeted", PhotonTargets.AllViaServer,new object[]{false,playerData.playerID});
									photonView.RPC ("OnRazerTargetNull", PhotonTargets.AllViaServer,null);
								}else{
									OnRazerTargetNull();
								}
							}
						}
					}

					


				}
			}


			if(!isInDanzerZone){
				if(!PSPhoton.GameManager.instance.safeZone.IsInSafeZone(transform.position))OnEnterDangerZone(true);
			}else{
				if(PSPhoton.GameManager.instance.safeZone.IsInSafeZone(transform.position))OnEnterDangerZone(false);
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
	#endregion

	public bool isOwnersShip(){
		return GUIManager.Instance.shipControll==this?true:false;
	}
}
