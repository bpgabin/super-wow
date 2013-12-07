using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class GUISystem : MonoBehaviour, IEventListener {

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

    public bool HandleEvent(IEvent evt) { return false; }

    void Start(){
        EventManager.instance.AddListener(this, "GameOver", OnGameOver);

        currentGUI = TestGUI;
    }

    public bool OnGameOver(IEvent evt) {
        currentGUI = GameOverGUI;
        return true;
    }

    void OnGUI(){
        currentGUI();
    }

    void GameOverGUI() {
        GUILayout.Box("GAME OVER\n\n\nScore: " + GameManager.instance.score + "\nRound: " + GameManager.instance.round);
        if (GUILayout.Button("Restart")) {
            Application.LoadLevel("Prototype");
			HOTween.Kill();
	        }
    }

    void TestGUI(){
        GUILayout.Label("Score: " + GameManager.instance.score);
        GUILayout.Label("Stage: " + GameManager.instance.round);
    }
}
