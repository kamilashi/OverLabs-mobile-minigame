using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public PlayerManager playerReference;
    [SerializeField]
    public EnvironmentManager environmentReference;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EndDay()
    {
        environmentReference.CalculateCrateLogic();
    }


}
