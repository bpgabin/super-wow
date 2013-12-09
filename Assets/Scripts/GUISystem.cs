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
    public bool captionPlaying = false;

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
        return false;
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
        GUILayout.BeginArea(new Rect(Screen.width / 2.0f - 100, Screen.height / 2.0f - 250, 200, 200));
        GUILayout.Box("SUCH PAUSE");
        if (GUILayout.Button("SO RESUMING")) {
            OnGameUnPaused();
            GameManager.instance.OnGameUnPaused();
        }
        if (GUILayout.Button("MUCH QUIT")) {
            Time.timeScale = 1.0f;
            Application.LoadLevel("Menu");
            HOTween.Kill();
        }
        GUILayout.EndArea();
    }

    public void OnRoundOver() {
        currentGUI = StageCompleteGUI;
    }

    public void OnNextRound() {
        currentGUI = TestGUI;
    }

    void StageCompleteGUI() {
        /*
        GUILayout.BeginArea(new Rect(Screen.width / 2.0f - 100, Screen.height / 2.0f - 100, 200, 200));
        if (GUILayout.Button("Next Round")) {
            currentGUI = TestGUI;
            GameManager.instance.BeginNewRound();
        }
        GUILayout.EndArea();
         */
    }

    void GameOverGUI() {
        GUILayout.BeginArea(new Rect(Screen.width / 2.0f - 200, Screen.height / 2.0f - 150, 400, 500));
        GUILayout.Box("GAME OVER\n\n\nScore: " + GameManager.instance.score + "\nRound: " + GameManager.instance.round);
        if (GUILayout.Button("SUCH RESTARTING")) {
            Time.timeScale = 1.0f;
            Application.LoadLevel("Prototype");
			HOTween.Kill();
	    }
        if (GUILayout.Button("MUCH QUIT")) {
            Time.timeScale = 1.0f;
            Application.LoadLevel("Menu");
            HOTween.Kill();
        }
        GUILayout.EndArea();
    }

    public string caption = "";

    public bool OnAudioCaption(IEvent evt) {
        CaptionNeeded cptNed = evt as CaptionNeeded;
        HOTween.To(this, 1.0f, "caption", cptNed.caption);
        captionPlaying = true;
        Invoke("CaptionComplete", 3.0f);
        return true;
    }

    void CaptionComplete() {
        caption = "";
        captionPlaying = false;
    }

    void TestGUI(){
        GUI.skin.font = Resources.Load("ComicSans") as Font;
        GUILayout.BeginArea(new Rect(Screen.width / 2.0f - 80, Screen.height / 2.0f - 30, 160, 200));
        GUILayout.Box(GameManager.instance.score.ToString());
        //GUILayout.Label("Stage: " + GameManager.instance.round);
        //GUILayout.Label("EarthHP: " + GameManager.instance.earthHP);
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(Screen.width / 2.0f - 300, Screen.height - 50, 600, 50));
        GUILayout.Box(caption);
        GUILayout.EndArea();
    }
}
