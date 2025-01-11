using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    // Card selection UI
    public GameObject cardSelectionUI; // the UI that contains the cards
    public GameObject cardPrefab; // Card prefab consisting no unique information, only the card layout (Works as a template)
    public Transform cardPosition1; // Position of the first card
    public Transform cardPosition2; // Position of the second card
    public Transform cardPosition3; // Position of the third card

    // Player PowerUps
    GameObject card1, card2, card3; // The three cards that the player has been given
    private List<PowerUp> playersPowerUps = new List<PowerUp>(); // The total powerups that the player has chosen throughout the game
    public static CardManager instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        GameManager.instance.OnStateChange += OnGameStateChange;
        GenerateRandomCards();
    }

    void OnDestroy()
    {
        GameManager.instance.OnStateChange -= OnGameStateChange;
    }

    // When the game state changes in the GameManager
    void OnGameStateChange(GameManager.GameState newState)
    {
        if (newState == GameManager.GameState.CardSelection)
        {
            GenerateRandomCards();
        }
    }

    void GenerateRandomCards()
    {
        // Remove previous cards
        if (card1 != null)
        {
            Destroy(card1);
        }
        if (card2 != null)
        {
            Destroy(card2);
        }
        if (card3 != null)
        {
            Destroy(card3);
        }

        CardTier randomTier = (CardTier) Random.Range(0, 4);
        string directory = "ScriptableObjects/PowerUps/" + randomTier.ToString();
        List<PowerUp> availablePowerUps = new List<PowerUp>(Resources.LoadAll<PowerUp>(directory)); // All powerups of the specified tier
        List<PowerUp> selectedPowerUps = new List<PowerUp>(); // the three cards that the player has been given

        while (selectedPowerUps.Count < 3)
        {
            PowerUp randomPowerUp = availablePowerUps[Random.Range(0, availablePowerUps.Count)];
            selectedPowerUps.Add(randomPowerUp);
            availablePowerUps.Remove(randomPowerUp);
        }
        card1 = CreateCard(selectedPowerUps[0], cardPosition1);
        card2 = CreateCard(selectedPowerUps[1], cardPosition2);
        card3 = CreateCard(selectedPowerUps[2], cardPosition3);
    }

    GameObject CreateCard(PowerUp powerUp, Transform position)
    {
        GameObject cardGO = Instantiate(cardPrefab, position.position, Quaternion.identity, position);
        Card card = cardGO.GetComponent<Card>();
        card.Setup(powerUp);
        return cardGO;
    }

    public void SelectCard(PowerUp powerUp) {
        playersPowerUps.Add(powerUp);
        GameManager.instance.changeState(GameManager.GameState.Playing);
    }

    public void ShowCardSelection()
    {
        cardSelectionUI.SetActive(true);
    }

    public void HideCardSelection()
    {
        cardSelectionUI.SetActive(false);
    }
}