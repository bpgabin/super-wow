using UnityEngine;
using System.Collections;

public class CreditsBackPressed : BaseEvent { }

public class CreditsBack : MonoBehaviour {
    void OnMouseDown() {
        EventManager.instance.QueueEvent(new CreditsBackPressed());
    }
}
