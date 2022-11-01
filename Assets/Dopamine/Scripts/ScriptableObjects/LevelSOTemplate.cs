using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelHolder", menuName = "Create Level Holder", order = 1)]
public class LevelSOTemplate : ScriptableObject
{
    public List<LevelController> levels;
} 

[System.Serializable]
public class LevelController
{
    public GameObject level;

    [Range(0,3)]
    public int hardness;

    public bool isLoop;
}