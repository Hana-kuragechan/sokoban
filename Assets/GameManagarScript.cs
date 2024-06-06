using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Cinemachine;
using UnityEngine.SceneManagement;

public class GameManagarScript : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject boxPrefab;
    public GameObject storePrefab;
    public GameObject clearText;
    public GameObject WallPrefab;
    int[,] map;
    GameObject[,] field;
    

    

    Vector2Int GetPlayerIndex()
    {
        for (int y = 0; y < field.GetLength(0); y++)
        {
            for (int x = 0; x < field.GetLength(1); x++)
            {
                GameObject obj = field[y, x];

                if (obj != null && obj.tag == "Player")
                {
                    return new Vector2Int(x, y);
                }
            }
        }
        return new Vector2Int(-1, -1);
    }

    bool MoveNumber(Vector2Int moveFrom, Vector2Int moveTo)
    {
        if (moveTo.y < 1 || moveTo.y >= field.GetLength(0)-1) { return false; }
        if (moveTo.x < 1 || moveTo.x >= field.GetLength(1)-1) { return false; }

        if (field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Box")
        {
            Vector2Int velocity = moveTo - moveFrom;
            bool success = MoveNumber(moveTo, moveTo + velocity);
            if (!success) { return false; }
        }



        //field[moveFrom.y, moveFrom.x].transform.position =
        //    new Vector3(moveTo.x, map.GetLength(0) - moveTo.y, 0);

        Vector3 moveToPosition = new Vector3(moveTo.x, map.GetLength(0) - moveTo.y, 0);
        field[moveFrom.y, moveFrom.x].GetComponent<Move>().MoveTo(moveToPosition);
        field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
        field[moveFrom.y, moveFrom.x] = null;
        return true;
    }

    bool IsCleard()
    {
        List<Vector2Int> goals = new List<Vector2Int>();
        for(int y = 0; y < map.GetLength(0); y++)
        {
            for(int x = 0; x < map.GetLength(1); x++)
            {
                if (map[y, x] == 3)
                {
                    goals.Add(new Vector2Int(x, y));
                }
            }
        }

        for(int i = 0; i < goals.Count; i++)
        {
            GameObject f = field[goals[i].y, goals[i].x];
            if (f == null || f.tag != "Box")
            {
                return false;
                
            }
        }
        return true;
    }
   
    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1280, 720, false);
        map = new int[,]
        {
            {4,4,4,4,4,4,4,4,4,4},
            {4,3,0,0,1,0,0,0,0,4},
            {4,0,2,0,0,3,0,2,0,4},
            {4,0,0,2,0,0,3,0,0,4},
            {4,0,0,0,0,0,2,0,0,4},
            {4,3,0,0,0,0,0,0,0,4},
            {4,4,4,4,4,4,4,4,4,4},
        };

        field = new GameObject
        [map.GetLength(0),
        map.GetLength(1)
        ];

        
        string debugText = "";
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                debugText += map[y, x].ToString() + ",";
                if (map[y, x] == 1)
                {
                    field[y, x] = 
                        Instantiate(playerPrefab, 
                        new Vector3(x, map.GetLength(0) - y, 0), 
                        Quaternion.identity);
                }
                else if (map[y, x] == 2)
                {
                    field[y, x] = 
                        Instantiate(boxPrefab, 
                        new Vector3(x, map.GetLength(0) - y, 0), 
                        Quaternion.identity);
                }
                else if (map[y, x] == 3)
                {
                    field[y, x]  =
                        Instantiate(storePrefab,
                        new Vector3(x, map.GetLength(0) - y, 0),
                        Quaternion.identity);
                }
                else if (map[y, x] == 4)
                {
                    field[y, x] =
                        Instantiate(WallPrefab,
                        new Vector3(x, map.GetLength(0) - y, 0),
                        Quaternion.identity);
                }
            }

            debugText += "\n";
        }
        Debug.Log(debugText);

        
    }

    // Update is called once per frame
    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (IsCleard())
        {
            clearText.SetActive(true);
        }

        if (IsCleard() == false)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Vector2Int playerIndex = GetPlayerIndex();
                MoveNumber(
                    playerIndex, playerIndex + new Vector2Int(1, 0));
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Vector2Int playerIndex = GetPlayerIndex();
                MoveNumber(
                    playerIndex, playerIndex - new Vector2Int(1, 0));
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Vector2Int playerIndex = GetPlayerIndex();
                MoveNumber(
                    playerIndex, playerIndex - new Vector2Int(0, 1));
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Vector2Int playerIndex = GetPlayerIndex();
                MoveNumber(
                    playerIndex, playerIndex + new Vector2Int(0, 1));
            }
        }
    }
}
