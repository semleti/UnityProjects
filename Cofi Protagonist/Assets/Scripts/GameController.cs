using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameController : MonoBehaviour {

    public const string SAVE_FILENAME = "save.dat";
    public static string SAVE_FILE_PATH;

    [Space(10)]
    [Header("References")]
    public HeartDisplay heartDisp;
    public TextCounter roundCounter;
    public TextCounter coinCounter;
    public TextCounter coinCounterTotalOwned;
    public BonusDisplay bonusDisplay;
    public GameObject verticalWarningsContainer;
    public GameObject[] horizontalWarnings;
    [HideInInspector]
    //public PlayerController player;
    public TimerDisplay timer;
    public GameObject looseMenu;
    public GameObject mainMenu;
    public GameObject HUD;
    public GameObject optionsMenu;

    public static GameController instance;

    // Use this for initialization
    void Start () {
        SAVE_FILE_PATH = Application.persistentDataPath + "/" + SAVE_FILENAME;
        instance = this;
        LoadSave();
    }

    public void LoadSceneAsync(string level)
    {
        SceneManager.LoadSceneAsync(level, LoadSceneMode.Additive);
        GameController.instance.heartDisp.setLives(GameController.instance.heartDisp.maxLives);
        GameController.instance.bonusDisplay.ClearBonuses();
        GameController.instance.mainMenu.GetComponent<Canvas>().enabled = false;
        GameController.instance.HUD.GetComponent<Canvas>().enabled = true;
        GameController.instance.coinCounter.value = 0;
        GameController.instance.optionsMenu.GetComponent<Canvas>().enabled = false;
        for(int i = 0; i < verticalWarningsContainer.transform.childCount; i++)
        {
            verticalWarningsContainer.transform.GetChild(i).GetComponent<Image>().enabled = false;
        }
        for(int i = 0; i < horizontalWarnings.Length; i++)
        {
            horizontalWarnings[i].GetComponent<Image>().enabled = false;
        }
    }

    public void OnApplicationQuit()
    {
        SaveSave();
    }

    [System.Serializable]
    private class SaveData
    {
        public int coins;
    }

    public void LoadSave()
    {
        if (File.Exists(SAVE_FILE_PATH))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(SAVE_FILE_PATH, FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();
            GameController.instance.coinCounterTotalOwned.value = data.coins;
        }
        else
        {
            Debug.Log(SAVE_FILE_PATH + " not found");
        }
    }

    public void SaveSave()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(SAVE_FILE_PATH);

        SaveData data = new SaveData();
        data.coins = GameController.instance.coinCounterTotalOwned.value;

        bf.Serialize(file, data);
        file.Close();

    }

    public void DeleteSave()
    {
        //delete physical save
        File.Delete(SAVE_FILE_PATH);
        SceneManager.LoadSceneAsync("Menus");
    }

}
