using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public List<GameObject> soldierList = new List<GameObject>();

    public List<GameObject> enemyList = new List<GameObject>();
}
