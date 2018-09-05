using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour {
    public Canvas optionsMenuCanvas;
	// Use this for initialization
	void Start ()
    {
        
    }
	
    public void toggleOptionsMenu()
    {
        optionsMenuCanvas.enabled = !optionsMenuCanvas.enabled;
        if (optionsMenuCanvas.enabled)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    public void Quit()
    {
        optionsMenuCanvas.enabled = false;
        GameController.instance.mainMenu.GetComponent<Canvas>().enabled = true;
    }

    public void Restart()
    {
        //destroy all objects
        //reset counter values
        //cancel coroutines
        /*GameController.instance.coroutineManager.StopAllCoroutines();

        toggleOptionsMenu();*/


        //Bruteforce reloading level
        //don't want to be bothered with resetting values
        //might do it later if performance issues or there are other benefits overweighing the work to switch
        LevelController.instance.Restart();
    }

}
