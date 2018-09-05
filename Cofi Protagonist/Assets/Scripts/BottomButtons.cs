using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomButtons : MonoBehaviour {

    public GameObject activePanel;


    public void SetActive(GameObject panel)
    {
        activePanel.SetActive(false);
        panel.SetActive( true);
        activePanel = panel;
    }
}
