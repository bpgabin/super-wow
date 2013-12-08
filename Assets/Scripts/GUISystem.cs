using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class CaptionNeeded : BaseEvent {
    public string caption;
    public CaptionNeeded(string caption) {
        this.caption = caption;
    }
}

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

    public void OnGameStart() {
        currentGUI = MainMenuGUI;
    }

    public bool HandleEvent(IEvent evt) { return false; }

    void Start(){
        EventManager.instance.AddListener(this, "GameOver", OnGameOver);
        EventManager.instance.AddListener(this, "CaptionNeeded", OnAudioCaption);
    }

    public bool OnGameOver(IEvent evt) {
        currentGUI = GameOverGUI;
        return true;
    }

    public void OnGamePaused() {
        currentGUI = PausedGUI;
    }

    public void OnGameUnPaused() {
        currentGUI = TestGUI;
    }

    public void OnGameLoaded() {
        currentGUI = TestGUI;
    }

    void OnGUI(){
        currentGUI();
    }

    void MainMenuGUI() {
    }

    void PausedGUI() {
        GUILayout.BeginArea(new Rect(Screen.width / 2.0f - 100, Screen.height / 2.0f - 100, 200, 200));
        GUILayout.Box("SUCH PAUSE");
        if (GUILayout.Button("SO RESUMING")) {
            OnGameUnPaused();
            GameManager.instance.OnGameUnPaused();
        }
        GUILayout.EndArea();
    }

    public void OnRoundOver() {
        currentGUI = StageCompleteGUI;
    }

    void StageCompleteGUI() {
        GUILayout.BeginArea(new Rect(Screen.width / 2.0f - 100, Screen.height / 2.0f - 100, 200, 200));
        if (GUILayout.Button("Next Round")) {
            currentGUI = TestGUI;
            GameManager.instance.BeginNewRound();
        }
        GUILayout.EndArea();
    }

    void GameOverGUI() {
        GUILayout.Box("GAME OVER\n\n\nScore: " + GameManager.instance.score + "\nRound: " + GameManager.instance.round);
        if (GUILayout.Button("Restart")) {
            Application.LoadLevel("Prototype");
			HOTween.Kill();
	    }
    }

    public string caption = "";

    public bool OnAudioCaption(IEvent evt) {
        CaptionNeeded cptNed = evt as CaptionNeeded;
        HOTween.To(this, 1.0f, "caption", cptNed.caption);
        Invoke("CaptionComplete", 3.0f);
        return true;
    }

    void CaptionComplete() {
        caption = "";
    }

    void TestGUI(){
        GUI.skin.font = Resources.Load("ComicSans") as Font;
        GUILayout.Label("Score: " + GameManager.instance.score);
        GUILayout.Label("Stage: " + GameManager.instance.round);
        GUILayout.Label("EarthHP: " + GameManager.instance.earthHP);

        GUILayout.BeginArea(new Rect(Screen.width / 2.0f - 200, Screen.height - 50, 400, 50));
        GUILayout.Box(caption);
        GUILayout.EndArea();
    }
}
