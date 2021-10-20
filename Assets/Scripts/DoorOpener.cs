using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    [SerializeField]
    GameObject keyholeCamera;
    [SerializeField]
    PlayerMovement movement;
    [SerializeField]
    MouseLook mouseLook;
    [SerializeField]
    PickUpAndExamine examineScript;

    Door door;

    bool isPeeping = false;

    System.Collections.Generic.List<Key> keys = new System.Collections.Generic.List<Key>(3);

    private void Awake() 
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if(isPeeping)
        {
            keyholeCamera.transform.Rotate(Input.GetAxis("Mouse Y") * Time.deltaTime, Input.GetAxis("Mouse X") * Time.deltaTime, 0, Space.Self);
        }
        if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 2))
        {
            door = hit.transform.GetComponentInParent<Door>();
            if(door)
            {
                if(door.IsOpened == false)
                {
                    //start looking through the keyhole in the door
                    if(Input.GetKeyDown(KeyCode.F))
                    {
                        isPeeping = true;
                        movement.enabled = false;
                        mouseLook.enabled = false;
                        examineScript.enabled = false;
                        var keyHole = door.GetKeyHole(transform.position);
                        keyholeCamera.SetActive(true);
                        keyholeCamera.transform.AlignWith(keyHole);
                    } //stop looking
                    else if(Input.GetKeyUp(KeyCode.F))
                    {
                        isPeeping = false;
                        movement.enabled = true;
                        mouseLook.enabled = true;
                        examineScript.enabled = true;
                        keyholeCamera.SetActive(false);
                    }
                    //locking and unlocking
                    if(Input.GetKeyDown(KeyCode.E))
                    {
                        if(door.RequiredKey)
                        {
                            if(keys.Contains(door.RequiredKey))
                                door.ToggleLocked();
                            else
                                PopupMessage.ShowMessage("I need the key to unlock this.");     
                        }
                        else
                            door.ToggleLocked();
                    }
                }
                //safe open and close.
                if(isPeeping == false)
                {
                    if(Input.GetMouseButtonDown(0))
                    {
                        door.ToggleOpen();
                    }
                }
            }
            else 
            {
                var key = hit.transform.GetComponent<Collectable>();
                if(key)
                {
                    if(Input.GetMouseButtonDown(0))
                    {
                        var item = key.GetItem();
                        keys.Add(item);
                        PopupMessage.ShowMessage($"Picked up {item.name}.");
                    }
                }
            }
        }
    }
}
