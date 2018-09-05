using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusDisplay : MonoBehaviour {
    public int maxBonuses = 3;
    [HideInInspector]
    public int numOfBonuses = 0;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddBonus(GameObject bonusUI)
    {
        if (numOfBonuses < maxBonuses)
        {
            GameObject inst = Instantiate(bonusUI,transform);
            inst.transform.SetAsFirstSibling();
            numOfBonuses++;
        }
    }

    public void ActivateBonus(int index)
    {
        if (index >= 0 && transform.childCount > index)
            transform.GetChild(index).GetComponent<BonusUI>().Activate();
    }

    public void ClearBonuses()
    {
        foreach(Transform tr in transform)
        {
            Destroy(tr.gameObject);
        }
    }
}
