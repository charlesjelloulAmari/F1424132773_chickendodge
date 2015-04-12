using UnityEngine;
using System.Collections;

public enum GameState { playing, gameover };

public class GameControl : MonoBehaviour {
	
    public static GameState gameState;
	public static float gravity = 9.8f;
	public static float gameWidth = 34.0f;
	public static float gameHeight = 16.0f;
	
    private Transform playerTrans;

    
	void Awake () {
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
        StartGame();
	}

    void StartGame()
    {
        Time.timeScale = 1.0f;
        gameState = GameState.playing;

    }

    void GameOver()
    {
        Time.timeScale = 0.0f; //Pause the game
        gameState = GameState.gameover;
        GameGUI.SP.CheckHighscore();
    }

	void Update () {
	}

}