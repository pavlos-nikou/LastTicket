using System.Collections;
using UnityEngine;
public class DrawerSlam : MonoBehaviour
{
    public float slamDistance = 0.5f;
    public float openSpeed = 1f;
    public float slamSpeed = 10f;

    private Vector3 startPos;
    private Coroutine slamCoroutine;

    private void Start()
    {
        startPos = transform.position;
    }

    public void StartSlamming()
    {
        if (slamCoroutine == null)
        {
            slamCoroutine = StartCoroutine(SlamRoutine());
        }
    }

    public void StopSlamming()
    {
        if (slamCoroutine != null)
        {
            StopCoroutine(slamCoroutine);
            slamCoroutine = null;
        }
    }

    IEnumerator SlamRoutine()
{
    while (true)
    {
        float waitTime = Random.Range(1f, 3f);
        yield return new WaitForSeconds(waitTime);

        Vector3 openPos = startPos + transform.forward * slamDistance;

        while (Vector3.Distance(transform.position, openPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                openPos,
                openSpeed * Time.deltaTime
            );

            yield return null;
        }

        transform.position = openPos;

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
}