using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    //loads scene
    public void StartScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    //exits app
	public void ExitApp()
    {
        Application.Quit();
    }
}
