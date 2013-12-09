using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class NextRoundButton : MonoBehaviour, IEventListener {

    public bool HandleEvent(IEvent evt) { return false; }

    void Start() {
        EventManager.instance.AddListener(this, "GameRoundEnd", this.OnRoundEnd);
    }

    public bool OnRoundEnd(IEvent evt) {
        StartCoroutine("RoundTransition");
        return false;
    }

    void OnGUI() {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        GUILayout.BeginArea(new Rect(screenPos.x - 200, screenPos.y + 40, 400, 500));
        GUILayout.Label("Round " + GameManager.instance.round +  " Compleet: Bonus Poinst");
        GUILayout.Label("Remining HP: " + GameManager.instance.earthHP + " X 100 = " + GameManager.instance.earthHP * 100);
        GUILayout.Label("Missuls: " + GameManager.instance.remainingMissiles() + " X 5 = " + GameManager.instance.remainingMissiles() * 5);
        if (GameManager.instance.scoreThreshold >= 10000) {
            GUILayout.Label("O WOW HP SUCH GAINED");
        }
        GUILayout.EndArea();
    }

    IEnumerator RoundTransition() {
        animation.Play("nextRoundAnim");
        yield return new WaitForSeconds(3.5f);
        animation.Play("nextRoundLeave");
        yield return new WaitForSeconds(1.5f);
        GameManager.instance.BeginNewRound();
        GUISystem.instance.OnNextRound();
    }
}
