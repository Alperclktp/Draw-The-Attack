using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class LevelManager : MonoBehaviour
{
    public LevelSOTemplate levelSOTemplate;

    public static LevelManager Instance;

    private void Awake()
    {
        Instance = this;

        if (PlayerPrefs.HasKey("CurrentLevel"))
        {
            int tmpLevelCount = PlayerPrefs.GetInt("CurrentLevel");

            if (tmpLevelCount < levelSOTemplate.levels.Count)
            {
                Instantiate(levelSOTemplate.levels[PlayerPrefs.GetInt("CurrentLevel")].level);
            }
            else
            {
                Instantiate(levelSOTemplate.levels[0].level);
                PlayerPrefs.SetInt("CurrentLevel", 0);
            }
        }
        else
        {
            PlayerPrefs.SetInt("CurrentLevel", 0);
            Instantiate(levelSOTemplate.levels[0].level);
        }

    } 

    [Button("Delete Save")]
    public void DeleteSave()
    {
        PlayerPrefs.DeleteAll();

        GameManager.Instance.RestartGame();

        GameManager.Instance.currentMoney = 1000;
    }
} 

