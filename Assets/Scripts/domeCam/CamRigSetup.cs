using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRigSetup : MonoBehaviour {

	public enum type{SideBySide, Cross, Dome};
	public type _Type;
	
	public enum rtr{_512, _1024, _2048, _3072, _4096};
	public rtr _TextureResolution;

	RenderTexture[] _rtArray = new RenderTexture[6];
	string[] _camName = new string[] {"left","front","right","back","top","bottom"}; 
	GameObject camContainer;

	// Use this for initialization
	void Start () {

		switch (_Type) {
		case type.SideBySide:
			SideBySide ();
			break;
		case type.Cross:
			Cross();
			break;
		case type.Dome:
			Dome(180f);
			break;
		}
			
	}
		

	void CreateCamereRig(){
		
		camContainer = new GameObject ("CameraRig");
		camContainer.transform.position = Vector3.zero;

		Vector3[] camOrientation = new Vector3[] { 
			new Vector3(0f,-90f,0f),
			new Vector3(0f,0f,0f),
			new Vector3(0f,90f,0f),
			new Vector3(0f,180f,0f),
			new Vector3(-90f,0f,0f),
			new Vector3(90f,0f,0f)
		};

		for(int i=0; i < _camName.Length; i++){

			int res = 0;

			switch (_TextureResolution) {
				case rtr._512:
					res = 512;
					break;
				case rtr._1024:
					res = 1024;
					break;
				case rtr._2048:
					res = 2048;
					break;
				case rtr._3072:
					res = 3072;
					break;
				case rtr._4096:
					res = 4096;
					break;
				default:
					res = 1024;
				break;
			}

			RenderTexture rt = new RenderTexture (res, res, 16, RenderTextureFormat.ARGB32);
			rt.name = _camName [i]+"-rt";
			_rtArray [i] = rt;

			GameObject cam = new GameObject (_camName [i]);
			cam.transform.position = Vector3.zero;
			cam.transform.rotation = Quaternion.Euler (camOrientation [i]);
			cam.AddComponent<Camera> ();
			cam.GetComponent<Camera> ().targetTexture = rt;
			cam.GetComponent<Camera> ().fieldOfView = 90f;
			cam.transform.parent = camContainer.transform;
		}
	}


	public void Cross(){
		CreateCamereRig ();
		GameObject container = new GameObject ("Cross");

		GameObject cam = new GameObject ("output");
		cam.transform.position = new Vector3(15f, 0.5f, 0f);
		cam.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
		cam.AddComponent<Camera> ();
		cam.GetComponent<Camera> ().clearFlags = CameraClearFlags.SolidColor;
		cam.GetComponent<Camera> ().backgroundColor = new Color (0f, 0f, 0f, 1f);
		cam.GetComponent<Camera> ().orthographic = true;
		cam.GetComponent<Camera> ().orthographicSize = 15;
		cam.GetComponent<Camera>().rect = new Rect(0f, 0f, 0.89f, 1f);
		cam.GetComponent<Camera> ().cullingMask = 1 << 8;

		cam.transform.parent = container.transform;

		Vector3[] pos = new Vector3[] {
			new Vector3(20f,0f,0f),
			new Vector3(10f,0f,0f),
			new Vector3(0f,0f,0f),
			new Vector3(30f,0f,0f),
			new Vector3(10f,0f,-10f),
			new Vector3(10f,0f,10f)
		};

		for (int i = 0; i < _camName.Length; i++) {
			GameObject cross = GameObject.CreatePrimitive (PrimitiveType.Plane);
			cross.name = _camName [i];
			cross.transform.position = pos [i];
			cross.layer = 8;

			Renderer rend = cross.GetComponent<Renderer>();
			rend.material = new Material(Shader.Find("Unlit/Texture"));
			rend.material.SetTexture ("_MainTex", _rtArray [i]);

			cross.transform.parent = container.transform;
		}
	}

	public void SideBySide(){
		CreateCamereRig ();
		GameObject container = new GameObject ("SideBySide");

		GameObject cam = new GameObject ("output");
		cam.transform.position = new Vector3 (10f, 0.5f, 5f);
		cam.transform.rotation = Quaternion.Euler (90f, 0f, 0f);
		cam.AddComponent<Camera> ();
		cam.GetComponent<Camera> ().clearFlags = CameraClearFlags.SolidColor;
		cam.GetComponent<Camera> ().backgroundColor = new Color (0f, 0f, 0f, 1f);
		cam.GetComponent<Camera> ().orthographic = true;
		cam.GetComponent<Camera> ().orthographicSize = 10;
		cam.GetComponent<Camera>().rect = new Rect(0f, 0f, 1f, 1f);
		cam.GetComponent<Camera> ().cullingMask = 1 << 8;

		cam.transform.parent = container.transform;

		Vector3[] pos = new Vector3[] {
			new Vector3(0f,0f,0f),
			new Vector3(10f,0f,0f),
			new Vector3(20f,0f,0f),
			new Vector3(0f,0f,10f),
			new Vector3(10f,0f,10f),
			new Vector3(20f,0f,10f)
		};

		for (int i = 0; i < _camName.Length; i++) {
			GameObject cross = GameObject.CreatePrimitive (PrimitiveType.Plane);
			cross.name = _camName [i];
			cross.transform.position = pos [i];
			cross.layer = 8;

			Renderer rend = cross.GetComponent<Renderer>();
			rend.material = new Material(Shader.Find("Unlit/Texture"));
			rend.material.SetTexture ("_MainTex", _rtArray [i]);

			cross.transform.parent = container.transform;
		}

	}

	public void Dome(float degree){

		GameObject container = new GameObject ("Dome");


		GameObject camWorld = new GameObject ("camWorld");
		camWorld.AddComponent<Camera> ();
		camWorld.GetComponent<Camera> ().cullingMask = 1;
		camWorld.GetComponent<Camera> ().allowHDR = false;


		GameObject camProjection = new GameObject("camProjection");
		camProjection.AddComponent<Camera> ();
		camProjection.GetComponent<Camera> ().clearFlags = CameraClearFlags.Nothing;
		camProjection.GetComponent<Camera> ().cullingMask = 0 << 0;
		camProjection.GetComponent<Camera> ().allowHDR = false;
		camProjection.GetComponent<Camera> ().depth = 1;
		camProjection.GetComponent<Camera> ().tag = "MainCamera";


		camWorld.transform.parent = container.transform;
		camProjection.transform.parent = container.transform;

		camProjection.AddComponent<DomeProjection> ();
		container.AddComponent<DomeController> ();


	}



}