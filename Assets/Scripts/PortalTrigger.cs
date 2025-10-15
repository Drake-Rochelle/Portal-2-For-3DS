using UnityEngine;
public class PortalTrigger : MonoBehaviour
{
    [HideInInspector] public BoxCollider boundCollider;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform otherPortal;
    [SerializeField] private BoxCollider portalBoxCollider;
    public bool placed;
    private bool portaled;
    private Vector3 oldPos;
    void Awake()
    {
        oldPos = playerTransform.position;
    }
    void Update()
    {
        Debug.DrawLine(this.transform.position, this.transform.position+this.transform.forward);
    }
    void FixedUpdate()
    {
        portalBoxCollider.size = new Vector3(portalBoxCollider.size.x, portalBoxCollider.size.y, 0.02f + Mathf.Abs((playerTransform.position - oldPos).magnitude)*5);
        oldPos = playerTransform.position;
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Player")
        {
            if (otherPortal.gameObject.GetComponent<PortalTrigger>().placed)
            {
                boundCollider.isTrigger = true;
            }
        }
        if (col.gameObject.name == "Portal Trigger")
        {
            if (!portaled&&otherPortal.gameObject.GetComponent<PortalTrigger>().placed)
            {
                playerTransform.transform.position = otherPortal.position + (otherPortal.forward*((SphereCollider)col).radius*2.5f);

                Quaternion delta = Quaternion.FromToRotation(-this.transform.forward,otherPortal.transform.forward);

                playerTransform.rotation = Quaternion.Euler(new Vector3(0, playerTransform.rotation.eulerAngles.y + delta.eulerAngles.y, 0));
                playerTransform.gameObject.GetComponent<Rigidbody>().velocity = delta * playerTransform.gameObject.GetComponent<Rigidbody>().velocity;
                otherPortal.gameObject.GetComponent<PortalTrigger>().portaled = true;
            }
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.name == "Portal Trigger")
        {
            portaled = false;
        }
        if (col.gameObject.name == "Player")
        {
            boundCollider.isTrigger = false;
        }
    }
    public void SetBoundCollider(BoxCollider bindCollider)
    {
        boundCollider = bindCollider;
    }

}
