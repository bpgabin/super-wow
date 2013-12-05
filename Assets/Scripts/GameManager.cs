using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameOver : BaseEvent {

}

public class GameManager : MonoBehaviour, IEventListener {

    public Camera explosionCamera;
    public float spawnDistance = 10f;
    public GameObject outMissile;
    public GameObject targetPrefab;
    public GameObject enemyMissilePrefab;
    public GameObject explosionPrefab;
    public LayerMask earthMask;

    public List<Transform> stations;

    private int missilesSpawned = 1;
    private int m_score = 0;
    private int m_round = 0;

    private bool roundSpawnDone = true;
    private bool gameRunning = true;

    private static GameManager s_instance = null;

    public static GameManager instance {
        get {
            if (s_instance == null) {
                Debug.LogError("GameManger Missing From Scene!");
            }
            return s_instance;
        }
    }

    public int score { get { return m_score; } }
    public int round { get { return m_round; } }

    void Start() {
        Time.timeScale = 1f;
        s_instance = this;

        Holoville.HOTween.HOTween.Init();

        // Register for Events
        EventManager.instance.AddListener(this, "StationDestroyed", OnStationDestroyed);
        EventManager.instance.AddListener(this, "MissileExploded", OnMissileExploded);

        // Begin Infinite Missile Spawner
        //StartCoroutine("SpawnMissiles");

        GameObject go = new GameObject("GUISystem");
        go.AddComponent<GUISystem>();
    }

    public bool HandleEvent(IEvent evt) { return true; }

    public bool OnMissileExploded(IEvent evt) {
        MissileExploded explosionEvent = evt as MissileExploded;
        Vector3 screenLocation = Camera.main.WorldToScreenPoint(explosionEvent.position);
        Vector3 newWorldLocation = explosionCamera.ScreenToWorldPoint(screenLocation);
        newWorldLocation.z = -1f;
        GameObject explosion = Instantiate(explosionPrefab, newWorldLocation, Quaternion.identity) as GameObject;
        StartCoroutine(MissileChecker(explosion, explosionEvent.position));
        return true;
    }

    IEnumerator MissileChecker(GameObject explosion, Vector2 position) {
        while (explosion != null) {
            Collider2D[] others = Physics2D.OverlapCircleAll(position, explosion.transform.localScale.x / 20.0f);
            foreach (Collider2D other in others) {
                if (other.tag == "EnemyMissile") {
                    EventManager.instance.QueueEvent(new MissileExploded(other.transform.position));
                    Destroy(other.gameObject);
                    // Temporary Score
                    if (m_round < 2) {
                        m_score += 50;
                    }
                    else if (m_round < 4) {
                        m_score += 100;
                    }
                    else {
                        m_score += 150;
                    }
                }
            }
            yield return null;
        }
    }

    public bool OnStationDestroyed(IEvent evt) {
        for (int i = 0; i < stations.Count; i++) {
            if (stations[i] == null) {
                stations.RemoveAt(i);
            }
        }
        if (stations.Count == 0) {
            gameRunning = false;
            Time.timeScale = 0f;
            EventManager.instance.QueueEvent(new GameOver());
        }
        return true;
    }

    void Update() {
        // User Missile Launching
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit info;
            if (!Physics.Raycast(ray, out info, earthMask)) {
                LaunchMissile();
            }
        }

        if (roundSpawnDone && gameRunning) {
            GameObject missileCheck = GameObject.FindGameObjectWithTag("EnemyMissile");
            if (!missileCheck) {
                m_round++;
                StartCoroutine("GameRound");
            }
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

    void SpawnMissile() {
        missilesSpawned++;
        float deg = Random.Range(0f, 360f);
        float rad = Mathf.Deg2Rad * deg;
        float x = spawnDistance * Mathf.Cos(rad);
        float y = spawnDistance * Mathf.Sin(rad);
        Vector3 location = new Vector3(x, y, -1f);
        GameObject newMissile = Instantiate(enemyMissilePrefab, location, Quaternion.identity) as GameObject;

        Transform target = GetClosestStation(location);

        newMissile.GetComponent<EnemyMissile>().target = target;
    }

    IEnumerator GameRound() {
        roundSpawnDone = false;
        int numWaves = m_round;
        int missilesPerWave = 4 + m_round;
        for (int i = 0; i < numWaves; i++) {
            for (int j = 0; j < missilesPerWave; j++) {
                SpawnMissile();
            }
            if(i < numWaves - 1) yield return new WaitForSeconds(4f);
        }
        roundSpawnDone = true;
    }

    IEnumerator SpawnMissiles() {
        while (true) {
            int numMissiles = 4;
            for (int i = 0; i < numMissiles; i++) {
                SpawnMissile();
            }
            yield return new WaitForSeconds(5f);
        }
    }
}
