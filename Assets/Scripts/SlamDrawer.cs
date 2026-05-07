using System.Collections;
using UnityEngine;

public class DrawerSlam : MonoBehaviour
{
    public float slamDistance = 0.3f;
    public float slamSpeed = 12f;

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    public void Slam()
    {
        StartCoroutine(SlamRoutine());
    }

    IEnumerator SlamRoutine()
    {
        Vector3 openPos = startPos + transform.forward * slamDistance;

        // move out
        while (Vector3.Distance(transform.position, openPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                openPos,
                slamSpeed * Time.deltaTime
            );

            yield return null;
        }

        // move back
        while (Vector3.Distance(transform.position, startPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                startPos,
                slamSpeed * Time.deltaTime
            );

            yield return null;
        }

        transform.position = startPos;
    }
}