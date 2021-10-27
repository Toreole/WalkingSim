using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupMessageTrigger : MonoBehaviour
{
    [SerializeField]
    private string message;

    private void OnTriggerEnter(Collider other) 
    {
        PopupMessage.ShowMessage(message);    
    }
}
