using UnityEngine;
using System.Collections;

public class StationDestroyed : BaseEvent {

}

public class EnemyMissile : MonoBehaviour {

    public Transform target;
    public float speed;

    private Vector3 targetPos;
    private float startDistance;

	// Use this for initialization
	void Start () {
        targetPos = target.position;

        float angleOffset = Random.Range(-90, 90);
        float radOffset = angleOffset * Mathf.Deg2Rad;

        Vector3 startDirection = target.position - transform.position;
        startDistance = startDirection.magnitude;
        float angle = Mathf.Atan2(startDirection.y, startDirection.x);
        float finalAngle = angle + radOffset;

        float xSpeed = speed * Mathf.Cos(finalAngle);
        float ySpeed = speed * Mathf.Sin(finalAngle);

        rigidbody2D.velocity = new Vector2(xSpeed, ySpeed);

        speed += GameManager.instance.round / 4.0f;
	}

    void FixedUpdate() {
        SteerTowardsTarget();
    }

    void SteerTowardsTarget() {
        Vector3 diffVector = targetPos - transform.position;
        Vector3 direction = diffVector.normalized;
        float distance = diffVector.magnitude;
        float turnAmount = 5f * (GameManager.instance.round / 4.0f) * (startDistance / (distance * 3.0f));
        rigidbody2D.AddForce(direction * turnAmount);
        Vector2 clampedVelocity = rigidbody2D.velocity.normalized;
        clampedVelocity *= speed;
        rigidbody2D.velocity = clampedVelocity;

        float angle = Mathf.Atan2(rigidbody2D.velocity.y, rigidbody2D.velocity.x);
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle * Mathf.Rad2Deg));
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Station") {
            EventManager.instance.QueueEvent(new StationDestroyed());
            Destroy(other.gameObject);
        }
        Destroy(gameObject);
    }
}
