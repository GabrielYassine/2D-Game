using System.Collections;
using System.Collections.Generic;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using UnityEngine;

public class CustomPlayerSpawner : MonoBehaviour
{
    [Header("Player Prefabs")]
    public NetworkObject[] playerPrefabs;
    private int playerIndex = 0;
}
