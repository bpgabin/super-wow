using UnityEngine;
using System.Collections;

public class InstructionsMenuLoad : BaseEvent { }

public class InstructionsButton : MonoBehaviour {
    void OnMouseDown() {
        EventManager.instance.QueueEvent(new InstructionsMenuLoad());
        audio.Play();
    }
}
