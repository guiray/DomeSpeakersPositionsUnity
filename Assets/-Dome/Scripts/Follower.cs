using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Follower : MonoBehaviour {
public GameObject objectToFollow;
public Vector3 positionFromObject;
public Vector3 orientationFromObject;
	
	
	// Update is called once per frame
	void LateUpdate () {
		if (objectToFollow != null) {
			transform.position = objectToFollow.transform.position + positionFromObject;
			transform.rotation = Quaternion.Euler (orientationFromObject);
		} else {
			Debug.LogWarning ("You need to assign an Object To Follow to use position and orientation");
		}
	}
}
