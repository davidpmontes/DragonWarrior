using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject warrior;

    void Update()
    {
        transform.position = new Vector3((float)(System.Math.Round(warrior.transform.position.x, 2)),
                                          (float)(System.Math.Round(warrior.transform.position.y, 2)),
                                         -10);
    }
}
