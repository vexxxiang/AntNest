using UnityEngine;

public class Hex
{
    

    public Vector3 _position;
    public GameObject _prefab;
    
    public Hex(Vector3 position, GameObject prefab,GameObject MapParent)
    {
        this._position = position;

        if (position.y % 2 == 0)
        {
            this._prefab = Object.Instantiate(prefab, new Vector3(_position.x * Mathf.Sqrt(3), _position.y * 1.5f , _position.z), new Quaternion(0,180,0,0),MapParent.transform);
            
        }
        else {
            this._prefab = Object.Instantiate(prefab, new Vector3((_position.x * Mathf.Sqrt(3)) + (Mathf.Sqrt(3)/2), _position.y * 1.5f, _position.z), new Quaternion(0, 180, 0, 0),MapParent.transform);
          
        }
        _position = this._prefab.transform.position;
        this._prefab.transform.name = this._prefab.GetComponent<Renderer>().material.name;
        
        



    }


    public void Info()
    {
        Debug.Log("Utworzono: " + this._prefab.name + ", na pozycji:" + _position + "\n");
    }
}
