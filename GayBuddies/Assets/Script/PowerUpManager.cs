using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is a MonoBehaviour that represents a PowerUpManager. It has a list of availablePowerUps and a list of selectedPowerUps.
// It also has a method to generate powerUpChoices and a method to show powerUpChoices.

public class PowerUpManager : MonoBehaviour
{
    public List<PowerUp> availablePowerUps;
    private List<PowerUp> selectedPowerUps;
    private int selectedCardsAmount = 3;
    public List<PowerUpSelector> players;

    public void GeneratePowerUpChoices(CardTier tier) {
        List<PowerUp> tierPowerUps = availablePowerUps.FindAll(powerUp => powerUp.cardTier == tier);

        for (int i = 0; i < selectedCardsAmount; i++) {
            PowerUp selectedPowerUp = tierPowerUps[Random.Range(0, tierPowerUps.Count)];
            selectedPowerUps.Add(selectedPowerUp);
            tierPowerUps.Remove(selectedPowerUp);
        }
    }

    private void GenerateRandomCardTier() {
        CardTier randomTier = (CardTier)Random.Range(0, 4);
        GeneratePowerUpChoices(randomTier);
    }

    public void ShowPowerUpChoices() {
        foreach (PowerUpSelector player in players) {
            player.ShowPowerUpChoices(selectedPowerUps);
        }
    }
}
