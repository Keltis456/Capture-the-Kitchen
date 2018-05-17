using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    [SerializeField]
    GameObject mainMenuCanvas;

    public void PlayGame()
    {
        Map.instance.PlayGame();
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
