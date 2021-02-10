using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;

    void Start()
    {
        // distance between camera and player
        offset = transform.position - player.transform.position;
    }


    void LateUpdate()
    {
        // fix the distance between player and camera
        transform.position = player.transform.position + offset;
    }
}
