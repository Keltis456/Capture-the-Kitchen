using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    public GameObject mainMenuCanvas;

    public void PlayGame()
    {
        Map.instance.PlayGame();
        GameManager.instance.StartGame();
        Destroy(mainMenuCanvas);
    }

    public void LoadLastSave()
    {
        Map.instance.LoadLastSave();
        GameManager.instance.StartGame();
        Destroy(mainMenuCanvas);
    }
	
    public void ExitGame()
    {
        Debug.Log("Application.Quit()");
        Application.Quit();
    }
}
