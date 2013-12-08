using UnityEngine;
using System.Collections;

public class SoundManagerScript : MonoBehaviour, IEventListener {

    public AudioSource noAmmo;
    public AudioSource suchDestroyed;
    public AudioSource stationDown;
    public AudioSource idle1;
    public AudioSource idle2;
    public AudioSource idle3;
    public AudioSource idle4;
    private int num = 0;
    private string[] captions;

    public bool HandleEvent(IEvent evt) { return false; }

	// Use this for initialization
	void Start () {
        EventManager.instance.AddListener(this, "StationDestroyed", OnStationDestroyed);

        captions = new string[7];
        captions[0] = "SUCH LAUNCH - SO CUNFIRMED";
        captions[1] = "SUPER WOW";
        captions[2] = "SUCH DEFENS";
        captions[3] = "WOW - SO INCOMYNG";
        captions[4] = "TARGHET SUCH DESTROYD";
        captions[5] = "WOW - SO IMPACT, SUCH HURTING";
        captions[6] = "NO BOOM ROKITS, SO SADNESS";

        StartCoroutine("PlayASound");
	}
	
	// Update is called once per frame
	void Update () {

	}

    // We need to have a queue for the sounds
    // We also might need more sounds in: Round Begin "So Start Such Redy"


    public bool OnStationDestroyed(IEvent evt) {
        suchDestroyed.Play();
        EventManager.instance.QueueEvent(new CaptionNeeded(captions[5]));
        return true;
    }

    IEnumerator PlayASound() {
        while (true) {
            idle1.Play();
            EventManager.instance.QueueEvent(new CaptionNeeded(captions[0]));
            yield return new WaitForSeconds(6.0f);
        }
    }
}
