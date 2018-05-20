using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    [SerializeField]
    GameObject CurrPlayerIndicator;

    TextMeshProUGUI textDPIndicator;
    TextMeshProUGUI textAPIndicator;
    TextMeshProUGUI textHPIndicator;
    TextMeshProUGUI textMPIndicator;
    TextMeshProUGUI textLVLIndicator;
    TextMeshProUGUI textXPIndicator;
    TextMeshProUGUI textFoodPointsIndicator;
    TextMeshProUGUI textCurrPlayerIndicator;

    #region Singleton
    public static UIManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
    }
    #endregion

    private void Start()
    {
        ForcedStart();
    }

    public void ForcedStart()
    {
        textDPIndicator = DPIndicator.transform.GetComponentInChildren<TextMeshProUGUI>();
        textAPIndicator = APIndicator.transform.GetComponentInChildren<TextMeshProUGUI>();
        textHPIndicator = HPIndicator.transform.GetComponentInChildren<TextMeshProUGUI>();
        textMPIndicator = MPIndicator.transform.GetComponentInChildren<TextMeshProUGUI>();
        textLVLIndicator = LVLIndicator.transform.GetComponentInChildren<TextMeshProUGUI>();
        textXPIndicator = XPIndicator.transform.GetComponentInChildren<TextMeshProUGUI>();
        textFoodPointsIndicator = FoodPointsIndicator.transform.GetComponentInChildren<TextMeshProUGUI>();
        textCurrPlayerIndicator = CurrPlayerIndicator.transform.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void UIUpdate()
    {
        if (GameManager.instance.currActivePlayer != null)
        {
            textFoodPointsIndicator.text = GameManager.instance.currActivePlayer.foodCount.ToString();
            textCurrPlayerIndicator.text = GameManager.instance.currActivePlayer.playerName;
            if (GameManager.instance.currActivePlayer.activeHex != null)
            {
                if (GameManager.instance.currActivePlayer.activeHex.unit != null)
                {
                    textDPIndicator.text = GameManager.instance.currActivePlayer.activeHex.unit.damagePoints.ToString();
                    textAPIndicator.text = GameManager.instance.currActivePlayer.activeHex.unit.armourPoints.ToString();
                    textHPIndicator.text = GameManager.instance.currActivePlayer.activeHex.unit.currHP.ToString() + "/" + GameManager.instance.currActivePlayer.activeHex.unit.maxHealthPoints.ToString();
                    textMPIndicator.text = GameManager.instance.currActivePlayer.activeHex.unit.movePoints.ToString() + "/" + GameManager.instance.currActivePlayer.activeHex.unit.maxMovePoints.ToString();
                    textLVLIndicator.text = GameManager.instance.currActivePlayer.activeHex.unit.currLevel.ToString() + "/" + GameManager.instance.currActivePlayer.activeHex.unit.maxLevel.ToString();
                    textXPIndicator.text = GameManager.instance.currActivePlayer.activeHex.unit.experiencePoints.ToString() + "/" + GameManager.instance.currActivePlayer.activeHex.unit.experiencePointsPerLevel.ToString();
                    return;
                }

                textDPIndicator.text = "";
                textAPIndicator.text = "";
                textHPIndicator.text = "";
                textMPIndicator.text = "";
                textLVLIndicator.text = "";
                textXPIndicator.text = "";
            }
            return;
        }

        textDPIndicator.text = "";
        textAPIndicator.text = "";
        textHPIndicator.text = "";
        textMPIndicator.text = "";
        textLVLIndicator.text = "";
        textXPIndicator.text = "";
        textFoodPointsIndicator.text = "";
        textCurrPlayerIndicator.text = "";
    }
}
