using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    [SerializeField]
    GameObject mainMenuCanvas;
    [SerializeField]
    GameObject mainMenu;
    [SerializeField]
    GameObject newGameMenu;
    [SerializeField]
    GameObject PlayerInput1;
    [SerializeField]
    GameObject PlayerInput2;
    
    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        newGameMenu.SetActive(false);
    }

    public void ShowNewGameMenu()
    {
        newGameMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void PlayGame()
    {
        Map.instance.PlayGame
            (
            PlayerInput1.transform.GetComponentInChildren<InputField>().text, 
            PlayerInput1.transform.GetComponentInChildren<Dropdown>().value.ToString(),
            PlayerInput2.transform.GetComponentInChildren<InputField>().text,
            PlayerInput2.transform.GetComponentInChildren<Dropdown>().value.ToString()
            );
        Destroy(mainMenuCanvas);
    }

    public void LoadLastSave()
    {
        Map.instance.LoadLastSave();
        Destroy(mainMenuCanvas);
    }
	
    public void ExitGame()
    {
        Debug.Log("Application.Quit()");
        Application.Quit();
    }
}
