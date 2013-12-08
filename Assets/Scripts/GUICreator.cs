using UnityEngine;
using System.Collections;

public class GUICreator : MonoBehaviour {
	// Use this for initialization
	void Start () {
        GUISystem.instance.OnGameStart();
        Destroy(gameObject);

        GameObject kongAPIObject = GameObject.FindGameObjectWithTag("KongAPI");
        if (kongAPIObject == null) {
            GameObject newKongAPIObject = new GameObject();
            newKongAPIObject.name = "KongAPI";
            newKongAPIObject.tag = "KongAPI";
            newKongAPIObject.AddComponent<KongregateAPI>();
        }

	}
}
