using UnityEngine;
public class PortalTrigger : MonoBehaviour
{
    [HideInInspector] public BoxCollider boundCollider;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform otherPortal;
    [SerializeField] private int maxTriggers;
    [SerializeField] private string player;
    [SerializeField] private BoxCollider portalBoxCollider;
    private int triggered;
    private int exited;
    public bool placed;
    private bool portaled;
    private Vector3 oldPos;
    void Awake()
    {
        oldPos = playerTransform.position;
    }
    void FixedUpdate()
    {
        portalBoxCollider.size = new Vector3(portalBoxCollider.size.x, portalBoxCollider.size.y, 0.02f + Mathf.Abs((playerTransform.position - oldPos).magnitude)*5);
        oldPos = playerTransform.position;
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == player)
        {
            if (triggered >= maxTriggers) { return; }
            triggered++;
            if (otherPortal.gameObject.GetComponent<PortalTrigger>().placed)
            {
                boundCollider.isTrigger = true;
            }
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.name == player)
        {
            if (exited >= maxTriggers) { return; }
            exited++;
            if (!portaled && otherPortal.gameObject.GetComponent<PortalTrigger>().placed)
            {
                playerTransform.transform.position = otherPortal.position;
                playerTransform.transform.rotation = otherPortal.rotation;
                playerTransform.transform.rotation = Quaternion.Euler(0, playerTransform.transform.rotation.y, 0);
                otherPortal.gameObject.GetComponent<PortalTrigger>().portaled = true;
            }
            portaled = false;
            boundCollider.isTrigger = false;
        }
    }
    public void SetBoundCollider(BoxCollider bindCollider)
    {
        boundCollider = bindCollider;
    }

}
