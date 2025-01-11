using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NewPowerUp", menuName = "ScriptableObjects/PowerUps", order = 1)]

public class PowerUp : ScriptableObject
{

    // PowerUp properties
    public cardEffect effectType; // what kind of buff/debuff the card gives
    public float effectValue; // how much the buff/debuff affects the player
    public CardTier cardTier; // defines card rarity and sets the background
    public bool isUnique; // if the card is unique, it can only be selected once (It may also remove other unique cards from the selection)

    // PowerUp Card look
    public string powerUpName; // Name shown on the card
    public string powerUpDescription; // Description shown on the card
    public Sprite powerUpImage; // Icon shown on the card
    public CardBackgrounds cardBackgrounds; // Background shown on the card

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

public enum cardEffect
{
    DamageIncrease, DamageDecrease, HealthIncrease, HealthDecrease, SpeedIncrease, SpeedDecrease
}
