using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    void Start()
    {
        
    }

    public void DisplayCard(PowerUp powerUp)
    {
        transform.Find("Name").GetComponent<TMPro.TextMeshProUGUI>().text = powerUp.powerUpName;
        transform.Find("Description").GetComponent<TMPro.TextMeshProUGUI>().text = powerUp.powerUpDescription;
        transform.Find("Icon").GetComponent<UnityEngine.UI.Image>().sprite = powerUp.powerUpImage;
        transform.Find("Background").GetComponent<UnityEngine.UI.Image>().sprite = powerUp.GetCardTierImage();
    }
}
