using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform cameraPosition;
    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = cameraPosition.position;
    }
}
