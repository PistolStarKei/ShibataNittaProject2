using UnityEngine;
using System.Collections;

public class CaptureScreenshot : MonoBehaviour {


	[UnityEditor.MenuItem("Edit/CaptureScreenshot")]

	static void Capture()
	{
		Application.CaptureScreenshot("image.png", 4);
	}
}
