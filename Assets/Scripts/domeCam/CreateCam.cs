using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateCam {

	[MenuItem("GameObject/Camera Cubemap Side by Side %g")]
	private static void sidebyside(){
		GameObject gs = GameObject.Find ("Scripts");
		gs.GetComponent<CamRigSetup> ().SideBySide ();
	}

	[MenuItem("GameObject/Camera Cubemap Cross ")]
	private static void cross(){
		GameObject gs = GameObject.Find ("Scripts");
		gs.GetComponent<CamRigSetup> ().Cross ();
	}

	[MenuItem("GameObject/Camera Cubemap Dome ")]
	private static void dome(){
		GameObject gs = GameObject.Find ("Scripts");
		gs.GetComponent<CamRigSetup> ().Dome (180f);
	}
	
}