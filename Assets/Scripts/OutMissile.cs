using UnityEngine;
using System.Collections;

public class OutMissile : MonoBehaviour {

    public float speed = 3.0f;
    public Vector3 target;

    void Start() {
        Vector3 direction3 = target - transform.position;
        Vector2 direction2 = new Vector2(direction3.x, direction3.y).normalized;
        rigidbody2D.velocity = direction2 * speed;	
    }
}
