using UnityEngine;
using System.Collections;

public class Razer : MonoBehaviour {

	public GameObject particle;

	public LineRenderer line;
	shipControl ship;
	void Start () 
	{
		size = new Vector2 (1.0f / uvTieX ,1.0f / uvTieY);
		myRenderer = GetComponent<LineRenderer>();
		if(myRenderer == null)
			enabled = false;
	}
	bool isOn=false;
	public void ShowLine(Transform target,shipControl ship){
		this.ship=ship;
		isOn=true;
		particle.SetActive(true);
		myRenderer.enabled=true;
	}

	public void HideLine(){
		isOn=false;
		particle.SetActive(false);
		myRenderer.enabled=false;

	}

	int uvTieX = 7;
	int uvTieY = 3;
	public int fps = 10;
	private Vector2 size;

	private LineRenderer myRenderer;
	private int lastIndex = -1;


	// Update is called once per frame
	void Update()
	{
		if(!isOn)return;

		if(ship==null|| ship.razerTarget==null){
			if(myRenderer.enabled)myRenderer.enabled=false;
			return;
		}

		if(!myRenderer.enabled)myRenderer.enabled=true;

		line.SetPosition(0,transform.position);
		line.SetPosition(1,ship.razerTarget.transform.position);

		// Calculate index
		int index = (int)(Time.timeSinceLevelLoad * fps) %  (uvTieX * uvTieY);
		if(index != lastIndex)
		{
			// split into horizontal and vertical index
			int uIndex = index % uvTieX;
			int vIndex = index / uvTieY;
			// build offset
			// v coordinate is the bottom of the image in opengl so we need to invert.
			Vector2 offset = new Vector2 (uIndex * size.x, 1.0f - size.y - vIndex * size.y);
		
			myRenderer.material.SetTextureOffset ("_MainTex", offset);
			myRenderer.material.SetTextureScale ("_MainTex", size);

			lastIndex = index;
		}
	}
}
