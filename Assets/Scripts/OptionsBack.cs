using UnityEngine;
using System.Collections;

public class OptionsBackPressed : BaseEvent { }

public class OptionsBack : MonoBehaviour {
    void OnMouseDown() {
        EventManager.instance.QueueEvent(new OptionsBackPressed());
    }

    void OnGUI() {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        GUILayout.BeginArea(new Rect(screenPos.x - 200, screenPos.y + 180, 400, 100));
        GUILayout.Box("Global Volume: " + string.Format("{0:0.00}", AudioListener.volume));
        AudioListener.volume = GUILayout.HorizontalSlider(AudioListener.volume, 0.0f, 1.0f);
        GUILayout.EndArea();
    }
}
