using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManagerScript : MonoBehaviour, IEventListener {

    public AudioSource noAmmo;
    public AudioSource suchDestroyed;
    public AudioSource stationDown;
    public AudioSource idle1;
    public AudioSource idle2;
    public AudioSource idle3;
    public AudioSource idle4;
    private int num = 0;
	private int randNumSide;
    private string[] captions;
	private AudioSource[] chatter;
	private Queue<int> soundQueue;

    public bool HandleEvent(IEvent evt) { return false; }

	// Use this for initialization
	void Awake () {
        EventManager.instance.AddListener(this, "StationDestroyed", OnStationDestroyed);

		soundQueue = new Queue<int>();

        captions = new string[7];
        captions[0] = "SUCH LAUNCH - SO CUNFIRMED";
		captions[1] = "WOW - SO INCOMYNG";
        captions[2] = "SUCH DEFENS";
        captions[3] = "SUPER WOW";
        captions[4] = "TARGHET SUCH DESTROYD";
        captions[5] = "WOW - SO IMPACT, SUCH HURTING";
        captions[6] = "NO BOOM ROKITS, SO SADNESS";

		chatter = new AudioSource[7];
		chatter[0] = idle1;
		chatter[1] = idle2;
		chatter[2] = idle3;
		chatter[3] = idle4;
		chatter[4] = suchDestroyed;
		chatter[5] = stationDown;
		chatter[6] = noAmmo;


        StartCoroutine("PlayASound");
	}
	
	// Update is called once per frame
	void Update () {

		if (soundQueue.Count > 0) 
		{
			int dequeued = soundQueue.Dequeue ();
			chatter[dequeued].Play();
			EventManager.instance.QueueEvent(new CaptionNeeded(captions[dequeued]));
		}
	}

    // We need to have a queue for the sounds
    // We also might need more sounds in: Round Begin "So Start Such Redy"


    public bool OnStationDestroyed(IEvent evt) {
		soundQueue.Enqueue(5);
        return true;
    }

	public bool OnAmmoEmpty(IEvent evt) {
		soundQueue.Enqueue (6);
		return true;
	}

	// Every time score goes to 1000;
	public bool OnScore(IEvent evt) {
		soundQueue.Enqueue (4);
		return true;
	}

    IEnumerator PlayASound() {
        while (true) {
			randNumSide = Random.Range(0, 4);
			soundQueue.Enqueue(randNumSide);
			yield return new WaitForSeconds(4.0f);
        }
    }
}
