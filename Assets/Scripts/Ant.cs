using UnityEngine;

public class Ant 
{
    public GameObject _prefab;
    public Vector3 _position;

    public Ant(Vector3 Position, GameObject Model, string Caste,GameObject AntsParent)
    {
        _position = Position;
        this._prefab = Object.Instantiate(Model, new Vector3(_position.x * Mathf.Sqrt(3), _position.y * 1.5f, _position.z), new Quaternion(0, 180, 0, 0), AntsParent.transform);
    }
}
