using UnityEngine;

public class ToggleApp : MonoBehaviour
{
    public GameObject targetObject;

    public void Toggle()
    {
        targetObject.SetActive(!targetObject.activeSelf);
    }
}
