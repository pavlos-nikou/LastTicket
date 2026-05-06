using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float sensX = 100f;
    public float sensY = 100f;
    public Transform orientation;
    float xRotation;
    float yRotation;
    //void Start()
    //{
    //    Cursor.lockState = CursorLockMode. Locked;
    //    Cursor.visible = false;
    //}

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.IsPaused) return; // ignore all input while paused

        // get mouse input|
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
