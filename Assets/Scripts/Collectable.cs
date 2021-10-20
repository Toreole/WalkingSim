using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField]
    private Key key;

    public Key GetItem()
    {
        gameObject.SetActive(false);
        return key;
    }
}
