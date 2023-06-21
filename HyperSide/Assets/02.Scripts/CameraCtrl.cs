using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    public Transform way;
    public float maxPosition;

    void Start()
    {
        MoveHorizon(0f);
    }

    public void MoveHorizon(float pos)
    {
        pos -= 0.5f;
        transform.position = new Vector3(pos * maxPosition, transform.position.y, transform.position.z);
    }
}
