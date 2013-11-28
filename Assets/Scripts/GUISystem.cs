using UnityEngine;
using System.Collections;

public class GUISystem : MonoBehaviour {

    private static GUISystem s_instance = null;

    public static GUISystem instance {
        get {
            if (s_instance == null) {
                GameObject go = new GameObject("GUISystem");
                s_instance = go.AddComponent<GUISystem>();
            }
            return s_instance;
        }
    }

    private delegate void GUIFunction();
    private GUIFunction currentGUI;

    void Start(){
        currentGUI = TestGUI;
    }

    void OnGUI(){
        currentGUI();
    }

    void TestGUI(){
        GUILayout.Label("Score: " + GameManager.instance.score);
    }
}
