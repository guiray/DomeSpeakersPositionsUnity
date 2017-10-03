using System.Collections;
using UnityEngine;

public class SpeakerLookAtCenter : MonoBehaviour {

    private GameObject target;
    private Transform targetPos;

    void Start () {
        target = GameObject.Find("ListenPoint");
        targetPos = target.transform;
        transform.LookAt(target);
    }
}
