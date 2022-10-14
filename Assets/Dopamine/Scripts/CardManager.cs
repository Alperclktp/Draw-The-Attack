using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    public List<Card> cardList = new List<Card>();

    public Card selectedCard;

    public GameObject spawnHolder;

    private void Update()
    {
        SpawnCard(selectedCard);    
    }

    public void ChooseCard(Card card)
    {
        for (int i = 0; i < cardList.Count; i++) // Deselect operation
        {
            cardList[i].GetComponent<Image>().color = Color.gray;

            cardList[i].IsSelected = false;
        }

        selectedCard = card;

        card.cardObj.GetComponent<Image>().color = Color.green; // Select operation

        card.IsSelected = true;

        Debug.Log("Choose this card: " + card.name);
    }

    public void SpawnCard(Card selectedCard)
    {
        if (selectedCard != null && selectedCard.IsSelected && Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                if (hit.collider != null && !hit.collider.CompareTag("Soldier"))
                {
                    GameObject obj = Instantiate(selectedCard.cardPrefab, hit.point, selectedCard.cardPrefab.transform.rotation);

                    obj.transform.parent = spawnHolder.transform;

                    Debug.Log("Spawned the: " + selectedCard.name);
                }
            }
        }
    }
}
