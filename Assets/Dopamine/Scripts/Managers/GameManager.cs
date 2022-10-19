using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    DEFAULT,
    START,
    PAUSE,
    FAIL,
    COMPLATE
}

public class GameManager : Singleton<GameManager>
{
    public Transform endLinePosition;

    public Transform winLinePosition;

    public List<GameObject> soldierList = new List<GameObject>();

    public List<GameObject> enemyList = new List<GameObject>();

    [Header("Game State")]
    public GameState gameState;

    private void Start()
    {
        gameState = GameState.START;
    }
}
