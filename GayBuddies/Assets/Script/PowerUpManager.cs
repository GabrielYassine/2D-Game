using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public List<PowerUp> bronzePowerUps;
    public List<PowerUp> silverPowerUps;
    public List<PowerUp> goldPowerUps;
    public List<PowerUp> diamondPowerUps;

    public Sprite TempImage;
    private bool listsGenerated = false;
    private int selectedCardsAmount = 3;
    public List<PowerUpSelector> players;

    private void Awake()
    {
        if (!listsGenerated)
        {
            GeneratePowerUpLists();
            listsGenerated = true;
        }
    }

    private void GeneratePowerUpLists()
    {
        // Generate bronze power-ups
        bronzePowerUps.Add(CreatePowerUp("Bronze PowerUp 1", "Description 1", TempImage, CardTier.Bronze));
        bronzePowerUps.Add(CreatePowerUp("Bronze PowerUp 2", "Description 2", TempImage, CardTier.Bronze));
        bronzePowerUps.Add(CreatePowerUp("Bronze PowerUp 3", "Description 3", TempImage, CardTier.Bronze));

        // Generate silver power-ups
        silverPowerUps.Add(CreatePowerUp("Silver PowerUp 1", "Description 1", TempImage, CardTier.Silver));
        silverPowerUps.Add(CreatePowerUp("Silver PowerUp 2", "Description 2", TempImage, CardTier.Silver));
        silverPowerUps.Add(CreatePowerUp("Silver PowerUp 3", "Description 3", TempImage, CardTier.Silver));

        // Generate gold power-ups
        goldPowerUps.Add(CreatePowerUp("Gold PowerUp 1", "Description 1", TempImage, CardTier.Gold));
        goldPowerUps.Add(CreatePowerUp("Gold PowerUp 2", "Description 2", TempImage, CardTier.Gold));
        goldPowerUps.Add(CreatePowerUp("Gold PowerUp 3", "Description 3", TempImage, CardTier.Gold));

        // Generate diamond power-ups
        diamondPowerUps.Add(CreatePowerUp("Diamond PowerUp 1", "Description 1", TempImage, CardTier.Diamond));
        diamondPowerUps.Add(CreatePowerUp("Diamond PowerUp 2", "Description 2", TempImage, CardTier.Diamond));
        diamondPowerUps.Add(CreatePowerUp("Diamond PowerUp 3", "Description 3", TempImage, CardTier.Diamond));
    }

    private PowerUp CreatePowerUp(string name, string description, Sprite image, CardTier tier)
    {
        PowerUp powerUp = ScriptableObject.CreateInstance<PowerUp>();
        powerUp.powerUpName = name;
        powerUp.powerUpDescription = description;
        powerUp.powerUpImage = image;
        powerUp.cardTier = tier;    
        return powerUp;
    }

    public void GenerateCards() {
        GenerateRandomCardTier();
    }

    private void GenerateRandomCardTier() {
        CardTier randomTier = (CardTier)Random.Range(0, 4);
        GeneratePowerUpChoices(randomTier);
    }

    public void GeneratePowerUpChoices(CardTier tier) {
        foreach (PowerUpSelector player in players) {
            List<PowerUp> tierPowerUps = new List<PowerUp>();
            switch (tier) {
                case CardTier.Bronze:
                    tierPowerUps = bronzePowerUps;
                    break;
                case CardTier.Silver:
                    tierPowerUps = silverPowerUps;
                    break;
                case CardTier.Gold:
                    tierPowerUps = goldPowerUps;
                    break;
                case CardTier.Diamond:
                    tierPowerUps = diamondPowerUps;
                    break;
            }
            List<PowerUp> powerUpChoices = new List<PowerUp>();
            for (int i = 0; i < selectedCardsAmount; i++) {
                PowerUp randomPowerUp = tierPowerUps[Random.Range(0, tierPowerUps.Count)];
                powerUpChoices.Add(randomPowerUp);
                tierPowerUps.Remove(randomPowerUp);
            }
            player.ShowPowerUpChoices(powerUpChoices);
        }
    }
}
