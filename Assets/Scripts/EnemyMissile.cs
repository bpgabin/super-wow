using UnityEngine;
using System.Collections;

public class StationDestroyed : BaseEvent {

}

public class EnemyMissile : MonoBehaviour {

    public Transform target;
    public float speed;

	// Use this for initialization
	void Start () {
        Vector3 direction3 = target.position - transform.position;
        Vector2 direction2 = new Vector2(direction3.x, direction3.y).normalized;
        rigidbody2D.velocity = direction2 * speed;

        float angle = Mathf.Atan2(direction2.y, direction2.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
	}

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Station") {
            EventManager.instance.QueueEvent(new StationDestroyed());
            Destroy(other.gameObject);
        }
        Destroy(gameObject);
    }
}
