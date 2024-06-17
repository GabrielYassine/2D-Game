using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public int selectedCardsAmount = 3;
    private List<PowerUp> playerPowerUps = new List<PowerUp>();
    public Transform playerCardDisplayParent;
    public GameObject cardPrefab;
    private List<GameObject> displayedCards = new List<GameObject>();

    void Start()
    {
        playerPowerUps = GenerateRandomCards();
        DisplayPlayerCards();
    }

    private List<PowerUp> GenerateRandomCards()
    {
        CardTier randomTier = GenerateRandomCardTier();
        string directory = "ScriptableObjects/PowerUps/" + randomTier.ToString();
        List<PowerUp> powerUps = new List<PowerUp>(Resources.LoadAll<PowerUp>(directory));

        List<PowerUp> selectedPowerUps = new List<PowerUp>();

        while (selectedPowerUps.Count < selectedCardsAmount)
        {
            PowerUp randomPowerUp = powerUps[Random.Range(0, powerUps.Count)];
            if (!selectedPowerUps.Contains(randomPowerUp))
            {
                selectedPowerUps.Add(randomPowerUp);
            }
        }

        return selectedPowerUps;
    }

    private void DisplayPlayerCards()
    {
        foreach (GameObject card in displayedCards)
        {
            Destroy(card);
        }
        displayedCards.Clear();

        int cardWidth = 100;
        int spacing = 300;

        int i = 0;
        int count = playerPowerUps.Count;
        foreach (PowerUp powerUp in playerPowerUps)
        {
            GameObject card = Instantiate(cardPrefab, playerCardDisplayParent);
            card.GetComponent<CardDisplay>().DisplayCard(powerUp);

            RectTransform rectTransform = card.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2((i - count / 2) * (cardWidth + spacing), 0);

            displayedCards.Add(card);

            i++;
        }
    }

    public void ClearDisplayedCards()
    {
        foreach (GameObject card in displayedCards)
        {
            Destroy(card);
        }
        displayedCards.Clear();
    }

    private CardTier GenerateRandomCardTier()
    {
        return (CardTier)Random.Range(0, 4);
    }


}
