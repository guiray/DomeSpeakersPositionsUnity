using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubemapRender : MonoBehaviour {

	public GameObject _cam;
	public Texture2D _t;
	public Cubemap _c;
	CubemapFace _cf;
	public Material _m;



	// Use this for initialization
	void Start () {

		Texture2D t = new Texture2D (1024, 1024);

		_cam.GetComponent<Camera> ().RenderToCubemap(_c);
		Color[] c = _c.GetPixels (CubemapFace.PositiveX);
		t.SetPixels (c);
		t.Apply ();

		_m.SetTexture ("Main_Tex", t);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
