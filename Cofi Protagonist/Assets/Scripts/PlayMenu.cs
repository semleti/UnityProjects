using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayMenu : MonoBehaviour {

    public void StartArcade()
    {
        GameController.instance.LoadSceneAsync("Level1");
        GameController.instance.heartDisp.setMaxLives(3);
    }
    
    public void StartHardcore()
    {
        GameController.instance.LoadSceneAsync("Level1");
        GameController.instance.heartDisp.setMaxLives(1);
    }
}
