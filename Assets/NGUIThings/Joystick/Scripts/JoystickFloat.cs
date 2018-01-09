using UnityEngine;
using System.Collections;

public class JoystickFloat : PS_SingletonBehaviour<JoystickFloat> 
{


	public void Show(bool isShow){
        if(this.gameObject.activeSelf!=isShow)NGUITools.SetActiveSelf(this.gameObject,isShow);
        if(joystick.gameObject!=false)NGUITools.SetActive(joystick.gameObject,false);
		isPressed = false;
	}

	public Transform joystick;
	public Transform center;
	/// <summary>
	/// If true, generate message OnJoystickRotate(float rot).
	/// rot = (left) -Pi .. 0 (forward) .. Pi (right)
	/// </summary>

	float joystickRadius = 0f;
	Plane plane;
	int cntFrame;
	int cntFramePressed = 0;
	bool isPressed = false;
	//Vector3 lastPos = Vector3.zero;
	Vector3 prevPos = Vector3.zero;
	Transform mTrans;
    private Vector2 preVDelata=Vector2.zero;

   
    float prevDist=0.0f;
    public void OnJoystick(Vector2 delta){
        
        if(delta.x>1.0f)delta.x=1.0f;
        if(delta.y>1.0f)delta.y=1.0f;
        if(delta.y<-1.0f)delta.y=-1.0f;
        if(delta.x<-1.0f)delta.x=-1.0f;
		Debug.Log("OnJoyStick "+delta.ToString());
		preVDelata=delta;
    }
		
	public TweenAlpha ta;

	void Awake()
	{
		mTrans = transform;
	}
	
	
	void Start()
	{
		// Create the plane to drag along
		plane = new Plane(transform.forward, transform.position);
		
		if (joystick == null)
		{
			joystick = transform.FindChild("Joystick");
			if (joystick == null)
				Debug.LogWarning("Child object Joystick is not found.");
			else if (center == null)
			{
				center = joystick.FindChild("Center");
				if (center == null)
					Debug.LogWarning("Child object Center is not found.");
			}
		}
		
		if (joystick != null)
		{
			joystickRadius = ((SphereCollider) joystick.GetComponent<Collider>()).radius;
			joystick.GetComponent<Collider>().enabled = false;	// need only for radius
			NGUITools.SetActive(joystick.gameObject,false);
		}
	}

	void LateUpdate()
	{
		if (isPressed && cntFramePressed < cntFrame)
		{
			SendMessageOnJoystick(prevPos);
			cntFramePressed = cntFrame;
		}
	}


	/// <summary>
	/// Press and show the joystick
	/// </summary>
	void OnPress(bool pressed)
	{
		if (joystick != null)
		{
			if (pressed && !isPressed)
			{
					prevPos = Vector3.zero;

					CalcPositionJoystick();
					// Show joystick
					NGUITools.SetActive(joystick.gameObject,true);
					ta.ResetToBeginning();
					ta.Play();
					center.localPosition = Vector3.zero;
						isPressed = true;

			}
			else if (pressed && isPressed)
			{
				CalcPositionCenter();
			}
			else
			{
                OnJoystickOff();
				CenterToDefault();
				NGUITools.SetActive(joystick.gameObject,false);
				isPressed = false;
			}
		}
	}
    public void OnJoystickOff(){
        
        Debug.Log("OnJoystickOff");
    }
	/// <summary>
	/// Drag the center
	/// </summary>
    /// 
    void OnDrag(Vector2 delta)
	{
		prevPos = delta;

		if (center != null)
			CalcPositionCenter();
	}
	
    Ray ray;
    Vector3 newPos1;
	void CalcPositionJoystick()
	{
		ray = UICamera.currentCamera.ScreenPointToRay (UICamera.lastTouchPosition);
		float dist = 0f;
		newPos1 = joystick.position;
		
		if (plane.Raycast(ray, out dist))
			newPos1 = ray.GetPoint(dist);
		
		joystick.localPosition = mTrans.InverseTransformPoint(newPos1);
	}
	

	void CalcPositionCenter()
	{
		ray = UICamera.currentCamera.ScreenPointToRay(UICamera.lastTouchPosition);
		float dist = 0f;
		newPos1 = center.position;
		
		if (plane.Raycast(ray, out dist))
			newPos1 = ray.GetPoint(dist);
		
		newPos1 = joystick.InverseTransformPoint(newPos1);
		
		if (newPos1.magnitude > joystickRadius)
			newPos1 = newPos1.normalized * joystickRadius;
		
		center.localPosition = newPos1;
		
		SendMessageOnJoystick(newPos1);
	}
	
	void CenterToDefault(){
		center.localPosition = Vector3.zero;
		SendMessageOnJoystick(center.localPosition);
	}
	/// <summary>
	/// Joystick event.
	/// Return Vector2, x - rotation (-Pi..Pi), y - radius (0..1)
	/// </summary>
	void SendMessageOnJoystick(Vector3 newPos)
	{
		float x = newPos.x / joystickRadius;
		float y = newPos.y / joystickRadius;
		Vector2 delta = new Vector2(x > 1f ? 1f : x, y > 1f ? 1f : y);
		OnJoystick(delta);

		prevPos = newPos;
	}

	/// <summary>
	/// Converting between Cartesian coordinates and polar,
	/// return angle in rad
	/// </summary>
	float Polar(float x, float y)
	{
		float f = 0f;	// if (x == 0 && y == 0)
		if (x > 0)
			f = Mathf.Atan (y / x);
		else if (x < 0 && y >= 0)
			f = Mathf.Atan (y / x) + Mathf.PI;
		else if (x < 0 && y < 0)
			f = Mathf.Atan (y / x) - Mathf.PI;
		else if (x == 0 && y > 0)
			f = Mathf.PI / 2f;
		else if (x == 0 && y < 0)
			f = -Mathf.PI / 2f;
		
		return f;
	}


    void FixedUpdate(){
        cntFrame++;
      
    }



}
