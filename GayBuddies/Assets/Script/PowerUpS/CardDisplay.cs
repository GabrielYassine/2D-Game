using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private PowerUp currentPowerUp;
    private PowerUpManager powerUpManager;

    void Start()
    {
        powerUpManager = Object.FindAnyObjectByType<PowerUpManager>();
    }

    public void DisplayCard(PowerUp powerUp)
    {
        
        currentPowerUp = powerUp;
        transform.Find("Name").GetComponent<TMPro.TextMeshProUGUI>().text = powerUp.powerUpName;
        transform.Find("Description").GetComponent<TMPro.TextMeshProUGUI>().text = powerUp.powerUpDescription;
        transform.Find("Icon").GetComponent<UnityEngine.UI.Image>().sprite = powerUp.powerUpImage;
        transform.Find("Background").GetComponent<UnityEngine.UI.Image>().sprite = powerUp.GetCardTierImage();
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

    // Animation when the card is clicked
    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(ClickAnimation());
        powerUpManager.ClearDisplayedCards();
    }

    private IEnumerator ClickAnimation()
    {
        transform.localScale *= 0.9f;
        yield return new WaitForSeconds(0.1f);
        transform.localScale /= 0.9f;
    }
}