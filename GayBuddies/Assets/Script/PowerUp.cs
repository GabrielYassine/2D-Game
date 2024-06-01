using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

// This class is a ScriptableObject that represents a PowerUp. It has a name, description, image and tier.

public class PowerUp : ScriptableObject
{
    public string powerUpName;
    public string powerUpDescription;
    public Sprite powerUpImage;
    public CardTier cardTier;
    public Sprite cardTierImage;

    public void SetCardTierImage() {
        string folderPath = "Sprites/PowerUp/";

        switch (cardTier)
        {
            case CardTier.Bronze:
                cardTierImage = Resources.Load<Sprite>(folderPath + "BronzeCard");
                break;
            case CardTier.Silver:
                cardTierImage = Resources.Load<Sprite>(folderPath + "SilverCard");
                break;
            case CardTier.Gold:
                cardTierImage = Resources.Load<Sprite>(folderPath + "GoldCard");
                break;
            case CardTier.Diamond:
                cardTierImage = Resources.Load<Sprite>(folderPath + "DiamondCard");
                break;
        }
    }
}

public enum CardTier
{
    Bronze, Silver, Gold, Diamond
}
