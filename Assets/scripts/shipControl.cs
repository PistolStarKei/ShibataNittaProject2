using UnityEngine;
using System.Collections;
using PathologicalGames;

public class shipControl : MonoBehaviour {


	public float MaxHP=1500.0f;
	public float currentHP=1500.0f;
	public bool isDead=false;
	public bool isOwnerShip=false;

	Vector3 newRotation = new Vector3(0,0,0);





	void Start(){


		//GUIManagerに機体を設定
		if(isOwnerShip)GUIManager.Instance.SetShipControll(this);

		rd = GetComponent<Rigidbody> (); 
		razerLine=GetComponent<LineRenderer>();
		gameObject.tag="Player";

		isShooting=true;
		InvokeRepeating("Shot",0.0f,shotDulation);

		isPressed=false;
		currentHP=MaxHP;
	}

	void OnDead(){
		isDead=true;
		gameObject.GetComponent<SphereCollider>().enabled=false;
		gameObject.GetComponent<Rigidbody>().isKinematic=true;
		//ここで判定
		ParticleManager.Instance.ShowExplosionBigAt(transform.position,Quaternion.identity,this.transform);
		engine.SetActive(false);

	}

	public bool isOwnersShip(){
		return GUIManager.Instance.shipControll==this?true:false;
	}


	//通常弾
	public float shotDulation=0.2f;
	public float shotOffset=0.2f;
	public float shotOffsetX=0.1f;
	public int shot_rensou=1;
	public DanmakuColor shotCol;

	bool stopShot=false;
	public void Shot(){

		if(currentUsing==Subweapon.YUDOU){
			if(weaponNum%2==0){
				PickupAndWeaponManager.Instance.SpawnSubweapon_Yudoudan(this,this.transform.position+ transform.right *0.1f,this.transform.rotation,null);
			}else{
				PickupAndWeaponManager.Instance.SpawnSubweapon_Yudoudan(this,this.transform.position-transform.right *0.1f,this.transform.rotation,null);
			}

			weaponNum--;
			if(weaponNum<=0){
				OnWeaponTimerOver();
			}
		}
		if(currentUsing==Subweapon.WAVE)PickupAndWeaponManager.Instance.SpawnSubweapon_Wave(this,this.transform.position+ transform.forward *shotOffset,this.transform.rotation,null);

		if(currentUsing==Subweapon.ZENHOUKOU){
			//前
			PickupAndWeaponManager.Instance.SpawnSubweapon_Zenhoukou(this,this.transform.position+ transform.forward *shotOffset,this.transform.rotation,null);
			//後ろ
			PickupAndWeaponManager.Instance.SpawnSubweapon_Zenhoukou(this,this.transform.position+ -transform.forward *shotOffset,Quaternion.LookRotation(-transform.forward,transform.up),null);
			//右
			PickupAndWeaponManager.Instance.SpawnSubweapon_Zenhoukou(this,this.transform.position+ (transform.right *shotOffsetX),Quaternion.LookRotation(transform.right,transform.up),null);
			//左
			PickupAndWeaponManager.Instance.SpawnSubweapon_Zenhoukou(this,this.transform.position+ (-transform.right *shotOffsetX),Quaternion.LookRotation(-transform.right,transform.up),null);
			//左前
			PickupAndWeaponManager.Instance.SpawnSubweapon_Zenhoukou(this,this.transform.position+ (transform.forward *shotOffset)+ (-transform.right *shotOffsetX),Quaternion.LookRotation(-transform.right+transform.forward,transform.up),null);
			//右前
			PickupAndWeaponManager.Instance.SpawnSubweapon_Zenhoukou(this,this.transform.position+ (transform.forward *shotOffset)+ (transform.right *shotOffsetX),Quaternion.LookRotation(transform.right+transform.forward,transform.up),null);
		
			//右後
			PickupAndWeaponManager.Instance.SpawnSubweapon_Zenhoukou(this,this.transform.position+ (-transform.forward *shotOffset)+ (transform.right *shotOffsetX),Quaternion.LookRotation(transform.right-transform.forward,transform.up),null);
			//左後
			PickupAndWeaponManager.Instance.SpawnSubweapon_Zenhoukou(this,this.transform.position+ (-transform.forward *shotOffset)+ (-transform.right *shotOffsetX),Quaternion.LookRotation(-transform.right-transform.forward,transform.up),null);

		
		}

		if(stopShot)return;
		switch(shot_rensou){
			case 1:
				PickupAndWeaponManager.Instance.SpawnShot(this,shotCol,this.transform.position+ transform.forward *shotOffset,this.transform.rotation,null);
				
				break;
			case 2:
				PickupAndWeaponManager.Instance.SpawnShot(this,shotCol,this.transform.position+ transform.forward *shotOffset+ (transform.right *shotOffsetX),this.transform.rotation,null);
		
				PickupAndWeaponManager.Instance.SpawnShot(this,shotCol,this.transform.position+ transform.forward *shotOffset+ (-transform.right *shotOffsetX),this.transform.rotation,null);
		
				break;
			case 3:
				PickupAndWeaponManager.Instance.SpawnShot(this,shotCol,this.transform.position+ transform.forward *shotOffset+ (transform.right *shotOffsetX),this.transform.rotation,null);

				PickupAndWeaponManager.Instance.SpawnShot(this,shotCol,this.transform.position+ transform.forward *shotOffset+ (-transform.right *shotOffsetX),this.transform.rotation,null);
				PickupAndWeaponManager.Instance.SpawnShot(this,shotCol,this.transform.position+ transform.forward *shotOffset,this.transform.rotation,null);
				break;
		}


	}

	//ナパーム弾
	public void ShotNapam(){
		PickupAndWeaponManager.Instance.SpawnSubweapon_Napam(this,this.transform.position+ transform.forward *shotOffset,this.transform.rotation,null);
		//OnWeaponTimerOver();
	}

	//ヌーく弾
	public void ShotNuke(){
		PickupAndWeaponManager.Instance.SpawnSubweapon_Nuke(this,this.transform.position+ transform.forward *shotOffset,this.transform.rotation,null);
		//OnWeaponTimerOver();
	}

	bool isShooting=false;
	public void OnShotToggle(bool val){
		Debug.Log("OnShotToggle "+val);
		if(isDead)return;
		if(val){
			//ShotがOnになったの時
			if(isShooting)return;
			if(currentUsing==Subweapon.STEALTH){
				OnWeaponTimerOver();
			}
			isShooting=true;
			InvokeRepeating("Shot",0.0f,shotDulation);
		}else{
			//ShotがOffになったの時
			if(!isShooting)return;
			isShooting=false;
			CancelInvoke("Shot");
		}


	}



	public StealthEffecter stealthEffecter;
	public GameObject engine;

	float weapontimer=0.0f;
	int weaponNum=0;
	public Subweapon currentUsing=Subweapon.NONE;

	void OnWeaponTimerOver(){
		if(currentUsing==Subweapon.STEALTH){
			StealthMode(false);
		}else if(currentUsing==Subweapon.RAZER){
			razerLine.enabled=false;
		}

		currentUsing=Subweapon.NONE;
		stopShot=false;
		GUIManager.Instance.EnableSubweapon();
	}

	public void OnUseSubWeapon(Subweapon weaponType){
		Debug.Log("OnUseSubWeapon "+weaponType.ToString());
		if(isDead || currentUsing!=Subweapon.NONE)return;
		currentUsing=weaponType;
		weaponNum=0;
		stopShot=false;
		switch(weaponType){
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
					StealthMode(true);
					break;
				case Subweapon.RAZER:
					razerLine.enabled=true;
					weapontimer=10.0f;
					stopShot=false;
					break;
			}

	}

	public void StealthMode(bool isOn){
		if(isOn){
			engine.SetActive(false);
			stealthEffecter.StealthMode(true);
		}else{
			engine.SetActive(true);
			stealthEffecter.StealthMode(false);
		}
	}

	LineRenderer razerLine;
	public float razerMaxdistance=5.0f;
	void SetRazerTarget(Vector3 pos){
		razerLine.SetPosition(1,pos);
	}



	//サブウェポンからのダメージを受けた時
	public void OnHit(Subweapon type,float damage,Vector3 hitPosition){
		if(isDead)return;

		//実際は、エフェクトを出すだけ

		//AudioController.Play ("Explosion");
		ParticleManager.Instance.ShowExplosionSmallAt(new Vector3(hitPosition.x,hitPosition.y+0.5f,hitPosition.z),Quaternion.identity,null);


		//これは、プレイヤーオブジェクトのみでやり　他のプレイヤーに告知する
		if(isOwnerShip){
			currentHP-=damage;
			GUIManager.Instance.Damage (damage, MaxHP);
		}

		if(currentHP<=0.0f){
			OnDead();
		}
	}
		


	//回復を拾った時
	public void OnPickUp_Cure(Pickup pu){
		if(isDead || !isOwnersShip())return;
		switch(pu){
			case Pickup.CureS:
				CureSelf(10.0f);
				break;
			case Pickup.CureM:
				CureSelf(20.0f);
				break;
			case Pickup.CureL:
				CureSelf(30.0f);
				break;
		}
		
	}
	void CureSelf(float percentage){
		AudioController.Play ("Powerup");
		currentHP+=MaxHP*(percentage/100.0f);
		ParticleManager.Instance.ShowCureSAt(transform.position,Quaternion.identity,transform);
		if(currentHP>MaxHP)currentHP=MaxHP;
		GUIManager.Instance.Cure (percentage,MaxHP);
	}
		
	//サブウェポンを拾った時
	public void OnPickUp_Subweapon(Pickup pu){
		if(isDead || !isOwnersShip())return;

		int num=(int)pu;
		num=num-3;
		GUIManager.Instance.OnGetSubWeapon((Subweapon)num);

	}


	//操作系統
	Vector3 temp;
	bool isPressed=false;
	Vector3 currentTappedPos;
	// 移動スピード
	public float maxSpeed = 2.0f;
	public float speed = 0.01f;
	Vector3 tr;
	Rigidbody rd;
	Vector3 velocity;

	//GUIManagerからの入力受け取りメソッド
	public void OnPressTapLayer(bool isPress,Vector3 worldPos){
		//Debug.Log("OnPressTapLayer"+isPress+worldPos.ToString());
		isPressed=isPress;
		currentTappedPos=worldPos;
	}

	public void OnUpdateTapLayer(Vector3 worldPos){
		//Debug.Log("OnPressTapLayer"+worldPos.ToString());
		currentTappedPos=worldPos;
	}
	void Update () {

		if(isDead || !isOwnersShip())return;

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
			Debug.LogWarning("Maxspeedを超えた");
			rd.velocity = rd.velocity.normalized * maxSpeed;
		}

		tr = transform.forward * speed;

		rd.AddForce (tr, ForceMode.VelocityChange);
	}


}
