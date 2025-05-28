using System.Collections.Generic;
using UnityEngine;

public class AntManager : MonoBehaviour
{
    private Map _map;
    public GameObject[] ant;
    public GameObject AntsParent;
    public List<Ant> Ants;
    

    public void Start()
    {
        _map = Map.instance;
        
    }
    public void SpawnAntMom()
    {
        var MomSpawn = new Vector3(_map.MomCenter.x, _map.MomCenter.y, 0);

        Ants.Add(new Ant(MomSpawn, ant[0], "Mom", AntsParent));
    }
}
