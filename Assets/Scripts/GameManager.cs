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

    private void Awake()
    {
        _dayCounter = 1;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        updateGameUI();
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

}
