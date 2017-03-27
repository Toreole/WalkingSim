
using UnityEngine;

public class PickUp : MonoBehaviour
{

	private GameObject hitObject;

	// Use this for initialization
	void Start ()
	{
	}

	// Update is called once per frame
	void Update()
	{

		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;

			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
			{
				//hitObject = hit.collider.gameObject;
				hitObject = hit.transform.gameObject;
				hitObject.transform.parent = gameObject.transform;

				hitObject.GetComponent<Rigidbody>().isKinematic = true;
			}

		}

		if (Input.GetMouseButtonUp(0))
		{
			hitObject.transform.parent = null;
			hitObject.GetComponent<Rigidbody>().isKinematic = false;
			hitObject = null;
		}
	}
}
