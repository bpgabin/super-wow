using UnityEngine;
using System.Collections;

public class SpawnExplosion : MonoBehaviour {

    public GameObject explosionPrefab;
    public GameObject missile;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject == missile) {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
