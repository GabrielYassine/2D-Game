using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This class is a MonoBehaviour that represents a PowerUpSelector. It has a reference to a powerUpPanel and a powerUpCardPrefab.
// It also has a method to show the powerUpChoices and a method to select a powerUp.

public class PowerUpSelector : MonoBehaviour
{
    public GameObject powerUpPanel;
    public GameObject powerUpCardPrefab;
    private List<PowerUp> currentChoices;

    public void ShowPowerUpChoices(List<PowerUp> powerUpChoices) {
        currentChoices = powerUpChoices;
        powerUpPanel.SetActive(true);

        foreach (Transform child in powerUpPanel.transform) {
            Destroy(child.gameObject);
        }
        
        foreach (PowerUp powerUp in powerUpChoices) {
            GameObject powerUpCard = Instantiate(powerUpCardPrefab, powerUpPanel.transform);
            powerUpCard.transform.Find("PowerUpName").GetComponent<Text>().text = powerUp.powerUpName;
            powerUpCard.transform.Find("PowerUpDescription").GetComponent<Text>().text = powerUp.powerUpDescription;
            powerUpCard.transform.Find("PowerUpImage").GetComponent<Image>().sprite = powerUp.powerUpImage;
        }
    }

    private void SelectPowerUp(int choiceIndex) {
        PowerUp selectedPowerUp = currentChoices[choiceIndex];
        ApplyPowerUp(selectedPowerUp);
        powerUpPanel.SetActive(false);
    }

    private void ApplyPowerUp(PowerUp powerUp) {
        // Depending on powerup, apply effect :)
    }
}
