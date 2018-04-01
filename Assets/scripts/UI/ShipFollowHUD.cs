using UnityEngine;
using System.Collections;
using PSGUI;

public class ShipFollowHUD : FollowGUI {

	public shipControl ship;
	public void SetTarget(shipControl ship){
		this.target=ship.transform;
		this.ship=ship;
		nameLB.text=ship.playerData.userName;
		countly.spriteName=ship.playerData.countlyCode;
	}

	public override void OverlayPosition(){
		if(PSPhoton.GameManager.instance.playerShip.isDead || !ship || ship.isDead || !ship.gameObject.activeSelf){
			Show(false);
			return;
		}

		if(ship.isStealthMode){
			Show(false);
			return;
		}else{
			Show(true);
		}


		if(isInsideView()){
			if(yajirusi.enabled)yajirusi.enabled=false;
			if(!nameLB.enabled)nameLB.enabled=true;
			if(!countly.enabled)countly.enabled=true;

			vec=BalidateInView(GetViewPortPointOfTarget());
			vec=GetOverlayPosition(vec);
			vec+=offset;
			widget.transform.localPosition=vec;
		}else{
			//画面外に出た場合
			if(!yajirusi.enabled)yajirusi.enabled=true;
			if(nameLB.enabled)nameLB.enabled=false;
			if(countly.enabled)countly.enabled=false;
			SetYajirushiColor();
			vec=Balidate(GetViewPortPointOfTarget());
			vec=GetOverlayPosition(vec);

			vec+=offset;
			widget.transform.localPosition=vec;
		}
	}

	public float minDistance=10.0f;
	public float minAlpha=0.1f;
	public float maxAlpha=1.0f;
	public void SetYajirushiColor(){
		float dist=Vector3.Distance(ship.transform.position,GUIManager.Instance.shipControll.transform.position);

		if(dist<=0.0f){
			yajirusi.alpha=maxAlpha;
		}else if(dist>minDistance){
			yajirusi.alpha=minAlpha;
		}else{
			dist=dist/minDistance;

			yajirusi.alpha=minAlpha+(maxAlpha-dist);
		}
			
	}


	[Header( "CustomOffset")]
	public float offset_Left=176.01f;
	public float offset_Right=-176.01f;
	public float offset_Top=-63f;
	public float offset_Btm=63f;
	public float offset_normalY=-63f;
	public float offset_normalX=176.01f;
	public float customOffsetRect=0.2f;


	Vector3 BalidateInView(Vector3 vector){
		
		//左
		if(vector.x<=customOffsetRect){
			offset.x=offset_Left;
			//上
			if(vector.y>=1.0f-customOffsetRect){
				offset.y=offset_Top;
			}else{
				//下
				if(vector.y<=customOffsetRect){
					offset.y=offset_Btm;
				}else{
					offset.y=0.0f;
				}
			}
		}else{
			//右
			if(vector.x>=1.0f-customOffsetRect){
				offset.x=offset_Right;
				//上
				if(vector.y>=1.0f-customOffsetRect){
					offset.y=offset_Top;
				}else{
					//下
					if(vector.y<=customOffsetRect){
						offset.y=offset_Btm;
					}else{
						offset.y=0.0f;
					}
				}
			}else{
				offset.x=offset_normalX;
				//上
				if(vector.y>=1.0f-customOffsetRect){
					offset.y=offset_Top;
				}else{
					//下
					if(vector.y<=customOffsetRect){
						offset.y=offset_Btm;
					}else{
						offset.y=offset_normalY;
					}
				}
			}
		}

		vector.z=0.0f;
		return vector;
	}

	[Header( "CustomOffset_yajirusi")]
	public float offset_Left_yajirusi=176.01f;
	public float offset_Right_yajirusi=-176.01f;
	public float offset_Top_yajirusi=-63f;
	public float offset_Btm_yajirusi=63f;

	Vector3 Balidate(Vector3 vector){
		//左
		if(vector.x<=0.0f){
			offset.x=offset_Left_yajirusi;
			vector.x=0.0f;
			//上
			if(vector.y>=1.0f){
				vector.y=1.0f;
				yajirusi.transform.localRotation=Quaternion.Euler(new Vector3(0.0f,0.0f,45.0f));
				offset.y=offset_Top_yajirusi;
			}else{
				//下
				if(vector.y<=0.0f){
					yajirusi.transform.localRotation=Quaternion.Euler(new Vector3(0.0f,0.0f,135.0f));
					vector.y=0.0f;
					offset.y=offset_Btm_yajirusi;
				}else{
					yajirusi.transform.localRotation=Quaternion.Euler(new Vector3(0.0f,0.0f,90.0f));
					offset.y=0.0f;
				}
			}
		}else{
			//右
			if(vector.x>=1.0f){
				vector.x=1.0f;
				offset.x=offset_Right_yajirusi;
				//上
				if(vector.y>=1.0f){
					vector.y=1.0f;
					yajirusi.transform.localRotation=Quaternion.Euler(new Vector3(0.0f,0.0f,-45.0f));
					offset.y=offset_Top_yajirusi;
				}else{
					//下
					if(vector.y<=0.0f){
						yajirusi.transform.localRotation=Quaternion.Euler(new Vector3(0.0f,0.0f,-135.0f));
						vector.y=0.0f;
						offset.y=offset_Btm_yajirusi;
					}else{
						yajirusi.transform.localRotation=Quaternion.Euler(new Vector3(0.0f,0.0f,-90.0f));
						offset.y=0.0f;
					}
				}
			}else{
				offset.x=offset_normalX;
				//上
				if(vector.y>=1.0f){
					vector.y=1.0f;
					yajirusi.transform.localRotation=Quaternion.Euler(new Vector3(0.0f,0.0f,0.0f));
					offset.y=offset_Top_yajirusi;
				}else{
					//下
					if(vector.y<=0.0f){
						vector.y=0.0f;
						yajirusi.transform.localRotation=Quaternion.Euler(new Vector3(0.0f,0.0f,180.0f));
						offset.y=offset_Btm_yajirusi;
					}else{
						offset.y=offset_normalY;
					}
				}
			}
		}

		vector.z=0.0f;
		return vector;
	}

	[Space(20.0f)]
	public UISprite yajirusi;
	public UILabel nameLB;
	public UISprite countly;


}
