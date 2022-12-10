using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    [SerializeField]
    PlayerManager playerReference;
    private float _tileSize = 1;
    private const int _gridWidth = 5;
    private const int _gridHeight = 5;
    private const int _crateNumber = 5;
    private List<GameObject> _crates = new List<GameObject>();
    private int[,] _positionMap = new int[_gridWidth, _gridHeight];
    private bool _mapLock = false;


    // Start is called before the first frame update
    void Start()
    {
        UpdatePositionMap(); //only store player coordinates
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
        for (int i = 0; i < _gridWidth; i++)
        {
            for (int j = 0; j < _gridHeight; j++)
            {
                Vector2 position = new Vector2(i - (_gridWidth - _tileSize) / 2, j - (_gridHeight - _tileSize) / 2) * _tileSize;
                GameObject tile = (GameObject)Instantiate(refTile, position, Quaternion.identity, transform);
            }

        }
        Destroy(refTile);

    }

    void GenerateCrates()
    {

        int min = (int)(-(_gridWidth - _tileSize) / 2);
        int max = (int)((_gridWidth - _tileSize) / 2);

        GameObject refCrate = (GameObject)Instantiate(Resources.Load("Crate"));
        for (int i = 0; i < _crateNumber; i++)
        {
            Vector3 _crateCoords = new Vector3(Random.Range(min, max), Random.Range(min, max), 0);
            Vector2 mapIndexes = GetMapIndexes(_crateCoords.x, _crateCoords.y);
            int slot = _positionMap[(int)mapIndexes.x, (int)mapIndexes.y];
            while (slot != 0)
            {
                _crateCoords = new Vector3(Random.Range(min, max), Random.Range(min, max), 0);
                mapIndexes = GetMapIndexes(_crateCoords.x, _crateCoords.y);
                slot = _positionMap[(int)mapIndexes.x, (int)mapIndexes.y];
            }

            _positionMap[(int)mapIndexes.x, (int)mapIndexes.y] = 1;
            Vector2 position = _crateCoords * _tileSize;
            GameObject crate = (GameObject)Instantiate(refCrate, position, Quaternion.identity, transform);
            _crates.Add(crate);
        }
        Destroy(refCrate);
    }

    // convert cartesian position coordinates into row and column indexes
    Vector2 GetMapIndexes(float x, float y)
    {
        int offset = (int)((_gridWidth - _tileSize) / 2);
        Vector2 mapCoords = new Vector2(_gridHeight - 1 - (int)(y + offset), (int)(x + offset));
        return mapCoords;
    }

    void UpdatePositionMap()
    {
        if (_mapLock)
        {
            _mapLock = true;
            System.Array.Clear(_positionMap, 0, _positionMap.Length);
            Debug.Log("");

            //store crate coordinates - skip if  there are no crates yet
            foreach (GameObject crate in _crates)
            {
                Vector2 mapIndexes = GetMapIndexes(crate.transform.position.x, crate.transform.position.y);
                _positionMap[(int)mapIndexes.x, (int)mapIndexes.y] = 1;
            }

            //store player coordinates

            Vector2 _playerPosition = new Vector2(playerReference.transform.position.x, playerReference.transform.position.y);
            Vector2 playerMapIndexes = GetMapIndexes(_playerPosition.x, _playerPosition.y);
            //adding player coordinates to position map:
            _positionMap[(int)playerMapIndexes.x, (int)playerMapIndexes.y] = -1;

            _mapLock = false;
        }

    }
}
