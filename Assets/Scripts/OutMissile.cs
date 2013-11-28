using UnityEngine;
using System.Collections;

public class MissileExploded : BaseEvent {
    public Vector3 position;
    public MissileExploded(Vector3 position) {
        this.position = position;
    }
}

public class OutMissile : MonoBehaviour {

    public float speed = 3.0f;
    public GameObject target;

    private float startDistance;
    private float closestDistance;

    void Start() {
        rigidbody2D.velocity = transform.position.normalized * speed;
        startDistance = (target.transform.position - transform.position).magnitude;
        closestDistance = startDistance;
    }

    void FixedUpdate() {
        SteerTowardsTarget();
    }

    void SteerTowardsTarget() {
        Vector3 diffVector = target.transform.position - transform.position;
        Vector3 direction = diffVector.normalized;
        float distance = diffVector.magnitude;
        if (distance < closestDistance) closestDistance = distance;
        float turnAmount = 30f * (startDistance / (closestDistance * 3.0f));
        rigidbody2D.AddForce(direction * turnAmount);
        Vector2 clampedVelocity = rigidbody2D.velocity.normalized;
        clampedVelocity *= speed;
        rigidbody2D.velocity = clampedVelocity;

        float angle = Mathf.Atan2(direction.y, direction.x);
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle * Mathf.Rad2Deg));
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject == target) {
            EventManager.instance.QueueEvent(new MissileExploded(other.transform.position));
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
