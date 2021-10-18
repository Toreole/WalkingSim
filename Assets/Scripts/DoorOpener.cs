using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 2))
            {
                var door = hit.transform.GetComponentInParent<Door>();
                if(door)
                {
                    door.ToggleOpen();
                }
            }
        }
    }
}
