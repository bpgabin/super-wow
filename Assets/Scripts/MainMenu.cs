using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class MainMenu : MonoBehaviour, IEventListener {

    public Transform start;
    public Transform options;
    public Transform instructions;
    public Transform credits;

    public Transform creditsPage;
    public Transform instructionsPage;
    public Transform optionsPage;

    public AudioSource supWow;
    public AudioSource suchDef;

    public bool HandleEvent(IEvent evt) { return false; }

    void Start() {
        EventManager.instance.AddListener(this, "CreditsMenuLoad", this.OnCreditsLoad);
        EventManager.instance.AddListener(this, "OptionsMenuLoad", this.OnOptionsLoad);
        EventManager.instance.AddListener(this, "InstructionsMenuLoad", this.OnInstructionsLoad);
        EventManager.instance.AddListener(this, "CreditsBackPressed", this.OnCreditsBack);
        EventManager.instance.AddListener(this, "OptionsBackPressed", this.OnOptionsBack);
        EventManager.instance.AddListener(this, "InstructionsBackPressed", this.OnInstructionsBack);
    }

	// Update is called once per frame
	void Update () {
        if (animation.isPlaying && Input.anyKeyDown) {
            float time = animation["Title"].normalizedTime;
            animation.Stop();
            animation["Title"].speed = 10.0f;
            animation["Title"].normalizedTime = time;
            animation.Play();
        }
	}

    public void PlayClip1() {
        supWow.Play();
    }

    public void PlayClip2() {
        suchDef.Play();
    }

    public bool OnCreditsLoad(IEvent evt) {
        HOTween.To(transform, 3.0f, "position", new Vector3(20.0f, transform.position.y, transform.position.z));
        HOTween.To(creditsPage, 3.0f, "position", new Vector3(0f, creditsPage.position.y, creditsPage.position.z));
        return true;
    }

    public bool OnOptionsLoad(IEvent evt) {
        HOTween.To(transform, 3.0f, "position", new Vector3(20.0f, transform.position.y, transform.position.z));
        HOTween.To(optionsPage, 3.0f, "position", new Vector3(0f, optionsPage.position.y, optionsPage.position.z));
        return true;
    }

    public bool OnInstructionsLoad(IEvent evt) {
        HOTween.To(transform, 3.0f, "position", new Vector3(20.0f, transform.position.y, transform.position.z));
        HOTween.To(instructionsPage, 3.0f, "position", new Vector3(0f, instructionsPage.position.y, instructionsPage.position.z));
        return true;
    }

    public bool OnCreditsBack(IEvent evt) {
        HOTween.To(transform, 3.0f, "position", new Vector3(0.0f, transform.position.y, transform.position.z));
        HOTween.To(creditsPage, 3.0f, "position", new Vector3(-10f, creditsPage.position.y, creditsPage.position.z));
        return true;
    }

    public bool OnOptionsBack(IEvent evt) {
        HOTween.To(transform, 3.0f, "position", new Vector3(0.0f, transform.position.y, transform.position.z));
        HOTween.To(optionsPage, 3.0f, "position", new Vector3(-10f, optionsPage.position.y, optionsPage.position.z));
        return true;
    }

    public bool OnInstructionsBack(IEvent evt) {
        HOTween.To(transform, 3.0f, "position", new Vector3(0.0f, transform.position.y, transform.position.z));
        HOTween.To(instructionsPage, 3.0f, "position", new Vector3(-10f, instructionsPage.position.y, instructionsPage.position.z));
        return true;
    }
}
