using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public PlayerManager PlayerReference;
    [SerializeField]
    public EnvironmentManager EnvironmentReference;
    [SerializeField]
    public Text DayCounterNumberReference;
    private int _dayCounter;
    [SerializeField]
    public GameObject MainMenuScreen;
    [SerializeField]
    public GameObject GameScreen;
    [SerializeField]
    public GameObject GameEndScreen;
    private GameObject _currentScreen;
    private bool _gameOver = false;
    private bool _gameStarted = false;

    private void Awake()
    {
        _dayCounter = 1;
        GameEndScreen.SetActive(false);
        GameScreen.SetActive(false);
        MainMenuScreen.SetActive(false);
        _currentScreen = MainMenuScreen;
    }

    // Start is called before the first frame update
    void Start()
    {
        SwitchToMainMenuScreen();
    }

    // Update is called once per frame
    void Update()
    {
        if ((_gameStarted)&&(!_gameOver))
        {
            updateGameUI();
        }
        
    }

    public void EndDay()
    {
        EnvironmentReference.CalculateCrateLogic();
        _dayCounter++;
        // play animation here;
    }
    
    private void updateGameUI()
    {
        DayCounterNumberReference.text = _dayCounter.ToString();
    }

    public void StartGame()
    {
        _dayCounter = 1;
        PlayerReference.ResetAll();
        EnvironmentReference.Initialize();
        EnvironmentReference.Initialize();
        SwitchToGameScreen();
        _gameStarted = true;
        _gameOver = false;
    }
    internal void SendGameOver()
    {
        _gameOver = true;
        SwitchToGameEndScreen();
    }
    public void SwitchToGameEndScreen()
    {

        _currentScreen.SetActive(false);
        _currentScreen = GameEndScreen;
        _currentScreen.SetActive(true);
    }
    public void SwitchToMainMenuScreen()
    {
        _currentScreen.SetActive(false);
        _currentScreen = MainMenuScreen;
        _currentScreen.SetActive(true);
        _gameOver = false;
    }
    public void SwitchToGameScreen()
    {
        _currentScreen.SetActive(false);
        _currentScreen = GameScreen;
        _currentScreen.SetActive(true);
    }

    internal void SendStartReady()
    {
        SwitchToGameScreen();
        _gameStarted = true;
        _gameOver = false;
    }
}
