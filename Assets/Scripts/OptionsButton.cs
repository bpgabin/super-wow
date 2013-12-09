using UnityEngine;
using System.Collections;

public class OptionsMenuLoad : BaseEvent { }

public class OptionsButton : MonoBehaviour {
    void OnMouseDown() {
        EventManager.instance.QueueEvent(new OptionsMenuLoad());
        audio.Play();
    }
}
