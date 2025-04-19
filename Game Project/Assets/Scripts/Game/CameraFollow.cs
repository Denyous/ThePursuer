using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform thingtofollow;
    public float speed = 10f;
    void LateUpdate()
    {
        if (thingtofollow != null)
        {
            transform.position = Vector3.Lerp(transform.position, thingtofollow.position, speed * Time.deltaTime);
        }
    }
}
