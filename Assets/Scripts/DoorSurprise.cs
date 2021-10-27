using UnityEngine;
using System;
using System.Collections;

public class DoorSurprise : MonoBehaviour
{
    [SerializeField]
    private Door door;

    [SerializeField]
    private GameObject surpriseObject;

    [SerializeField]
    private bool onlyOnce = true;

    Coroutine routine;

    void Start()
    {
        if(door is null)
            door = GetComponent<Door>();

        door.OnStartPeeping += OnPeep;
    }

    private void OnPeep()
    {
        if(onlyOnce)
            door.OnStartPeeping -= OnPeep;
        if(routine != null)
            StopCoroutine(routine);
        door.ForceOverrideKey(null);
        routine = StartCoroutine(DoSurprise());
    }

    IEnumerator DoSurprise()
    {
        surpriseObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        surpriseObject.SetActive(false);
    }
}