using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    int round = 1;
    GameState currentState;

    public delegate void OnStateChangeHandler(GameState newState);
    public event OnStateChangeHandler OnStateChange;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {  
        if(Input.GetKeyDown(KeyCode.P))
        {
            round++;
            Debug.Log("Round: " + round);
            changeState(GameState.CardSelection);
        }
    }

    public void changeState(GameState newState)
    {
        currentState = newState;
        HandleState();
        OnStateChange?.Invoke(newState);
    }

    public void HandleState()
    {
        switch (currentState)
        {
            case GameState.CardSelection:
                CardManager.instance.ShowCardSelection();
                break;
            case GameState.Playing:
                CardManager.instance.HideCardSelection();
                break;
        }
    }

    public enum GameState
    {
        CardSelection,
        Playing
    }
}