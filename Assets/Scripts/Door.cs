using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private float openedAngle, closedAngle;
    [SerializeField]
    private float moveTime = 0.6f;
    [SerializeField]
    private Transform innerKeyholePos, outerKeyholePos;
    [SerializeField, UnityEngine.Serialization.FormerlySerializedAs("audio")]
    private AudioSource audioSource;
    [SerializeField]
    private bool isLocked = false;
    [SerializeField]
    private AudioClip lockSfx, rumbleSfx;
    [SerializeField]
    private Key requiredKey;

    public Key RequiredKey => requiredKey;

    Coroutine routine;

    bool opened = false;
    float currentTime = 0f;

    public bool IsOpened => opened;
    public bool IsLocked => isLocked;

    public void ToggleLocked()
    {
        if(opened)
            return;
        isLocked = !isLocked;
        audioSource.clip = lockSfx;
        audioSource.Play();
    }

    public void ToggleOpen()
    {
        if(isLocked)
        {
            PopupMessage.ShowMessage("Locked.");   
            audioSource.clip = rumbleSfx;
            audioSource.Play();
            return;
        }
        if(routine != null)
            StopCoroutine(routine);
        routine = StartCoroutine(DoToggleOpen());
    }

    IEnumerator DoToggleOpen()
    {
        if(opened)
        {
            opened = false;
            //close
            for(; currentTime > 0; currentTime -= Time.deltaTime)
            {
                transform.localRotation = Quaternion.Euler(0, Mathf.Lerp(closedAngle, openedAngle, currentTime/moveTime), 0);
                yield return null;
            }
        }
        else 
        {
            opened = true;
            //open
            for(; currentTime < moveTime; currentTime += Time.deltaTime)
            {
                transform.localRotation = Quaternion.Euler(0, Mathf.Lerp(closedAngle, openedAngle, currentTime/moveTime), 0);
                yield return null;
            }
        }
        yield return null;
    }

    public Transform GetKeyHole(Vector3 playerPosition)
    {
        float innerDist = Vector3.SqrMagnitude(innerKeyholePos.position - playerPosition);
        float outerDist = Vector3.SqrMagnitude(outerKeyholePos.position - playerPosition);
        if(innerDist < outerDist)
            return innerKeyholePos;
        return outerKeyholePos;
    }
}
