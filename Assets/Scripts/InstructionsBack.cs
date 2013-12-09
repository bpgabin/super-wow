using UnityEngine;
using System.Collections;

public class InstructionsBackPressed : BaseEvent { }

public class InstructionsBack : MonoBehaviour {
    void OnMouseDown() {
        EventManager.instance.QueueEvent(new InstructionsBackPressed());
    }
}
