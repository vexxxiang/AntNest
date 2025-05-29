using System.Collections.Generic;
using UnityEngine;


public class Map : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] public static Map instance;
    [SerializeField] public GameObject Ants;
    [SerializeField] public int Width, Height;
    [SerializeField] public Hex[,] HexMap;
    [SerializeField] public GameObject[] Prefabs;
    [SerializeField] public GameObject[] PrefabsBG;
    [SerializeField] public float[,] NestStructure;
    [SerializeField] public List<Vector2Int> roomCenters;
    [SerializeField] public string[,] Biom; // Normal, Mom
    [SerializeField] public Vector2Int MomCenter; // Mother Center Position

    [Header("Mom")]
    [SerializeField] public GameObject momModel;



    private void Awake()
    {
        NestStructure = new float[Width, Height];
        Biom = new string[Width, Height];


        GenerateStartNest();


    }
    public void Start()
    {
        instance = this;
        

    }
    void GenerateMap()
    {


        HexMap = new Hex[Width, Height];
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                //Normal
                if (NestStructure[x, y] == 0)
                {
                    //Border
                    if (x == 0 || x == Width - 1 || y == 0)
                    {
                        HexMap[x, y] = new Hex(new Vector3(x, y, 0), Prefabs[0], this.gameObject);
                    }
                    //Grass
                    else if (y >= Height - 3)
                    {
                        HexMap[x, y] = new Hex(new Vector3(x, y, 0), Prefabs[1], this.gameObject);
                    }
                    //Dirt
                    else
                    {

                        HexMap[x, y] = new Hex(new Vector3(x, y, 0), Prefabs[2], this.gameObject);
                    }

                }
                //Background
                else
                {
                    //Border
                    if (x == 0 || x == Width - 1 || y == 0)
                    {
                        HexMap[x, y] = new Hex(new Vector3(x, y, 0), Prefabs[0], this.gameObject);
                    }
                    //Grass
                    else if (y >= Height - 3)
                    {
                        HexMap[x, y] = new Hex(new Vector3(x, y, NestStructure[x, y]), PrefabsBG[0], this.gameObject);
                    }
                    //Dirt
                    else
                    {

                        if (Biom[x, y] == "Mom")
                        {
                            HexMap[x, y] = new Hex(new Vector3(x, y, NestStructure[x, y]), PrefabsBG[2], this.gameObject);
                        }
                        else
                        {
                            HexMap[x, y] = new Hex(new Vector3(x, y, NestStructure[x, y]), PrefabsBG[1], this.gameObject);
                        }


                    }
                }



            }
        }
        Debug.Log("Liczba Pokoji:" + NestCount());




    }
    void GenerateStartNest()
    {

        int x = Width / 2;
        int y = Height - 1;

        int steps = 120;

        for (int i = 0; i < steps; i++)
        {

            NestStructure[x, y] = 0.9f;

            if (i < 15)
            {
                int dir = Random.Range(0, 10);

                if (dir < 4 || y > Height - 4) //0-3
                {
                    y = Mathf.Max(2, y - 1); // dó³
                }
                else if (dir < 7) //4-6
                {
                    x = Mathf.Max(1, x - 1); // lewo
                }
                else //7-9
                {
                    x = Mathf.Min(Width - 2, x + 1); // prawo
                }

            }
            else
            {
                int dir = Random.Range(0, 4);
                switch (dir)
                {
                    case 0: x = Mathf.Max(1, x - 1); break; // lewo
                    case 1: x = Mathf.Min(Width - 2, x + 1); break; // prawo
                    case 2: y = Mathf.Min(Height - 4, y + 1); break; // góra
                    case 3: y = Mathf.Max(2, y - 1); break; // dó³
                }

                if (i % 20 == 0 && i > 20)
                {
                    var pos = new Vector2Int(x, y);
                    if (IsFarEnough(pos, roomCenters, 8) && y < (Height - 12))
                    {
                        CreateRoom(x, y, 4, 4);
                        roomCenters.Add(pos);
                    }


                }
            }




        }

        CreateMomBiom();
        GenerateMap();


    }
    void CreateRoom(int cx, int cy, int w, int h)
    {
        for (int dx = -w / 2; dx <= w / 2; dx++)
        {
            for (int dy = -h / 2; dy <= h / 2; dy++)
            {
                int nx = cx + dx;
                int ny = cy + dy;
                if (nx > 0 && nx < Width && ny > 0 && ny < Height)
                {
                    NestStructure[nx, ny] = 0.9f;
                }
            }
        }
    }
    void spawnMom()
    {
        var positionMom = new Vector3(MomCenter.x, MomCenter.y,1f);
        if (MomCenter.y % 2 == 0)
        {
            positionMom = new Vector3(MomCenter.x * Mathf.Sqrt(3), MomCenter.y * 1.5f, 0.4f);

        }
        else
        {
            positionMom = new Vector3((MomCenter.x * Mathf.Sqrt(3)) + (Mathf.Sqrt(3) / 2), MomCenter.y * 1.5f, 0.4f);

        }
        
        var mom = Instantiate(momModel, new Vector3(positionMom.x, positionMom.y, positionMom.z ),new Quaternion(0f,-90f,90f,0f), Ants.transform);
    }
    void CreateMomBiom()
    {
        Vector2Int RndNest = new Vector2Int(Width / 2, Height - 1);
        if (NestCount() == 0)
        {
            Vector2Int NewNestPos = new Vector2Int(Width / 2, Height - 1);
            for (int x = 0; x < NestStructure.GetLength(0); x++)
            {
                for (int y = 0; y < NestStructure.GetLength(1); y++)
                {
                    if (NestStructure[x, y] != 0)
                    {
                        if (y < NewNestPos.y)
                        {
                            NewNestPos = new Vector2Int(x, y);
                        }
                    }
                }
            }
            CreateRoom(NewNestPos.x, NewNestPos.y, 4, 4);
            roomCenters.Add(NewNestPos);
            RndNest = NewNestPos;

        }
        else if (NestCount() == 1)
        {
            RndNest = roomCenters[0];
        }
        else if (NestCount() > 1)
        {
            RndNest = roomCenters[0];
            foreach (var nest in roomCenters)
            {
                if (nest.y < RndNest.y)
                {
                    RndNest = nest;
                }
            }
        }
        MomCenter = RndNest;
        int cx = MomCenter.x;
        int cy = MomCenter.y;
        float MomRadiusBiom = 3f;

        for (int _x = 0; _x < Width; _x++)
        {
            for (int _y = 0; _y < Height; _y++)
            {
                float dist = Vector2.Distance(new Vector2(cx, cy), new Vector2(_x, _y));
                if (dist <= MomRadiusBiom)
                {
                    // np. zmieñ wartoœæ w tablicy
                    if (NestStructure[_x, _y] == 0.9f)
                    {
                        Biom[_x, _y] = "Mom";
                    }
                }
            }
        }
        spawnMom();

    }
    int NestCount()
    {
        var nestcount = 0;
        foreach (var nest in roomCenters)
        {
            if (nest != null)
            {
                nestcount++;
            }
        }
        return nestcount;
    }
    bool IsFarEnough(Vector2Int newRoomPos, List<Vector2Int> existingRooms, int minDistanceRoom)
        {
            foreach (var room in existingRooms)
            {
                if (Vector2Int.Distance(newRoomPos, room) < minDistanceRoom)
                {
                    return false;
                }
                 
            }
            return true;
        }


    }

