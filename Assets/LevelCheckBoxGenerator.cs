using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCheckBoxGenerator : MonoBehaviour
{
    public Image levelBox;

    private UpgradeCard upgradeCard;

    public int levelBoxesCount;
    public float sizePerLevel;

    private void Awake()
    {
        upgradeCard = GetComponentInParent<UpgradeCard>();
    }

    private void Update()
    {
        SizePerLevel();
    }

    private void SizePerLevel()
    {
        foreach (var levelBox in upgradeCard.levelBoxes)
        {
            levelBox.transform.localScale = Vector3.one * (sizePerLevel / levelBoxesCount);
        }
    }

    public void SetBoxes(int amount)
    {
        foreach (var levelBox in upgradeCard.levelBoxes)
            Destroy(levelBox);

        levelBoxesCount = 0;
        upgradeCard.levelBoxes = new List<GameObject>();

        AddBoxes(amount);
    }

    public void AddBoxes(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject obj = Instantiate(levelBox, transform.localPosition, Quaternion.identity).gameObject;

            obj.transform.SetParent(transform);

            levelBoxesCount += 1;

            upgradeCard.levelBoxes.Add(obj);
        }
    }
}
