using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class Explosion : MonoBehaviour {

    public GameObject explosionPrefab;

    public float heatValue = 1f;

	// Use this for initialization
	void Start () {
        Sequence sequence = new Sequence(new SequenceParms().OnComplete(OnAnimationCompletion));
        sequence.Append(HOTween.From(transform, 2f, "localScale", new Vector3(3f, 3f, 3f)));
        sequence.AppendInterval(1f);
        sequence.Append(HOTween.To(transform, 2f, "localScale", new Vector3(2f, 2f, 2f)));

        Sequence heatSequence = new Sequence(new SequenceParms());
        heatSequence.AppendInterval(3f);
        heatSequence.Append(HOTween.To(this, 2f, "heatValue", 0.5f));

        sequence.Play();
        heatSequence.Play();
	}

    void OnAnimationCompletion() {
        Destroy(gameObject);
    }

	// Update is called once per frame
	void Update () {
        renderer.material.SetFloat("_Heat", heatValue);
	}
}
