using UnityEngine;
using System.Collections;

public class GUICreator : MonoBehaviour {
	// Use this for initialization
	void Start () {
        GUISystem.instance.OnGameStart();
        KongregateAPI.instance.InitialStart();
        Destroy(gameObject);
	}
}
