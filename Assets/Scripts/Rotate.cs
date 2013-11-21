using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

    // Test
    public Vector3 speed;

	void Update () {
        transform.Rotate(speed * Time.deltaTime);
    }
}
