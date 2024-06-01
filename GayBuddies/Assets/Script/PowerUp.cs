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

}

public enum CardTier
{
    Bronze, Silver, Gold, Diamond
}
