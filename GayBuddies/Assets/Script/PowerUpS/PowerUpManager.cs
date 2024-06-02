using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public int selectedCardsAmount = 3;
    private Dictionary<int, List<PowerUp>> playerPowerUps = new Dictionary<int, List<PowerUp>>();
    public Transform[] playerCardDisplayParent;
    public GameObject cardPrefab;
    private Dictionary<int, List<GameObject>> displayedCards = new Dictionary<int, List<GameObject>>();

    void Start()
    {
        foreach (Transform parent in playerCardDisplayParent)
        {
            int playerIndex = parent.GetSiblingIndex();

            if (!playerPowerUps.ContainsKey(playerIndex))
            {
                playerPowerUps[playerIndex] = GenerateRandomCards();
            }

            DisplayPlayerCards(playerIndex);
        }
    }

    private List<PowerUp> GenerateRandomCards()
    {
        CardTier randomTier = GenerateRandomCardTier();
        string directory = "ScriptableObjects/PowerUps/" + randomTier.ToString();
        List<PowerUp> powerUps = new List<PowerUp>(Resources.LoadAll<PowerUp>(directory));
        Debug.Log("Loaded " + powerUps.Count + " powerUps from: " + directory);
        
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
    private void DisplayPlayerCards(int playerIndex)
    {
        Transform parent = playerCardDisplayParent[playerIndex];

        if (!displayedCards.ContainsKey(playerIndex))
        {
            displayedCards[playerIndex] = new List<GameObject>();
        }

        foreach (GameObject card in displayedCards[playerIndex])
        {
            Destroy(card);
        }
        displayedCards[playerIndex].Clear();

        int cardWidth = 100; // Set this to the width of your card
        int spacing = 300; // Set this to the desired spacing between cards

        int i = 0;
        int count = playerPowerUps[playerIndex].Count;
        foreach (PowerUp powerUp in playerPowerUps[playerIndex])
        {
            GameObject card = Instantiate(cardPrefab, parent);
            card.GetComponent<CardDisplay>().DisplayCard(powerUp);

            // Set the position of the card
            RectTransform rectTransform = card.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2((i - count / 2) * (cardWidth + spacing), 0);

            displayedCards[playerIndex].Add(card);

            i++;
        }
    }

    private CardTier GenerateRandomCardTier()
    {
        return (CardTier)Random.Range(0, 4);
    }
}
