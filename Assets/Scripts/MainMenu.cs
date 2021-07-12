using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public string levelToLoad = "MainScene";

    public void Play () 
    {
        SceneManager.LoadScene(levelToLoad);
	}
    public void NewGame()
    {
        Data.newGame = true;                        // Если была нажата кнопка "New Game" то в Data-файл записывается то, что началась новая игра
        SceneManager.LoadScene(levelToLoad);        
        
    }

	public void Quit () 
    {
        Debug.Log("Выход из игры...");
        Application.Quit();
	}
}
