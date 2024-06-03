using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private PowerUp currentPowerUp; // Store the current power-up

    public void DisplayCard(PowerUp powerUp)
    {
        currentPowerUp = powerUp; // Save the power-up when displaying the card

        transform.Find("Name").GetComponent<TMPro.TextMeshProUGUI>().text = powerUp.powerUpName;
        transform.Find("Description").GetComponent<TMPro.TextMeshProUGUI>().text = powerUp.powerUpDescription;
        transform.Find("Icon").GetComponent<UnityEngine.UI.Image>().sprite = powerUp.powerUpImage;
        transform.Find("Background").GetComponent<UnityEngine.UI.Image>().sprite = powerUp.GetCardTierImage();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Apply hover animation here, for example, change scale or color
        transform.localScale *= 1.1f; // Example: Increase scale by 10%
    }

    // Pointer exit event handler for hover animation
    public void OnPointerExit(PointerEventData eventData)
    {
        // Remove hover animation here, revert back to original scale or color
        transform.localScale /= 1.1f; // Example: Decrease scale by 10%
    }

    // Click event handler for click animation
    public void OnPointerClick(PointerEventData eventData)
    {
        // Apply click animation here
        StartCoroutine(ClickAnimation());
    }

    private IEnumerator ClickAnimation()
    {
        // Make the card smaller
        transform.localScale *= 0.9f;
        yield return new WaitForSeconds(0.1f);

        // Then make it bigger
        transform.localScale /= 0.9f;

        // Print the power-up name
        Debug.Log(currentPowerUp.powerUpName);
    }
}