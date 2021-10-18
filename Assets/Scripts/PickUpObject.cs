
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PickUpObject : MonoBehaviour
{
	public bool adjustObject = false;
	public bool rotateHorizontal = false;
	public bool rotateVertical = false;

	private Rigidbody body;

	public Rigidbody Rigidbody => body;

	// Use this for initialization
	void Start ()
	{
		body = GetComponent<Rigidbody>();
	}
}