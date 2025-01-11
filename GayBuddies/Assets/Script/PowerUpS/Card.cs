using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public TextMeshProUGUI cardNameRenderer;
    public TextMeshProUGUI cardDescriptionRenderer;
    public UnityEngine.UI.Image cardImageRenderer;
    public UnityEngine.UI.Image cardBackgroundRenderer;
    private PowerUp powerUpInfo;
    private PowerUp selectedPowerUp;

    private void Awake()
    {
            cardNameRenderer = transform.Find("Name").GetComponent<TextMeshProUGUI>();
            cardDescriptionRenderer = transform.Find("Description").GetComponent<TextMeshProUGUI>();
            cardImageRenderer = transform.Find("Icon").GetComponent<UnityEngine.UI.Image>();
            cardBackgroundRenderer = transform.Find("Background").GetComponent<UnityEngine.UI.Image>();
    }

    public void Setup(PowerUp powerUp)
    {
        selectedPowerUp = powerUp;
        powerUpInfo = powerUp;
        cardNameRenderer.text = powerUp.powerUpName;
        cardDescriptionRenderer.text = powerUp.powerUpDescription;
        cardImageRenderer.sprite = powerUp.powerUpImage;
        cardBackgroundRenderer.sprite = powerUp.GetCardTierImage();
    }

    // Animation when the mouse enters the card
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale *= 1.1f;
    }

    // Animation when the mouse exits the card
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale /= 1.1f;
    }

    // When the card is clicked
    public void OnPointerClick(PointerEventData eventData)
    {
        transform.localScale /= 1.1f; // Reset the scale when clicked or else cards keep getting bigger every time they are clicked
        Debug.Log("Card clicked: " + powerUpInfo.powerUpName);
        CardManager.instance.SelectCard(selectedPowerUp);
    }
}