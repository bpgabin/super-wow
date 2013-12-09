using UnityEngine;
using System.Collections;

public class AnimationLooper : MonoBehaviour {
	// Update is called once per frame
	void Update () {
        if (!animation.isPlaying) animation.Play();
	}
}
