using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using MoreMountains.NiceVibrations;

public class CardManager : Singleton<CardManager>
{
    public List<Card> cardList = new List<Card>();

    [Space(5)]
    [SerializeField] public Card selectedCard;

    [SerializeField] private GameObject soldierSpawnHolder;

    [Header("Card Colors")]
    //[SerializeField] public Color defaultColor;
    //[SerializeField] private Color selectedColor;
    //[SerializeField] private Color unselectedColor;

    private float spawnIntervalTimer = 0.04f;

    private void Start()
    {
        for (int i = 0; i < cardList.Count; i++)
        {
            //cardList[i].GetComponent<Image>().color = defaultColor;
        }
    }

    private void Update()
    {   
        CheckCardInteractable();

        if (selectedCard != null)
        {
            ChooseCard(selectedCard);
        }

        SpawnCard(selectedCard);
    }

    public void CheckCardInteractable()
    {
        foreach (Card card in cardList)
        {
            if (GameManager.Instance.currentMana >= card.currentManaCost)
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
            //cardList[i].GetComponent<Image>().color = defaultColor;

            cardList[i].IsSelected = false;

            //cardList[i].transform.DOScale(1.05f, 0.2f);
            cardList[i].transform.DOScale(1.2f, 0.2f);

        }

        if (GameManager.Instance.currentMana >= card.currentManaCost)
        {
            selectedCard = card;

            //card.cardObj.GetComponent<Image>().color = selectedColor; // Select operation

            card.IsSelected = true;

            //card.transform.DOScale(1.15f, 0.2f);
            card.transform.DOScale(1.3f, 0.2f);

            if (GameManager.Instance.tutorial)
            {
                //GameManager.Instance.DrawCardTutorialHand();
            }
        }
        else
        {
            //card.GetComponent<Image>().color = Color.gray;
        }
    }

    public void OnClick()
    {
        MMVibrationManager.Haptic(HapticTypes.Selection, true, this);
    }

    [HideInInspector] public bool tutorialLock, onTutorial;

    private void SpawnCard(Card selectedCard)
    {
        spawnIntervalTimer -= Time.deltaTime;

        if (spawnIntervalTimer <= 0)
        {
            if (selectedCard != null && selectedCard.IsSelected && GameManager.Instance.currentMana > 0 && Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
                {
                    if (hit.collider != null && !hit.collider.CompareTag("Soldier") && !hit.collider.CompareTag("Enemy") && hit.collider.CompareTag("Ground"))
                    {

                        if (tutorialLock)
                            return;

                        GameObject obj = Instantiate(selectedCard.cardPrefab, hit.point, selectedCard.cardPrefab.transform.rotation);

                        obj.transform.parent = soldierSpawnHolder.transform;

                        GameManager.Instance.soldierList.Add(obj);

                        GameManager.Instance.currentMana -= onTutorial ? 1 : selectedCard.currentManaCost;

                        spawnIntervalTimer = 0.04f;

                        Destroy(VFXManager.SpawnEffect(VFXType.CARD_SPAWN_EFFECT, obj.transform.position + new Vector3(0, 1, 0), Quaternion.identity), 1);

                        //Debug.Log("Spawned the: " + selectedCard.name);

                        //GameManager.Instance.tutorial = false;
                        //GameManager.Instance.Tutorial();
                        //GameManager.Instance.Tutorial2();

                        MMVibrationManager.Haptic(HapticTypes.Selection, true, this);

                    }
                }
            }
            else if(selectedCard != null && GameManager.Instance.currentMana < selectedCard.currentManaCost && Input.GetMouseButton(0))
            {
                GameManager.Instance.manaSliderBar.GetComponent<Animator>().SetTrigger("EnergyBarAlert");

                //The warning animation is triggered even though we have enough mana  *********************** *********************** ***********************                                                     
            }
        }
    }
}
