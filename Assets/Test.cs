using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Test :MonoBehaviour {
	
	public string scene;

	public void OnClick(){
		SceneManager.LoadScene(scene);
	}

}
