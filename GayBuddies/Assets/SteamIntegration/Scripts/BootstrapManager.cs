using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FishNet.Managing;
using Steamworks;

public class BootstrapManager : MonoBehaviour
{
    private static BootstrapManager instance;

    private void Awake() {
        instance = this;
    }

    [SerializeField] private string menuName = "MenuSceneSteam";
    [SerializeField] private NetworkManager _networkManager;
    [SerializeField] private FishySteamworks.FishySteamworks _fishySteamworks;

    protected Callback<LobbyCreated_t> LobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> JoinRequest;
    protected Callback<LobbyEnter_t> LobbyEntered;
    public static ulong CurrentLobbyID;

    public void Start() 
    {
        LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        JoinRequest = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
        LobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
    } 

    public void GoToMenu()
    {
        SceneManager.LoadScene(menuName, LoadSceneMode.Additive);
    }

    public static void CreateLobby()
    {
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, 4); // 4 is the max number of players
    }

    private void OnLobbyCreated(LobbyCreated_t pCallback)
    {
        if (pCallback.m_eResult != EResult.k_EResultOK)
        {
            return;
        }
        CurrentLobbyID = pCallback.m_ulSteamIDLobby;
        SteamMatchmaking.SetLobbyData(new CSteamID(CurrentLobbyID), "HostAddress", SteamUser.GetSteamID().ToString());
        SteamMatchmaking.SetLobbyData(new CSteamID(CurrentLobbyID), "name", SteamFriends.GetPersonaName().ToString() + "'s Lobby");
        _fishySteamworks.SetClientAddress(SteamUser.GetSteamID().ToString());
        _fishySteamworks.StartConnection(true); // Starting connection as server (youll be joining as both server and client)
        Debug.Log("Lobby created with ID: " + CurrentLobbyID);
    }

    private void OnJoinRequest(GameLobbyJoinRequested_t pCallback)
    {
        SteamMatchmaking.JoinLobby(pCallback.m_steamIDLobby);
    }

    private void OnLobbyEntered(LobbyEnter_t pCallback)
    {
        CurrentLobbyID = pCallback.m_ulSteamIDLobby;
        MainMenuManager.LobbyEntered(SteamMatchmaking.GetLobbyData(new CSteamID(CurrentLobbyID), "name"), _networkManager.IsServer);
        _fishySteamworks.SetClientAddress(SteamMatchmaking.GetLobbyData(new CSteamID(CurrentLobbyID), "HostAddress"));
        _fishySteamworks.StartConnection(false); // Starting connection as client
        Debug.Log("Client address: " + _fishySteamworks.GetClientAddress());
    }

    public static void JoinByID(CSteamID steamID)
    {
        Debug.Log("Joining lobby with ID: " + steamID.m_SteamID);
        if (SteamMatchmaking.RequestLobbyData(steamID))
        {
            SteamMatchmaking.JoinLobby(steamID);
        } else
        {
            Debug.Log("Failed to join lobby with ID: " + steamID.m_SteamID);
        }
    }

    public static void LeaveLobby()
    {
        SteamMatchmaking.LeaveLobby(new CSteamID(CurrentLobbyID));
        CurrentLobbyID = 0;
        instance._fishySteamworks.StopConnection(false);
        if (instance._networkManager.IsServer)
        {
            instance._fishySteamworks.StopConnection(true);
        }
    }

    public static int GetPlayerCount()
    {
        return SteamMatchmaking.GetNumLobbyMembers(new CSteamID(CurrentLobbyID));
    }
    
}
