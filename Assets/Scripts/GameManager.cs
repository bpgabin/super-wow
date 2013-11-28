using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour, IEventListener {

    public Camera explosionCamera;
    public float spawnDistance = 10f;
    public GameObject outMissile;
    public GameObject targetPrefab;
    public GameObject enemyMissilePrefab;
    public GameObject explosionPrefab;

    public List<Transform> stations;

    private int missilesSpawned = 1;

    void Start() {
        Holoville.HOTween.HOTween.Init();

        // Register for Events
        EventManager.instance.AddListener(this, "StationDestroyed", OnStationDestroyed);
        EventManager.instance.AddListener(this, "MissileExploded", OnMissileExploded);

        // Begin Infinite Missile Spawner
        //StartCoroutine("SpawnMissiles");
    }

    public bool HandleEvent(IEvent evt) { return true; }

    public bool OnMissileExploded(IEvent evt) {
        MissileExploded explosionEvent = evt as MissileExploded;
        Vector3 screenLocation = Camera.main.WorldToScreenPoint(explosionEvent.position);
        Vector3 newWorldLocation = explosionCamera.ScreenToWorldPoint(screenLocation);
        newWorldLocation.z = -1f;
        Instantiate(explosionPrefab, newWorldLocation, Quaternion.identity);
        return true;
    }

    public bool OnStationDestroyed(IEvent evt) {
        for (int i = 0; i < stations.Count; i++) {
            if (stations[i] == null) {
                stations.RemoveAt(i);
            }
        }
        if (stations.Count == 0) {
            Application.LoadLevel("Prototype");
        }
        return true;
    }

    void Update() {
        // User Missile Launching
        if (Input.GetMouseButtonDown(0)) {
            LaunchMissile();
        }
    }

    void LaunchMissile() {
        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 targetLocation = new Vector3(target.x, target.y, -1f);

        Transform closestStation = GetClosestStation(targetLocation);
        Transform launchLocation = closestStation.GetChild(0).transform;

        Vector3 launchPos = new Vector3(launchLocation.position.x, launchLocation.position.y, -1f);

        GameObject targetObject = Instantiate(targetPrefab, targetLocation, Quaternion.identity) as GameObject;
        GameObject newMissile = Instantiate(outMissile, launchPos, Quaternion.identity) as GameObject;

        OutMissile missileScript = newMissile.GetComponent<OutMissile>();

        missileScript.target = targetObject;
    }

    Transform GetClosestStation(Vector3 location) {
        float closestDistance = Mathf.Infinity;
        Transform target = null;
        foreach (Transform station in stations) {
            Vector3 distance = station.position - location;
            if (distance.magnitude < closestDistance) {
                closestDistance = distance.magnitude;
                target = station;
            }
        }
        return target;
    }

    IEnumerator SpawnMissiles() {
        while (true) {
            missilesSpawned++;
            float deg = Random.Range(0f, 360f);
            float rad = Mathf.Deg2Rad * deg;
            float x = spawnDistance * Mathf.Cos(rad);
            float y = spawnDistance * Mathf.Sin(rad);
            Vector3 location = new Vector3(x, y, -1f);
            GameObject newMissile = Instantiate(enemyMissilePrefab, location, Quaternion.identity) as GameObject;

            Transform target = GetClosestStation(location);

            newMissile.GetComponent<EnemyMissile>().target = target;
            yield return new WaitForSeconds(3f);
        }
    }
}
