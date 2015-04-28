using UnityEngine;
using System.Collections;

public enum GameState { playing, gameover };

public class GameControl : MonoBehaviour {
	
    public static GameState gameState;
	public static float gravity = 9.8f;
	public static float levelWidth = 35.5f;
	public static float levelHeight = 15.5f;

	public static float overHeight = 10.28f - 7.75f; //position de la camera en y, - la psoition en y de la normal initiale
	
    private Transform playerTrans;
	private Transform cameraTrans;
	private Playermovement playerscript;

	public int compteur;

    
	void Awake () {
		playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
		cameraTrans = GameObject.FindGameObjectWithTag("MainCamera").transform;
		playerscript = (Playermovement) GameObject.FindGameObjectWithTag ("Player").
			GetComponent(typeof(Playermovement));

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
		if (playerTrans.position.x > cameraTrans.position.x + levelWidth/2){
			Vector3 temp = new Vector3(levelWidth,0,0);
			cameraTrans.position += temp;
			playerscript.setSalt(false);
		}
		else if (playerTrans.position.x < cameraTrans.position.x - levelWidth/2) {
			Vector3 temp = new Vector3(-levelWidth,0,0);
			cameraTrans.position += temp;
			playerscript.setSalt(false);
		}

		if (playerTrans.position.y > cameraTrans.position.y - overHeight + levelHeight/2){
			Vector3 temp = new Vector3(0,levelHeight,0);
			cameraTrans.position += temp;
			playerscript.setSalt(false);
		}
		else if (playerTrans.position.y < cameraTrans.position.y - overHeight - levelHeight/2) {
			Vector3 temp = new Vector3(0,-levelHeight,0);
			cameraTrans.position += temp;
			playerscript.setSalt(false);
		}
	}

}