using UnityEngine;

public class PickUpManager : MonoBehaviour 
{
    [SerializeField] private LayerMask physicsProps;
    private bool hold;
	private Rigidbody heldProp;
    private Vector3 pickupCamRot;
    private Vector3 pickupPropRot;
    void Update () 
	{
#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.R))
#else
		if (UnityEngine.N3DS.GamePad.GetButtonTrigger(N3dsButton.X))
#endif
        {
            if (!hold)
			{
                hold = PickUpObject();
            }
			else
			{
                hold = false;
                heldProp.useGravity = true;
                heldProp.gameObject.layer = LayerMask.NameToLayer("Physics Prop");
				heldProp = null;
            }
        }
	}
	bool PickUpObject()
	{
		RaycastHit hit;
		if(Physics.Raycast(transform.position,transform.forward,out hit, 3, physicsProps))
		{
            heldProp = hit.rigidbody;
            heldProp.useGravity = false;
			heldProp.gameObject.layer = LayerMask.NameToLayer("Held Physics Prop");
			pickupCamRot = transform.root.eulerAngles;
			pickupPropRot = heldProp.transform.rotation.eulerAngles;
			return true;
		}
		return false;
	}
	void FixedUpdate()
	{
		if (hold)
		{
			heldProp.velocity = Vector3.zero;
			heldProp.MovePosition(transform.position+(transform.forward*1.2f));
			Vector3 v = new Vector3(
                pickupPropRot.x,
                pickupPropRot.y + (transform.rotation.eulerAngles.y - pickupCamRot.x),
                pickupPropRot.z + (transform.rotation.eulerAngles.z - pickupCamRot.z)
			);
            heldProp.MoveRotation(Quaternion.Euler(v));
		}
	}
}
