using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    public GameObject way;
    public float maxPosition;
    void Start()
    {
        Move(0f);
    }

    public void Move(float position)
    {
        position -= .5f;
        transform.position = new Vector3(position * (way.transform.lossyScale.x - 20f), transform.position.y, transform.position.z);
    }
}
