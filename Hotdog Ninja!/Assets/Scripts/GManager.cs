using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GManager : MonoBehaviour {

    public GameObject PlayerShip;
    public GameObject ReadyGo;
    public GameObject EnemySpawner;
    public GameObject Endgame;

    public enum GameManagerState
    {
        Opening,
        Gameplay,
        GameOver,
    }

    GameManagerState GMState;

	// Use this for initialization
	void Start () {

        GMState = GameManagerState.Opening;
        UpdateGameManagerState();
       
	}
	
	void UpdateGameManagerState()
    {
        switch(GMState)
        {
            case GameManagerState.Opening:

                Endgame.SetActive(false);
                PlayReadyGo();
                Invoke("ChangeStateToGameplay", 4.02f);

                break;
            case GameManagerState.Gameplay:
                
                PlayerShip.GetComponent<PlayerControl>().Init();

                EnemySpawner.GetComponent<EnemySpawner>().ScheduleEnemySpawner();

                break;
            case GameManagerState.GameOver:

                Endgame.SetActive(true);
                EnemySpawner.GetComponent<EnemySpawner>().UnscheduleEnemySpawner();

                Invoke("MainMenu", 10f);

                break;
        }
        
    }

    public void SetGameManagerState(GameManagerState state)
    {
        GMState = state;
        UpdateGameManagerState();
    }

    public void PlayReadyGo()
    {
        GameObject readyGo = Instantiate(ReadyGo);
        
    } 

    public void ChangeStateToGameplay()
    {
        GMState = GameManagerState.Gameplay;
        UpdateGameManagerState();
    }

    public void ChangeStateToOpening()
    {
        CancelInvoke("MainMenu");
        GMState = GameManagerState.Opening;
        UpdateGameManagerState();
    }

    public void ChangeStateToGameOver()
    {
        GMState = GameManagerState.GameOver;
        UpdateGameManagerState();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
