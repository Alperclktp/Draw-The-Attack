using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class CardSelectionTracker : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private Card card;

    private void Awake() => card = GetComponent<Card>();

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("Selected this card: " + card.cardObj.name);

        card.IsSelected = true;
        GetComponent<Image>().color = Color.green;
    }

    public void OnDeselect(BaseEventData eventData)
    {   
        Debug.Log("DeSelect");

        card.IsSelected = false;
        GetComponent<Image>().color = Color.grey;

    }
}
