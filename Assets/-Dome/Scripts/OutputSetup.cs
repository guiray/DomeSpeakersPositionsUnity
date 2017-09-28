using UnityEngine;
using System.Collections;

public enum OutputConfig {dome_180, dome_210, dome_230, dome_18015, dome_18025};

[ExecuteInEditMode]
public class OutputSetup : MonoBehaviour {
	
	public OutputConfig config;
	public int stitcherLayer;
	public Camera stitcherCam_One;

	// Stitchers
	public GameObject stitcher180_back;
	public GameObject stitcher180_front;
	public GameObject stitcher180_One;
	public GameObject stitcher210_back;
	public GameObject stitcher210_front;
	public GameObject stitcher210_One;
	public GameObject stitcher230_back;
	public GameObject stitcher230_front;
	public GameObject stitcher230_One;
	public GameObject camrig;
	
	private OutputConfig currentConfig;
	
	void Start(){

		float stitcherScaling = 1f;
		Vector3 stitcherBackPos = Vector3.zero;
		Vector3 stitcherFrontPos = Vector3.zero;
		Vector3 stitcherOnePos = Vector3.zero;

		stitcherCam_One.rect = new Rect(0f, 0f, 1f, 1f);
		stitcherCam_One.enabled=true;
		stitcherScaling = stitcherCam_One.orthographicSize *(Screen.width / Screen.height);
		stitcherBackPos = new Vector3(0f, 0f, 0.12f);
		stitcherFrontPos = new Vector3(0f, 1f, 0.12f);
		stitcherOnePos = new Vector3(0f, 0f, 0.12f);

		
		// Scale and position stitchers to fit screen
		stitcher180_back.transform.localScale = new Vector3(stitcherScaling, stitcherScaling, stitcherScaling);
		stitcher180_front.transform.localScale = new Vector3(stitcherScaling, stitcherScaling, stitcherScaling);
		stitcher180_One.transform.localScale = new Vector3(stitcherScaling, stitcherScaling, stitcherScaling);
		stitcher180_back.transform.localPosition = stitcherBackPos;
		stitcher180_front.transform.localPosition = stitcherFrontPos;
		stitcher180_One.transform.localPosition = stitcherOnePos;
		stitcher210_back.transform.localScale = new Vector3(stitcherScaling, stitcherScaling, stitcherScaling);
		stitcher210_front.transform.localScale = new Vector3(stitcherScaling, stitcherScaling, stitcherScaling);
		stitcher210_One.transform.localScale = new Vector3(stitcherScaling, stitcherScaling, stitcherScaling);
		stitcher210_back.transform.localPosition = stitcherBackPos;
		stitcher210_front.transform.localPosition = stitcherFrontPos;
		stitcher210_One.transform.localPosition = stitcherOnePos;
		stitcher230_back.transform.localScale = new Vector3(stitcherScaling, stitcherScaling, stitcherScaling);
		stitcher230_front.transform.localScale = new Vector3(stitcherScaling, stitcherScaling, stitcherScaling);
		stitcher230_One.transform.localScale = new Vector3(stitcherScaling, stitcherScaling, stitcherScaling);
		stitcher230_back.transform.localPosition = stitcherBackPos;
		stitcher230_front.transform.localPosition = stitcherFrontPos;
		stitcher230_One.transform.localPosition = stitcherOnePos;
			
		//Set stitchers culling
		stitcher180_back.layer = stitcherLayer;
		stitcher180_front.layer = stitcherLayer;
		stitcher180_One.layer = stitcherLayer;
		stitcher210_back.layer = stitcherLayer;
		stitcher210_front.layer = stitcherLayer;
		stitcher210_One.layer = stitcherLayer;
		stitcher230_back.layer = stitcherLayer;
		stitcher230_front.layer = stitcherLayer;
		stitcher230_One.layer = stitcherLayer;

		stitcherCam_One.cullingMask = 1 << stitcherLayer;
		
		SwitchConfig(config);
	}

	void Update () {
		
		if (Input.GetKeyUp (KeyCode.Keypad1) || Input.GetKeyUp (KeyCode.Alpha1)) {
			config = (OutputConfig)0;
		}
		
		if (Input.GetKeyUp (KeyCode.Keypad2) || Input.GetKeyUp (KeyCode.Alpha2)) {
			config = (OutputConfig)1;
		}
		
		if (Input.GetKeyUp (KeyCode.Keypad3) || Input.GetKeyUp (KeyCode.Alpha3)) {
			config = (OutputConfig)2;
		}
		
		if (Input.GetKeyUp (KeyCode.Keypad4) || Input.GetKeyUp (KeyCode.Alpha4)) {
			config = (OutputConfig)3;
		}
		
		if (Input.GetKeyUp (KeyCode.Keypad5) || Input.GetKeyUp (KeyCode.Alpha5)) {
			config = (OutputConfig)4;
		}

		if(config != currentConfig){
			SwitchConfig(config);
		}
		
	}
	
	//Switch between 180 210 and 230°
	void SwitchConfig(OutputConfig c){
		
		switch(c){
			case OutputConfig.dome_180:
				SetActiveRecursive(stitcher180_back,true);
				SetActiveRecursive(stitcher180_front,true);
				SetActiveRecursive(stitcher180_One,true);
				SetActiveRecursive(stitcher210_back,false);
				SetActiveRecursive(stitcher210_front,false);
				SetActiveRecursive(stitcher210_One,false);
				SetActiveRecursive(stitcher230_back,false);
				SetActiveRecursive(stitcher230_front,false);
				SetActiveRecursive(stitcher230_One,false);
			break;
				
			case OutputConfig.dome_210:
				SetActiveRecursive(stitcher180_back,false);
				SetActiveRecursive(stitcher180_front,false);
				SetActiveRecursive(stitcher180_One,false);
				SetActiveRecursive(stitcher210_back,true);
				SetActiveRecursive(stitcher210_front,true);
				SetActiveRecursive(stitcher210_One,true);
				SetActiveRecursive(stitcher230_back,false);
				SetActiveRecursive(stitcher230_front,false);
				SetActiveRecursive(stitcher230_One,false);
			break;
				
			case OutputConfig.dome_230:
				SetActiveRecursive(stitcher180_back,false);
				SetActiveRecursive(stitcher180_front,false);
				SetActiveRecursive(stitcher180_One,false);
				SetActiveRecursive(stitcher210_back,false);
				SetActiveRecursive(stitcher210_front,false);
				SetActiveRecursive(stitcher210_One,false);
				SetActiveRecursive(stitcher230_back,true);
				SetActiveRecursive(stitcher230_front,true);
				SetActiveRecursive(stitcher230_One,true);
			break;
			
			case OutputConfig.dome_18015:
				SetActiveRecursive(stitcher180_back,true);
				SetActiveRecursive(stitcher180_front,true);
				SetActiveRecursive(stitcher180_One,true);
				SetActiveRecursive(stitcher210_back,false);
				SetActiveRecursive(stitcher210_front,false);
				SetActiveRecursive(stitcher210_One,false);
				SetActiveRecursive(stitcher230_back,false);
				SetActiveRecursive(stitcher230_front,false);
				SetActiveRecursive(stitcher230_One,false);
			break;
			
			case OutputConfig.dome_18025:
				SetActiveRecursive(stitcher180_back,true);
				SetActiveRecursive(stitcher180_front,true);
				SetActiveRecursive(stitcher180_One,true);
				SetActiveRecursive(stitcher210_back,false);
				SetActiveRecursive(stitcher210_front,false);
				SetActiveRecursive(stitcher210_One,false);
				SetActiveRecursive(stitcher230_back,false);
				SetActiveRecursive(stitcher230_front,false);
				SetActiveRecursive(stitcher230_One,false);
			break;
		}

		currentConfig = c;
	}

	void SetActiveRecursive(GameObject go, bool active){
		go.SetActive (active);
		foreach(Transform t in go.transform){
			SetActiveRecursive(t.gameObject, active);
		}
	}

}