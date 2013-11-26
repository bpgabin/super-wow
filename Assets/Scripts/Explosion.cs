using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class Explosion : MonoBehaviour {

    public GameObject explosionPrefab;

	// Use this for initialization
	void Start () {
        Sequence sequence = new Sequence(new SequenceParms().OnComplete(OnAnimationCompletion));
        sequence.Append(HOTween.From(transform, 3f, "localScale", new Vector3(2f, 2f, 2f)));
        sequence.Append(HOTween.To(transform, 3, "localScale", new Vector3(2f, 2f, 2f)));
        sequence.Play();
	}

    void OnAnimationCompletion() {
        Destroy(gameObject);
    }

	// Update is called once per frame
	void Update () {
        Collider2D[] others = Physics2D.OverlapCircleAll(transform.position, transform.localScale.x / 3);
        foreach (Collider2D other in others) {
            if (other.gameObject.tag == "EnemyMissile") {
                Instantiate(explosionPrefab, other.transform.position, Quaternion.identity);
                Destroy(other.gameObject);
            }
        }
	}
}
