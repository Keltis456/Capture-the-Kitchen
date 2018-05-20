using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    [SerializeField]
    GameObject DPIndicator;
    [SerializeField]
    GameObject APIndicator;
    [SerializeField]
    GameObject HPIndicator;
    [SerializeField]
    GameObject MPIndicator;
    [SerializeField]
    GameObject LVLIndicator;
    [SerializeField]
    GameObject XPIndicator;
    [SerializeField]
    GameObject FoodPointsIndicator;

    Text textDPIndicator;
    Text textAPIndicator;
    Text textHPIndicator;
    Text textMPIndicator;
    Text textLVLIndicator;
    Text textXPIndicator;
    Text textFoodPointsIndicator;

    private void Start()
    {/*
        textDPIndicator = DPIndicator.transform.GetComponentInChildren<Text>();
        textAPIndicator = APIndicator.transform.GetComponentInChildren<Text>();
        textHPIndicator = HPIndicator.transform.GetComponentInChildren<Text>();
        textMPIndicator = MPIndicator.transform.GetComponentInChildren<Text>();
        textLVLIndicator = LVLIndicator.transform.GetComponentInChildren<Text>();
        textXPIndicator = XPIndicator.transform.GetComponentInChildren<Text>();
        textFoodPointsIndicator = FoodPointsIndicator.transform.GetComponentInChildren<Text>();*/
    }

    public void UIUpdate()
    {
        if (GameManager.instance.currActivePlayer != null)
        {
            textFoodPointsIndicator.text = GameManager.instance.currActivePlayer.foodCount.ToString();
            if (GameManager.instance.currActivePlayer.activeHex != null)
            {

            }
            return;
        }

        textFoodPointsIndicator.text = "";
        textDPIndicator.text = "";
        textAPIndicator.text = "";
        textHPIndicator.text = "";
        textMPIndicator.text = "";
        textLVLIndicator.text = "";
        textXPIndicator.text = "";
        textFoodPointsIndicator.text = "";
    }
}
