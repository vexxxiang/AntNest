using System.Collections;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    
    [Header("Ustawienia zoomu")]
    [SerializeField] private float scrollSpeed = 0.2f;
    [SerializeField] private float minZ = -10f;
    [SerializeField] private float maxZ = 10f;

    [Header("Ustawienia przesuwania kamery")]
    [SerializeField] private float dragSpeed = 0.1f;
    [SerializeField] private float smoothTime = 0.2f;

    private Vector3 dragOrigin;
    private Vector3 velocity = Vector3.zero;
    private Vector3 targetPosition;
    public GameObject Maps;



    void Start()
    {
        targetPosition = new Vector3(Maps.GetComponent<Map>().HexMap[Maps.GetComponent<Map>().Width / 2, Maps.GetComponent<Map>().Height-1]._position.x, Maps.GetComponent<Map>().HexMap[Maps.GetComponent<Map>().Width / 2, Maps.GetComponent<Map>().Height-1]._position.y-5,-30f);
        StartCoroutine(SwipeToMom());

    }
    IEnumerator SwipeToMom()
    {
        yield return new WaitForSeconds(1.2f);
        targetPosition = new Vector3(Maps.GetComponent<Map>().HexMap[Maps.GetComponent<Map>().MomCenter.x, Maps.GetComponent<Map>().MomCenter.y]._position.x, Maps.GetComponent<Map>().HexMap[Maps.GetComponent<Map>().MomCenter.x, Maps.GetComponent<Map>().MomCenter.y]._position.y, -30f);
    }
    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0f)
        {
            if (scroll > 0f && transform.position.z <= minZ)
            {
                targetPosition = new Vector3(targetPosition.x, targetPosition.y, targetPosition.z + scrollSpeed);
                if (targetPosition.z > minZ)
                {
                    targetPosition.z = minZ;
                }
            }
            else if(scroll < 0f && transform.position.z >= maxZ)
            {
                targetPosition = new Vector3(targetPosition.x, targetPosition.y, targetPosition.z - scrollSpeed);
                if (targetPosition.z > maxZ)
                {
                    targetPosition.z = maxZ;
                }
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 currentMousePos = Input.mousePosition;
            Vector3 difference = dragOrigin - currentMousePos;

            // Przesuwanie w XY
            Vector3 move = new Vector3(difference.x, difference.y, 0) * dragSpeed;
            targetPosition += move;

            dragOrigin = currentMousePos;
        }

        // P³ynne przesuwanie do targetPosition (uwzglêdnia teraz zoom)
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}

