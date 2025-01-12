using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Steamworks;
public class MainMenuManager : MonoBehaviour
{
    private static MainMenuManager instance;

    [SerializeField] private GameObject menuScreen, lobbyScreen;
    [SerializeField] private TMP_InputField lobbyCodeInput;
    [SerializeField] private TextMeshProUGUI lobbyTitleText, LobbyIDText;

    [SerializeField] private Button startGameButton;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        OpenMainMenu();
    }

    public void CreateLobby()
    {
        BootstrapManager.CreateLobby();
    } 

    public void OpenMainMenu()
    {
        CloseAllScreens();
        menuScreen.SetActive(true);
    }

    public void OpenLobby()
    {
        CloseAllScreens();
        lobbyScreen.SetActive(true);
    }

    public static void LobbyEntered(string lobbyName, bool isHost)
    {
        instance.lobbyTitleText.text = lobbyName;
        instance.LobbyIDText.text = "Lobby ID: " + BootstrapManager.CurrentLobbyID;
        instance.startGameButton.gameObject.SetActive(isHost);
        instance.OpenLobby();
    }


    void CloseAllScreens()
    {
        menuScreen.SetActive(false);
        lobbyScreen.SetActive(false);
    }

    public void JoinLobby()
    {
        CSteamID steamID = new CSteamID(ulong.Parse(lobbyCodeInput.text));
        BootstrapManager.JoinByID(steamID);
    }

    public void LeaveLobby()
    {
        BootstrapManager.LeaveLobby();
        OpenMainMenu();
    }

    public void StartGame()
    {
        string[] scenesToClose = new string[] { "MenuSceneSteam" };
        BootstrapNetworkManager.ChangeNetworkScene("CardFeature", scenesToClose); // Change this to your game scene, right now thats the newely created scene
    }

}
