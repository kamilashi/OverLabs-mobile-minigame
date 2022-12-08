using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    private float _tileSize = 1;
    private float _gridWidth = 5;
    private float _gridHeight = 5;

    // Start is called before the first frame update
    void Start()
    {
        GenerateTerrain();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateTerrain()
    {
        GameObject refTile = (GameObject)Instantiate(Resources.Load("TileDefault"));
        for (int i = 0; i< _gridWidth; i++ )
        {
            for (int j = 0; j < _gridHeight; j++)
            {
                Vector2 position = new Vector2(i - (_gridWidth-_tileSize)/2, j - (_gridHeight - _tileSize) / 2) * _tileSize;
                GameObject tile = (GameObject) Instantiate(refTile, position, Quaternion.identity, transform);
            }

        }
        Destroy(refTile);

    }
}
