using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameOver : BaseEvent { }

public class GameLoaded : BaseEvent { }

public class GamePaused : BaseEvent { }

public class GameUnPaused : BaseEvent { }

public class ScoreAmountEvent : BaseEvent { }

public class GameRoundBegin : BaseEvent { }

public class GameRoundEnd : BaseEvent { }

public class GameManager : MonoBehaviour, IEventListener {

    public Camera explosionCamera;
    public float spawnDistance = 10f;
    public GameObject outMissile;
    public GameObject targetPrefab;
    public GameObject enemyMissilePrefab;
    public GameObject explosionPrefab;
    public Transform earthTransform;
    public LayerMask earthMask;

	// Peter being a lazy ass. STATIONS!
	public GameObject station_Q;
	public GameObject station_E;
	public GameObject station_A;
	public GameObject station_D;

    public List<Transform> stations;

    private int missilesSpawned = 1;
    private int m_score = 0;
    private int m_round = 0;
    private int m_earthHP = 6;

    private bool roundSpawnDone = true;
    private bool gameRunning = true;
    private bool paused = false;
    public int scoreThreshold = 0;

    private static GameManager s_instance = null;

    public int remainingMissiles() {
        int ammoLeft = 0;
        foreach (Transform station in stations) {
            ammoLeft += station.gameObject.GetComponent<StationScript>().ammo;
        }
        return ammoLeft;
    }

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
    public int earthHP { get { return m_earthHP; } }

    void Awake() {
        Time.timeScale = 1f;
        s_instance = this;

        Holoville.HOTween.HOTween.Init();

        // Register for Events
        EventManager.instance.AddListener(this, "MissileExploded", this.OnMissileExploded);
        EventManager.instance.AddListener(this, "EarthHit", this.OnEarthHit);

        GUISystem.instance.OnGameLoaded();
    }

    void Start() {
        BeginNewRound();
    }

    public bool HandleEvent(IEvent evt) { return true; }

    public bool OnMissileExploded(IEvent evt) {
        MissileExploded explosionEvent = evt as MissileExploded;
        Vector3 screenLocation = Camera.main.WorldToScreenPoint(explosionEvent.position);
        Vector3 newWorldLocation = explosionCamera.ScreenToWorldPoint(screenLocation);
        newWorldLocation.z = -1f;
        GameObject explosion = Instantiate(explosionPrefab, newWorldLocation, Quaternion.identity) as GameObject;
        StartCoroutine(MissileChecker(explosion, explosionEvent.position));
        return false;
    }

    IEnumerator MissileChecker(GameObject explosion, Vector2 position) {
        while (explosion != null) {
            Collider2D[] others = Physics2D.OverlapCircleAll(position, explosion.transform.localScale.x / 20.0f);
            foreach (Collider2D other in others) {
                if (other.tag == "EnemyMissile") {
                    EventManager.instance.QueueEvent(new MissileExploded(other.transform.position));
                    Destroy(other.gameObject);
                    // Temporary Score
                    int baseScore = 25;
                    if (m_round < 3) {
                        m_score += baseScore * 1;
                        scoreThreshold += baseScore * 1;
                    }
                    else if (m_round < 5) {
                        m_score += baseScore * 2;
                        scoreThreshold += baseScore * 2;
                    }
                    else if(m_round < 7) {
                        m_score += baseScore * 3;
                        scoreThreshold += baseScore * 3;
                    }
                    else if (m_round < 9) {
                        m_score += baseScore * 4;
                    }
                    else if (m_round < 11) {
                        m_score += baseScore * 5;
                        scoreThreshold += baseScore * 5;
                    }
                    else {
                        m_score += baseScore * 6;
                        scoreThreshold += baseScore * 6;
                    }

                    /*
                    if (scoreThreshold >= 3 * scoreThresholdMult) {
                        scoreThreshold = 0;
                        EventManager.instance.QueueEvent(new ScoreAmountEvent());
                    }
                     */

                    int randNum = Random.Range(0, 30);
                    if (randNum == 4) EventManager.instance.QueueEvent(new ScoreAmountEvent());
                }
            }
            yield return null;
        }
    }

    public void OnGamePaused() {
        Time.timeScale = 0f;
        paused = true;
    }

    public void OnGameUnPaused() {
        Time.timeScale = 1f;
        paused = false;
    }

    public bool OnEarthHit(IEvent evt) {
        m_earthHP--;
        if (m_earthHP <= 0) {
            gameRunning = false;
            Time.timeScale = 0f;
            paused = true;
            EventManager.instance.QueueEvent(new GameOver());
            SubmitStats();
        }
        return false;
    }

    void Update() {
        // User Pause Options
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (!paused) {
                GUISystem.instance.OnGamePaused();
                OnGamePaused();
            }
            else {
                GUISystem.instance.OnGameUnPaused();
                OnGameUnPaused();
            }
        }

        if (!paused) {
            GameObject missileCheck = GameObject.FindGameObjectWithTag("EnemyMissile");
            if (!roundSpawnDone || missileCheck) {

                // User Missile Launching
                if (Input.GetMouseButtonDown(0)) {
                    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 100f, earthMask);
                    if (hit.collider == null) {
                        LaunchConvenientMissile();
                    }
                }



                if (Input.GetKeyDown(KeyCode.Space)) {
                    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 100f, earthMask);
                    if (hit.collider == null) {
                        LaunchConvenientMissile();
                    }
                }

                if (Input.GetKeyDown(KeyCode.Q)) {
                    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 100f, earthMask);
                    if (hit.collider == null) {
                        LaunchMissile(station_Q);
                    }
                }

                if (Input.GetKeyDown(KeyCode.E)) {
                    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 100f, earthMask);
                    if (hit.collider == null) {
                        LaunchMissile(station_E);
                    }
                }

                if (Input.GetKeyDown(KeyCode.A)) {
                    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 100f, earthMask);
                    if (hit.collider == null) {
                        LaunchMissile(station_A);
                    }
                }

                if (Input.GetKeyDown(KeyCode.D)) {
                    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 100f, earthMask);
                    if (hit.collider == null) {
                        LaunchMissile(station_D);
                    }
                }
            }

            if (roundSpawnDone && gameRunning) {
                GameObject ourMissileCheck = GameObject.FindGameObjectWithTag("OurMissile");
                GameObject explosionCheck = GameObject.FindGameObjectWithTag("Explosion");
                if (!missileCheck && !ourMissileCheck && !explosionCheck) {
                    paused = true;
                    GUISystem.instance.OnRoundOver();
                    RoundEndCalc();
                    EventManager.instance.QueueEvent(new GameRoundEnd());
                    //Time.timeScale = 0f;
                }
            }
        }
    }

    void RoundEndCalc() {
        m_score += remainingMissiles() * 5;
        m_score += earthHP * 100;
        scoreThreshold += remainingMissiles() * 5;
        scoreThreshold += earthHP * 100;
    }

    void SubmitStats() {
        KongregateAPI.instance.SubmitStats("HighScore", m_score);
        KongregateAPI.instance.SubmitStats("GameCompleted", 1);
        KongregateAPI.instance.SubmitStats("HighestRound", m_round);
    }

    public void BeginNewRound() {
        Holoville.HOTween.HOTween.Kill();
        m_round++;
        Time.timeScale = 1.0f;
        ResetStations();
        paused = false;
        StartCoroutine("GameRound");
    }

    void LaunchConvenientMissile() {
        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 targetLocation = new Vector3(target.x, target.y, -1f);

        Transform closestStation = GetClosestStation(targetLocation);
        if (closestStation != null) {
            StationScript closestStationSript = closestStation.GetComponent<StationScript>();
            closestStationSript.OnStationFired();

            Transform launchLocation = closestStation.GetChild(0).transform;

            Vector3 launchPos = new Vector3(launchLocation.position.x, launchLocation.position.y, -1f);

            GameObject targetObject = Instantiate(targetPrefab, targetLocation, Quaternion.identity) as GameObject;
            GameObject newMissile = Instantiate(outMissile, launchPos, Quaternion.identity) as GameObject;

            OutMissile missileScript = newMissile.GetComponent<OutMissile>();

            missileScript.target = targetObject;
        }
    }

	void LaunchMissile(GameObject station) {
		Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3 targetLocation = new Vector3(target.x, target.y, -1f);

        StationScript stationScript = station.GetComponent<StationScript>();
        if (stationScript.ammo > 0) {
            stationScript.OnStationFired();

            Transform launchLocation = station.transform.GetChild(0).transform;

            Vector3 launchPos = new Vector3(launchLocation.position.x, launchLocation.position.y, -1f);

            GameObject targetObject = Instantiate(targetPrefab, targetLocation, Quaternion.identity) as GameObject;
            GameObject newMissile = Instantiate(outMissile, launchPos, Quaternion.identity) as GameObject;

            OutMissile missileScript = newMissile.GetComponent<OutMissile>();

            missileScript.target = targetObject;
        }
	}

    Transform GetClosestStation(Vector3 location) {
        float closestDistance = Mathf.Infinity;
        Transform target = null;
        foreach (Transform station in stations) {
            Vector3 distance = station.position - location;
            if (distance.magnitude < closestDistance) {
                if (station.GetComponent<StationScript>().ammo > 0) {
                    closestDistance = distance.magnitude;
                    target = station;
                }
            }
        }
        return target;
    }

    void SpawnMissile() {
        missilesSpawned++;
        float deg;
        int randNumSide = Random.Range(0, 4);
        if (randNumSide == 0)
            deg = Random.Range(0f, 50f);
        else if (randNumSide == 1)
            deg = Random.Range(130f, 180f);
        else if (randNumSide == 2)
            deg = Random.Range(180f, 230f);
        else
            deg = Random.Range(310f, 360f);
        float rad = Mathf.Deg2Rad * deg;
        float x = spawnDistance * Mathf.Cos(rad);
        float y = spawnDistance * Mathf.Sin(rad);
        Vector3 location = new Vector3(x, y, -1f);
        GameObject newMissile = Instantiate(enemyMissilePrefab, location, Quaternion.identity) as GameObject;

        Transform target;
        int randNum = Random.Range(0, 2);
        if(randNum == 0)
            target = GetClosestStation(location);
        else
            target = earthTransform;

        newMissile.GetComponent<EnemyMissile>().target = target;
    }

    void ResetStations() {
        foreach (Transform station in stations) {
            station.gameObject.GetComponent<StationScript>().ResetStation();
        }
    }

    IEnumerator GameRound() {
        if (scoreThreshold > 10000) {
            m_earthHP++;
            scoreThreshold -= 10000;
        }
        if (earthHP > 6) m_earthHP = 6;
        roundSpawnDone = false;
        EventManager.instance.QueueEvent(new GameRoundBegin());
        int numWaves = 2 + Mathf.FloorToInt(m_round / 4.0f);
        int missilesPerWave = 4 + Mathf.FloorToInt(m_round / 5.0f);
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
            int numMissiles = 4 + Mathf.FloorToInt(m_round / 2.0f);
            for (int i = 0; i < numMissiles; i++) {
                SpawnMissile();
            }
            yield return new WaitForSeconds(4f);
        }
    }
}
