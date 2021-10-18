using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private float openedAngle, closedAngle;
    [SerializeField]
    private float moveTime = 0.6f;

    Coroutine routine;

    bool opened = false;
    float currentTime = 0f;

    public void ToggleOpen()
    {
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
}
