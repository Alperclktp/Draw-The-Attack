using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    public List<Card> cardList = new List<Card>();

    [Header("Mana Settings")]
    public int currentMana;
    public int maxMana;

    [Space(5)]
    public Card selectedCard;

    public GameObject spawnHolder;

    [Header("UI Elements")]
    public Slider manaSliderBar;

    public Text currentManaText;
    public Text maxManaText;

    private float spawnIntervalTimer = 0.04f;

    private void Start()
    {
        currentMana = maxMana;

        manaSliderBar.maxValue = maxMana;
        manaSliderBar.value = maxMana;
    }

    private void Update()
    {
        SpawnCard(selectedCard);

        CheckMana();

        CheckCardInteractable();

        if (selectedCard != null)
        {
            ChooseCard(selectedCard);
        }
    }

    public void CheckCardInteractable()
    {
        foreach (Card card in cardList)
        {
            if (currentMana >= card.currentManaCost)
            {
                card.GetComponent<Button>().interactable = true;
            }
            else
            {
                card.GetComponent<Button>().interactable = false;
            }
        }    
    }

    public void ChooseCard(Card card)
    {
        for (int i = 0; i < cardList.Count; i++) // Deselect operation
        {
            cardList[i].GetComponent<Image>().color = Color.gray;

            cardList[i].IsSelected = false;
        }

        if (currentMana >= card.currentManaCost)
        {
            selectedCard = card;

            card.cardObj.GetComponent<Image>().color = Color.green; // Select operation

            card.IsSelected = true;
        }
        else
        {
            card.GetComponent<Image>().color = Color.gray;
        }
    }

    private void SpawnCard(Card selectedCard)
    {
        spawnIntervalTimer -= Time.deltaTime;

        if (spawnIntervalTimer <= 0)
        {
            if (selectedCard != null && selectedCard.IsSelected && currentMana > 0 && Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
                {
                    if (hit.collider != null && !hit.collider.CompareTag("Soldier"))
                    {
                        GameObject obj = Instantiate(selectedCard.cardPrefab, hit.point, selectedCard.cardPrefab.transform.rotation);

                        obj.transform.parent = spawnHolder.transform;

                        GameManager.Instance.soldierList.Add(obj);

                        currentMana -= selectedCard.currentManaCost;

                        spawnIntervalTimer = 0.04f;

                        Debug.Log("Spawned the: " + selectedCard.name);
                    }
                }
            }
        }
    }

    private void CheckMana()
    {
        manaSliderBar.value = currentMana;

        manaSliderBar.maxValue = maxMana;

        currentManaText.text = currentMana.ToString();
        maxManaText.text = maxMana.ToString();

        if (currentMana <= 0)
        {
            currentMana = 0;
        }

        if (currentMana >= maxMana)
        {
            currentMana = maxMana;
        }
    }
}
