using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpSelector : MonoBehaviour
{
    public GameObject powerUpCard1;
    public GameObject powerUpCard2;
    public GameObject powerUpCard3;
    private List<PowerUp> currentChoices;

    public void ShowPowerUpChoices(List<PowerUp> powerUpChoices) {
        currentChoices = powerUpChoices;
        showCard(powerUpCard1, powerUpChoices[0]);
        showCard(powerUpCard2, powerUpChoices[1]);
        showCard(powerUpCard3, powerUpChoices[2]);
    }

    private void showCard(GameObject card, PowerUp powerUp) {
        card.transform.Find("PowerUpName").GetComponent<Text>().text = powerUp.powerUpName;
        card.transform.Find("PowerUpDescription").GetComponent<Text>().text = powerUp.powerUpDescription;
        card.transform.Find("PowerUpImage").GetComponent<Image>().sprite = powerUp.powerUpImage;
        card.transform.Find("CardTierImage").GetComponent<Image>().sprite = powerUp.cardTierImage;
    }

    private void SelectPowerUp(int choiceIndex) {
        PowerUp selectedPowerUp = currentChoices[choiceIndex];
        ApplyPowerUp(selectedPowerUp);
    }

    private void ApplyPowerUp(PowerUp powerUp) {
        // Depending on powerup, apply effect :)
    }
}
