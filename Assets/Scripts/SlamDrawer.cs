using System.Collections;
using UnityEngine;
public class DrawerSlam : MonoBehaviour
{
    public float slamDistance = 0.4f;
    public float openSpeed = 1f;
    public float slamSpeed = 10f;

    private Vector3 startPos;
    private Coroutine slamCoroutine;
    public AudioClip slamSound;

    public float minDelay = 3f;
    public float maxDelay = 10f;

    private void Start()
    {
        startPos = transform.position;
    }

    public void StartSlamming()
    {
        if (slamCoroutine == null)
        {
            slamCoroutine = StartCoroutine(SlamRoutine());
            // slamCoroutineSounds = StartCoroutine(PlayRandomSounds());
        }
    }

    public void StopSlamming()
    {
        if (slamCoroutine != null)
        {
            StopCoroutine(slamCoroutine);
            // StopCoroutine(slamCoroutineSounds);
            slamCoroutine = null;
            // slamCoroutineSounds = null;
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
            if (GetComponent<AudioSource>())
            {
                GetComponent<AudioSource>().PlayOneShot(slamSound);
            }

        }
    }

    //     IEnumerator PlayRandomSounds()
    //     {
    //         while (true)
    //         {
    //             // Wait random time
    //             float waitTime = Random.Range(minDelay, maxDelay);
    //             yield return new WaitForSeconds(waitTime);
    //             gameObject.GetComponent<AudioSource>().PlayOneShot(clip);
    //         }
    //     }
}