using UnityEngine;
using System.Collections;

public class CreditsMenuLoad : BaseEvent { }

public class CreditsButton : MonoBehaviour {
    void OnMouseDown() {
        EventManager.instance.QueueEvent(new CreditsMenuLoad());
        audio.Play();
    }
}
