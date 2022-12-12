using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    [SerializeField]
    public GameManager GameReference;
    private float _tileSize = 1;
    private const int _gridWidth = 5; // always odd - change to single square size later
    private const int _gridHeight = 5; 
    private int _crateNumber = 1;
    private List<GameObject> _crates = new List<GameObject>();
    private int[,] _positionMap = new int[_gridWidth, _gridHeight];
    private bool _mapLock = false;

    private void Awake()
    {
        UpdatePositionMap(); //only store player coordinates
    }
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_mapLock)
        {
                UpdatePositionMap();
                if (_crateNumber == 0)
                {
                GameReference.SendGameOver();
                }
        }
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

    internal void ResetAll()
    {
        // destroy all old crates:
        foreach (GameObject crate in _crates)
        {
            Destroy(crate); // remove from unity scene
        }
        _crateNumber = 1; // set back to default number
        _crates.Clear(); // clear the list to be filled with new crates later
        Initialize();
    }

    void Initialize()
    {
        GenerateTerrain();
        GenerateCrates();
        GenerateWalls();
        GameReference.SendStartReady();
    }

    void GenerateWalls()
    {
        GameObject refTile = (GameObject)Instantiate(Resources.Load("TileWall"));

        // create horizontal
        for (int i = -1; i <= _gridWidth; i++)
        {
            for (int j = -1; j <= _gridHeight; j+= _gridHeight+1)
            {
                Vector2 position = new Vector2(i - (_gridWidth - _tileSize) / 2, j - (_gridHeight - _tileSize) / 2) * _tileSize;
                GameObject tile = (GameObject)Instantiate(refTile, position, Quaternion.identity, transform);
            }

        }

        // create vertical
        for (int i = -1; i <= _gridWidth; i+=_gridWidth+1)
        {
            for (int j = -1; j <= _gridHeight; j ++)
            {
                Vector2 position = new Vector2(i - (_gridWidth - _tileSize) / 2, j - (_gridHeight - _tileSize) / 2) * _tileSize;
                GameObject tile = (GameObject)Instantiate(refTile, position, Quaternion.identity, transform);
            }

        }

        Destroy(refTile);

    }

    void GenerateCrates()
    {

        int minX = (int)(-(_gridWidth - _tileSize) / 2);
        int maxX = (int)((_gridWidth - _tileSize) / 2); 
        
        int minY = (int)(-(_gridHeight - _tileSize) / 2);
        int maxY = (int)((_gridHeight - _tileSize) / 2);

        GameObject refCrate = (GameObject)Instantiate(Resources.Load("Crate"));
        for (int i = 0; i < _crateNumber; i++)
        {
            Vector3 _crateCoords = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0);
            Vector2 mapIndexes = ToMapIndexes(_crateCoords.x, _crateCoords.y);
            int slot = _positionMap[(int)mapIndexes.x, (int)mapIndexes.y];

            // check if the slot is already occupied, if so - generate and check new coordinates
            while (slot != 0)
            {
                _crateCoords = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0);
                mapIndexes = ToMapIndexes(_crateCoords.x, _crateCoords.y);
                slot = _positionMap[(int)mapIndexes.x, (int)mapIndexes.y];
            }

            _positionMap[(int)mapIndexes.x, (int)mapIndexes.y] = 1;
            Vector2 position = _crateCoords * _tileSize;
            GameObject crate = (GameObject)Instantiate(refCrate, position, Quaternion.identity, transform);
            _crates.Add(crate);
        }
        Destroy(refCrate);
    }

    void UpdatePositionMap()
    {
        if (!_mapLock)
        {
            // prevent concurrent access:
            _mapLock = true;
            System.Array.Clear(_positionMap, 0, _positionMap.Length);

            // get player coordinates
            Vector2 _playerPosition = new Vector2(GameReference.PlayerReference.transform.position.x, GameReference.PlayerReference.transform.position.y);
            Vector2 playerMapIndexes = ToMapIndexes(_playerPosition.x, _playerPosition.y);

            // store player coordinates in position map:
            if (inMatrixBounds(playerMapIndexes))
            {
                _positionMap[(int)playerMapIndexes.x, (int)playerMapIndexes.y] = -1;
            }

            // store crate coordinates - skip if  there are no crates yet
            foreach (GameObject crate in _crates)
            {
                Vector2 mapIndexes = ToMapIndexes(crate.transform.position.x, crate.transform.position.y);
                if (inMatrixBounds(mapIndexes))
                {
                    _positionMap[(int)mapIndexes.x, (int)mapIndexes.y] = 1;
                }
            }

            _mapLock = false;
        }
       
    }



    public void CalculateCrateLogic()
    {
        while (_mapLock)
        {
            // wait for the map lock to release
        }
        _mapLock = true;

        // clear previous position map:
        System.Array.Clear(_positionMap, 0, _positionMap.Length);

        // store player position:
        Vector2 _playerPosition = new Vector2(GameReference.PlayerReference.transform.position.x, GameReference.PlayerReference.transform.position.y);
        Vector2 playerMapIndexes = ToMapIndexes(_playerPosition.x, _playerPosition.y);
        _positionMap[(int)playerMapIndexes.x, (int)playerMapIndexes.y] = -1;

        // store future crate positions:
        foreach (GameObject crate in _crates)
        {
            Vector2 up = crate.transform.position + Vector3.up;
            checkBoundariesAndAddtoMap(up);

            Vector2 right = crate.transform.position + Vector3.right;
            checkBoundariesAndAddtoMap(right);

            Vector2 down = crate.transform.position + Vector3.down;
            checkBoundariesAndAddtoMap(down);

            Vector2 left = crate.transform.position + Vector3.left;
            checkBoundariesAndAddtoMap(left);
        }

        // destroy all old crates:
        foreach (GameObject crate in _crates)
        {
            Destroy(crate); // remove from unity scene
        }
        _crateNumber = 0;
        _crates.Clear(); // clear the list to be filled with new crates later

        // create new crates:
        instantiateCrates();

        _mapLock = false; // release map lock
    }

    void checkBoundariesAndAddtoMap(Vector2 cartesianPosVector)
    {

        int minX = (int)(-(_gridWidth - _tileSize) / 2);
        int maxX = (int)((_gridWidth - _tileSize) / 2);

        int minY = (int)(-(_gridHeight - _tileSize) / 2);
        int maxY = (int)((_gridHeight - _tileSize) / 2);

        if ((cartesianPosVector.x >= minX) && (cartesianPosVector.x <= maxX))
                {
            if ((cartesianPosVector.y >= minY) && (cartesianPosVector.y <= maxY))
            {
                Vector2 mapIndexes = ToMapIndexes(cartesianPosVector.x, cartesianPosVector.y);
                if (_positionMap[(int)mapIndexes.x, (int)mapIndexes.y] >= 0) // ignore the player slot
                { _positionMap[(int)mapIndexes.x, (int)mapIndexes.y] += 1; }
            }
        }
    }

    void instantiateCrates()
    {
        GameObject refCrate = (GameObject)Instantiate(Resources.Load("Crate"));
        for (int i = 0; i < _gridWidth; i++)
        {
            for (int j = 0; j < _gridHeight; j++)
            {
                if (_positionMap[i, j] >= 0) // ignore player slot (-1) 
                {
                    _positionMap[i, j] = _positionMap[i, j] % 2;
                    if (_positionMap[i, j]==1)
                    {
                        Vector3 position = ToPositionCoordinates(i, j) * _tileSize;
                        _crateNumber++;
                        GameObject crate = (GameObject)Instantiate(refCrate, position, Quaternion.identity, transform);
                        _crates.Add(crate);
                    }
                }
            }
        }
        Destroy(refCrate);
    }


    bool inMatrixBounds(Vector2 indexes)
    {
        if ((indexes.x > 0) && (indexes.x < _gridWidth))
        {
            if ((indexes.y > 0) && (indexes.y < _gridHeight))
            {
                return true;
            }
        }
        return false;
    }
    public void PlayerPositionDebug()
    {
        Vector2 _playerPosition = new Vector2(GameReference.PlayerReference.transform.position.x, GameReference.PlayerReference.transform.position.y);
        Vector2 playerMapIndexes = ToMapIndexes(_playerPosition.x, _playerPosition.y);
        _positionMap[(int)playerMapIndexes.x, (int)playerMapIndexes.y] = -1;
        Debug.Log("Player at (map position) " + playerMapIndexes.x + ", " + playerMapIndexes.y);

        Vector3 playerCartPosition = ToPositionCoordinates(playerMapIndexes.x, playerMapIndexes.y) * _tileSize;
        Debug.Log("Player at (unity position) " + playerCartPosition.x + ", " + playerCartPosition.y);
    }

    // convert cartesian position coordinates into row and column indexes
    Vector2 ToMapIndexes(float coordX, float coordY)
    {
        int offsetHorizontal = (int)((_gridWidth - _tileSize) / 2);
        int offsetVertical = (int)((_gridHeight - _tileSize) / 2);
        Vector2 mapCoords = new Vector2(_gridHeight - 1 - (int)(coordY + offsetVertical), (int)(coordX + offsetHorizontal));
        return mapCoords;
    }

    // convert row and column indexes into cartesian position coordinates
    Vector3 ToPositionCoordinates(float mapRow, float mapCol)
    {
        int offsetHorizontal = (int)((_gridWidth - _tileSize) / 2);
        int offsetVertical = (int)((_gridHeight - _tileSize) / 2);
        Vector2 positionCoords = new Vector3((int)(mapCol - offsetHorizontal), (int)(-mapRow + _gridHeight - 1 - offsetVertical), 0);
        return positionCoords;
    }
}
