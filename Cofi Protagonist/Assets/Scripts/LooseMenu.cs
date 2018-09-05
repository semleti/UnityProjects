using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LooseMenu : MonoBehaviour {
    public TextCounter rounds;
    public TextCounter coins;

    public void Update()
    {
        rounds.value = GameController.instance.roundCounter.value;
        coins.value = GameController.instance.coinCounter.value;
    }

    public void Quit()
    {
        GameController.instance.mainMenu.GetComponent<Canvas>().enabled = true;
        HideMenu();
        LevelController.instance.quitLevel();
    }

    public void Restart()
    {
        LevelController.instance.Restart();
        HideMenu();
    }

    private void HideMenu()
    {
        gameObject.SetActive(false);
    }
}
