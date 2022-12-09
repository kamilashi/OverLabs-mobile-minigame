using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    private float _tileSize = 1;
    private int _gridWidth = 5;
    private int _gridHeight = 5;
    private const int _crateNumber = 5;
    private Vector2[] _crateCoords = new Vector2[_crateNumber];

    // Start is called before the first frame update
    void Start()
    {
        GenerateTerrain();
        GenerateCrates();
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

    void GenerateCrates()
    {

        GameObject refCrate = (GameObject)Instantiate(Resources.Load("Crate"));
        for (int i = 0; i < _crateNumber; i++)
        {
            int min = (int) (-(_gridWidth - _tileSize) / 2);
            int max = (int) ((_gridWidth - _tileSize) / 2);
            // _crateCoords[i] = new Vector2(Random.Range(min, max), Random.Range(min, max));
            _crateCoords[i] = new Vector2(i-2,-1);
            Vector2 position = _crateCoords[i] * _tileSize;
            GameObject crate = (GameObject)Instantiate(refCrate, position, Quaternion.identity, transform);
        }

        Destroy(refCrate);

    }
}
