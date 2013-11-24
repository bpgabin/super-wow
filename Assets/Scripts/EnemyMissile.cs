using UnityEngine;
using System.Collections;

public class EnemyMissile : MonoBehaviour {

    public Transform earth;
    public Transform target;
    public float speed;

	// Use this for initialization
	void Start () {
        Vector3 direction3 = target.position - transform.position;
        Vector2 direction2 = new Vector2(direction3.x, direction3.y).normalized;
        rigidbody2D.velocity = direction2 * speed;	
	}
	
	// Update is called once per frame
	void Update () {

	}

    void FixedUpdate() {
        Vector3 direction3 = earth.position - transform.position;
        Vector2 direction2 = new Vector2(direction3.x, direction3.y).normalized;
        rigidbody2D.AddForce(direction2 * 9.8f);
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject == target.gameObject)
            Destroy(target.gameObject);
        Destroy(gameObject);
    }
}
