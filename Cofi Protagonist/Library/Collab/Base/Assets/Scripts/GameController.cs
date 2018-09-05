using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    [Header("Setup")]
    public int numLanes = 5;
    public float totalWidth = 6;
    public float startingTime = 2f;

    [Space(10)]
    [Header("GameObject arrays")]
    public GameObject[] hazards;
    public GameObject[] bonuses;
    public GameObject[] horizontalEnemies;
    public GameObject[] horizontalWarnings;

    [Space(10)]
    [Header("Rounds")]
    public float roundDuration = 8f;
    public float roundCooldown = 8f;

    [Space(10)]
    [Header("Vertical Waves")]
    public float verticalCooldown = 3f;
    public int waveSize = 2;
    public float bonusRatio = 0.1f;

    
    [Space(10)]
    [Header("Horizontal Waves")]
    public float warningTime = 3f;
    public float horizontalCooldown = 3f;
    public float lowAltitude = 0f;
    public float highAltitude = 0.8f;

    [HideInInspector]
    public float[] lanes;

    [Space(10)]
    [Header("References")]
    public HeartDisplay heartDisp;
    public TextCounter roundCounter;
    public TextCounter coinCounter;
    public BonusDisplay bonusDisplay;
    public PlayerController player;
    public TimerDisplay timer; 

    [Space(10)]
    [Header("Misc")]
    public float gameSpeed = 1.0f;
    [Range(0,0.5f)]
    public float roundGameSeedMultiplicator = 0.05f;
    public float healEveryNRounds = 1;

    [HideInInspector]
    public CoroutineManager coroutineManager;


    public static GameController instance;
	// Use this for initialization
	void Start () {
        instance = this;
        coroutineManager = GetComponent<CoroutineManager>();
        InitLanes();
        coroutineManager.StartCoroutine(round(1));
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator SpawnWaves(float endTime) {
        while (Time.time < endTime)
        {
            HashSet<int> spawns = new HashSet<int>();
            for (int i = 0; i < waveSize; i++)
            {
                spawns.Add(Random.Range(0, numLanes));
            }
            foreach (int lane in spawns)
            {
                GameObject go;
                if (Random.value > bonusRatio)
                    go = hazards[Random.Range(0, hazards.Length)];
                else
                    go = bonuses[Random.Range(0, bonuses.Length)];
                Instantiate(go, new Vector3(lanes[lane], 0, 8), go.transform.rotation);

            }
            yield return new WaitForSeconds(verticalCooldown);
        }
    }

    //STUPID way of doing things, can't pause :/
    //might want to read time.time (can get altered by timerate)
    IEnumerator round(int roundNumber)
    {
        roundCounter.value = roundNumber;
        if(roundNumber % healEveryNRounds == 0)
        {
            addLive();
        }
        gameSpeed = 1 + (roundNumber-1) * roundGameSeedMultiplicator;
        float endTime = Time.time + roundDuration;
        timer.StartTimer(roundDuration);
        coroutineManager.StartCoroutine(SpawnHorizontalWaves(endTime));
        coroutineManager.StartCoroutine(SpawnWaves(endTime));
        yield return new WaitForSeconds(roundDuration + roundCooldown);
        coroutineManager.StartCoroutine(round(roundNumber +1 ));
    }

    IEnumerator SpawnHorizontalWaves(float endTimer)
    {
        while (Time.time < endTimer)
        {
            yield return new WaitForSeconds(horizontalCooldown + Random.Range(1f, 2f));
            int lane = Random.Range(0, 4);
            horizontalWarnings[lane].SetActive(true);
            yield return new WaitForSeconds(warningTime);
            horizontalWarnings[lane].SetActive(false);
            SpawnHorizontalEnemy(lane);
            yield return new WaitForSeconds(warningTime);
        }
    }

    void SpawnHorizontalEnemy(int lane)
    {
        GameObject go = horizontalEnemies[Random.Range(0, horizontalEnemies.Length)];
        //0 2
        //1 3
        Vector3 pos = new Vector3();
        float dir = lane < 2 ? 1 : -1;
        pos.x = totalWidth / 2.0f * -dir;
        pos.z = lane % 2 == 0? highAltitude: lowAltitude; 
        pos.y = 0;
        go = Instantiate(go, pos, go.transform.rotation);
        go.GetComponent<HorizontalEnemy>().direction = dir;
    }

    //temp? maybe allow differently spaced lanes
    void InitLanes()
    {
        lanes = new float[numLanes];
        for( int i = 0; i < numLanes; i++)
        {
            lanes[i] = -totalWidth / 2.0f + totalWidth / (numLanes - 1) * (float)i;
        }
    }

    public void looseLive()
    {
        if (heartDisp.currentLives > 0)
        {
            heartDisp.setLives(heartDisp.currentLives - 1);


            if (Application.platform == RuntimePlatform.Android)
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
        else
        {
            Debug.Log("loose");
        }
    }

    public void addLive()
    {
        if (heartDisp.currentLives < heartDisp.maxLives)
        {
            heartDisp.setLives(heartDisp.currentLives + 1);
        }
    }
}
