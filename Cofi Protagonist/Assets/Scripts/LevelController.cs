using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    //might want to move that to gamecontroller since it needs to know the names to load the scenes and knows which scene is currently active
    public string levelName;

    [System.Serializable]
    public class LevelData
    {
        [Space(10)]
        [Header("GameObject arrays")]
        public GameObject[] hazards;
        public GameObject[] bonuses;
        public GameObject[] horizontalEnemies;

        [Space(10)]
        [Header("Rounds")]
        public float roundDuration = 15f;
        public float roundCooldown = 3f;
        [Range(0, 1f)]
        public float piggyProbability = 0.2f;
        [Range(0, 1f)]
        public float planeProbability = 0.2f;

        [Space(10)]
        [Header("Vertical Waves")]
        public float verticalWarningTime = 2f;
        public float verticalCooldown = 3f;
        public int waveSize = 2;
        [Range(0, 1f)]
        public float bonusRatio = 0.1f;


        [Space(10)]
        [Header("Horizontal Waves")]
        public float horizontalWarningTime = 3f;
        public float horizontalCooldown = 3f;

        [Space(10)]
        [Header("Misc")]
        public float gameSpeed = 1.0f;
        [Range(0, 0.5f)]
        public float roundGameSpeedMultiplicator = 0.05f;
        public float healEveryNRounds = 1;
    }

    public LevelData[] rounds;

    [Header("Setup")]
    public int numLanes = 5;
    private float totalWidth;
    public float startingTime = 2f;
    public GameObject warning;
    public PlayerController player;
    public GameObject root;

    [Space(10)]
    [Header("GameObject arrays")]
    public GameObject piggyBank;
    public GameObject paperPlane;
    
    


    [Space(10)]
    [Header("Horizontal Waves")]
    public float lowAltitude = 0f;
    public float highAltitude = 0.8f;

    [HideInInspector]
    public float[] lanes;

    

    [HideInInspector]
    public CoroutineManager coroutineManager;

    [HideInInspector]
    public LevelData currentRound;


    public static LevelController instance;
    // Use this for initialization
    void Start()
    {
        instance = this;
        #if UNITY_EDITOR
            if(GameController.instance == null)
            {
                SceneManager.LoadSceneAsync("Menus");
            }
        #endif
        Time.timeScale = 1;
        currentRound = rounds[0];

        Camera.main.orthographicSize = (float)numLanes / Camera.main.aspect / 2.0f;
        Vector3 pos = Camera.main.transform.position;
        pos.y = Camera.main.orthographicSize - 1;
        Camera.main.transform.position = pos;
        totalWidth = numLanes;
        coroutineManager = GetComponent<CoroutineManager>();
        InitLanes();
        coroutineManager.StartCoroutine(round(1));
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnWaves(float endTime)
    {
        while (Time.time < endTime)
        {
            HashSet<int> spawns = new HashSet<int>();
            for (int i = 0; i < currentRound.waveSize; i++)
            {
                spawns.Add(Random.Range(0, numLanes));
            }
            foreach (int lane in spawns)
            {
                GameController.instance.verticalWarningsContainer.transform.GetChild(lane).GetComponent<Image>().enabled = true;
            }
            yield return new WaitForSeconds(currentRound.verticalWarningTime);
            foreach (int lane in spawns)
            {
                GameController.instance.verticalWarningsContainer.transform.GetChild(lane).GetComponent<Image>().enabled = false;
            }
            foreach (int lane in spawns)
            {
                GameObject go;
                if (Random.value > currentRound.bonusRatio)
                    go = currentRound.hazards[Random.Range(0, currentRound.hazards.Length)];
                else
                    go = currentRound.bonuses[Random.Range(0, currentRound.bonuses.Length)];
                Instantiate(go, new Vector3(lanes[lane], 8, 0) + root.transform.position, go.transform.rotation,root.transform);

            }
            yield return new WaitForSeconds(currentRound.verticalCooldown);
        }
    }

    //STUPID way of doing things, can't pause :/
    //might want to read time.time (can get altered by timerate)
    public IEnumerator round(int roundNumber)
    {
        GameController.instance.roundCounter.value = roundNumber;
        currentRound = rounds[Mathf.Min(roundNumber , rounds.Length)-1];
        if (roundNumber % currentRound.healEveryNRounds == 0)
        {
            addLive();
        }
        if(Random.Range(0f,1f) <= currentRound.piggyProbability)
        {
            Instantiate(piggyBank, root.transform);
        }
        if (Random.Range(0f, 1f) <= currentRound.planeProbability)
        {
            GameObject plane = Instantiate(paperPlane, root.transform);
            plane.GetComponent<PaperPlane>().bonus = currentRound.bonuses[Random.Range(0, currentRound.bonuses.Length)];
        }
        currentRound.gameSpeed = 1 + (roundNumber - 1) * currentRound.roundGameSpeedMultiplicator;
        float endTime = Time.time + currentRound.roundDuration;
        GameController.instance.timer.StartTimer(currentRound.roundDuration);
        coroutineManager.StartCoroutine(SpawnHorizontalWaves(endTime));
        coroutineManager.StartCoroutine(SpawnWaves(endTime));
        yield return new WaitForSeconds(currentRound.roundDuration + currentRound.roundCooldown);
        coroutineManager.StartCoroutine(round(roundNumber + 1));
    }

    IEnumerator SpawnHorizontalWaves(float endTimer)
    {
        while (Time.time < endTimer)
        {
            yield return new WaitForSeconds(currentRound.horizontalCooldown + Random.Range(1f, 2f));
            int lane = Random.Range(0, 4);
            GameController.instance.horizontalWarnings[lane].GetComponent<Image>().enabled = true;
            yield return new WaitForSeconds(currentRound.horizontalWarningTime);
            GameController.instance.horizontalWarnings[lane].GetComponent<Image>().enabled = false;
            SpawnHorizontalEnemy(lane);
            yield return new WaitForSeconds(currentRound.horizontalWarningTime);
        }
    }

    //FIXED y/z
    void SpawnHorizontalEnemy(int lane)
    {
        GameObject go = currentRound.horizontalEnemies[Random.Range(0, currentRound.horizontalEnemies.Length)];
        //0 2
        //1 3
        Vector3 pos = new Vector3();
        float dir = lane < 2 ? 1 : -1;
        pos.x = totalWidth / 2.0f * -dir;
        pos.y = lane % 2 == 0 ? highAltitude : lowAltitude;
        pos.z = 0;
        go = Instantiate(go, pos + root.transform.position, go.transform.rotation, root.transform);
        go.GetComponent<HorizontalEnemy>().direction = dir;
    }

    //temp? maybe allow differently spaced lanes
    void InitLanes()
    {
        float laneWidth = totalWidth / (float)numLanes;
        lanes = new float[numLanes];
        while (GameController.instance.verticalWarningsContainer.transform.childCount > numLanes)
        {
            Transform tr = GameController.instance.verticalWarningsContainer.transform.GetChild(0);
            tr.SetParent(null);
            Destroy(tr.gameObject);
        }
        while (GameController.instance.verticalWarningsContainer.transform.childCount < numLanes)
        {
            Instantiate(warning, GameController.instance.verticalWarningsContainer.transform, root.transform);
        }
        for (int i = 0; i < numLanes; i++)
        {
            lanes[i] = laneWidth * (i + 0.5f) - totalWidth / 2.0f;
        }
    }

    public void looseLive()
    {
        if (GameController.instance.heartDisp.currentLives > 0)
        {
            GameController.instance.heartDisp.setLives(GameController.instance.heartDisp.currentLives - 1);


            if (false && Application.platform == RuntimePlatform.Android)
            {
                AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject ca = unity.GetStatic<AndroidJavaObject>("currentActivity");
                AndroidJavaClass vibratorClass = new AndroidJavaClass("android.os.Vibrator");
                AndroidJavaObject vibratorService = ca.Call<AndroidJavaObject>("getSystemService", ca.GetStatic<AndroidJavaObject>("VIBRATOR_SERVICE"));
                vibratorService.Call("vibrate", (long)1000);

                unity.Dispose();
                ca.Dispose();
                vibratorClass.Dispose();
                vibratorService.Dispose();

            }
            else
            {
                Handheld.Vibrate();
            }


        }
        if (GameController.instance.heartDisp.currentLives <= 0)
        {
            gameOver();
        }
    }

    public void gameOver()
    {
        Debug.Log("loose\nRounds: " + GameController.instance.roundCounter.value + "\t\tCoins: " + GameController.instance.coinCounter.value);
        Time.timeScale = 0;
        GameController.instance.coinCounterTotalOwned.value += GameController.instance.coinCounter.value;
        GameController.instance.looseMenu.SetActive(true);
    }

    public void quitLevel()
    {
        SceneManager.UnloadSceneAsync(levelName);
        GameController.instance.HUD.GetComponent<Canvas>().enabled = false;
    }

    public void addLive()
    {
        if (GameController.instance.heartDisp.currentLives < GameController.instance.heartDisp.maxLives)
        {
            GameController.instance.heartDisp.setLives(GameController.instance.heartDisp.currentLives + 1);
        }
    }

    public void Restart()
    {
        SceneManager.UnloadSceneAsync(levelName);
        GameController.instance.LoadSceneAsync(levelName);
    }

    public int GetClosestLane(float x)
    {
        x += totalWidth / 2f;
        int lane = Mathf.RoundToInt(x);
        lane = Mathf.Clamp(lane, 0, numLanes - 1);
        return lane;
    }

    public float GetClosestLanePos(float x)
    {
        return lanes[GetClosestLane(x)];
    }
}
