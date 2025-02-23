using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] GameObject thingtofollow;
    void Update()
    {
        if (thingtofollow != null)
        {
            transform.position = thingtofollow.transform.position + new Vector3(0, 0, -10);
        }
    }
}
