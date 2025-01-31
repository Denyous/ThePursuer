using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject thingtofollow;
    void Update()
    {
        transform.position = thingtofollow.transform.position + new Vector3(0, 0, -10);
    }
}
