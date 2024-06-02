using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NewPowerUp", menuName = "ScriptableObjects/PowerUps", order = 1)]

public class PowerUp : ScriptableObject
{
    public string powerUpName;
    public string powerUpDescription;
    public Sprite powerUpImage;
    public CardTier cardTier;
    public CardBackgrounds cardBackgrounds;

    public Sprite GetCardTierImage()
    {
        switch (cardTier)
        {
            case CardTier.Bronze:
                return cardBackgrounds.bronzeBackground;
            case CardTier.Silver:
                return cardBackgrounds.silverBackground;
            case CardTier.Gold:
                return cardBackgrounds.goldBackground;
            case CardTier.Diamond:
                return cardBackgrounds.diamondBackground;
            default:
                return null;
        }
    }
}

public enum CardTier
{
    Bronze, Silver, Gold, Diamond
}
