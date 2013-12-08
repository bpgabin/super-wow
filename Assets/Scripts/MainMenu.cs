using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

    public GameObject start;
    public GameObject options;
    public GameObject instructions;
    public GameObject credits;

	// Update is called once per frame
	void Update () {
        if (animation.isPlaying && Input.anyKeyDown) {
            float time = animation["Title"].normalizedTime;
            animation.Stop();
            animation["Title"].speed = 10.0f;
            animation["Title"].normalizedTime = time;
            animation.Play();
        }

        if (Input.GetMouseButton(0)) {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null) {
                if (hit.transform.gameObject == start) {
                    Application.LoadLevel("Prototype");
                }
            }
        }
	}
}
