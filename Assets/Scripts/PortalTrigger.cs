using System.Collections.Generic;
using UnityEngine;
public class PortalTrigger : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform otherPortal;
    [SerializeField] private BoxCollider portalBoxCollider;
    public List<GameObject> portalableObjects;
    public bool placed;
    private Vector3 oldPos;
    public List<bool> positive;
    public List<bool> prev;
    public List<GameObject> colls;
    Vector3 vel;
    void Awake()
    {
        oldPos = playerTransform.position;
        colls = new List<GameObject>();
    }
    void Update()
    {
        Debug.DrawLine(this.transform.position, this.transform.position+this.transform.forward);
        Debug.DrawLine(otherPortal.transform.position, otherPortal.transform.position + vel, Color.red);
    }
    void FixedUpdate()
    {
        portalBoxCollider.size = new Vector3(portalBoxCollider.size.x, portalBoxCollider.size.y, 0.02f + Mathf.Abs((playerTransform.position - oldPos).magnitude)*5);
        oldPos = playerTransform.position;
        if (positive == null)
        {
            positive = new List<bool>();
            for (int i = 0; i < portalableObjects.Count; i++)
            {
                positive.Add(true);
            }
        }
        while (positive.Count != portalableObjects.Count)
        {
            positive.Add(true);
        }
        prev = positive;
        if (positive.Count > 0)
        {
            positive = new List<bool>();
            for (int i = 0; i < portalableObjects.Count; i++)
            {
                float signedDistance = Vector3.Dot(portalableObjects[i].transform.position - transform.position, transform.forward);
                if (signedDistance > 0)
                {
                    positive.Add(true);
                }
                else
                {
                    positive.Add(false);
                }
            }
            if (otherPortal.gameObject.GetComponent<PortalTrigger>().placed)
            {
                for (int i = 0; i < portalableObjects.Count; i++)
                {
                    if (prev[i] && !positive[i])
                    {
                        if (colls.Contains(portalableObjects[i]))
                        {
                            Teleport(portalableObjects[i]);
                        }
                    }
                }
            }
        }
    }
    void OnTriggerEnter(Collider col)
    {
        if (otherPortal.gameObject.GetComponent<PortalTrigger>().placed)
        {
            col.gameObject.layer = LayerMask.NameToLayer("Portal Collider");
            GameObject go = col.gameObject;
            if (go.GetComponent<Parent>()  != null)
            {
                go = go.GetComponent<Parent>().parent;
            }
            colls.Add(go);
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.name == "Player" || col.gameObject.name == "Portal Trigger")
        {
            col.gameObject.layer = LayerMask.NameToLayer("Player");
        }
        else
        {
            col.gameObject.layer = LayerMask.NameToLayer("Default");
        }
        GameObject go = col.gameObject;
        if (go.GetComponent<Parent>() != null)
        {
            go = go.GetComponent<Parent>().parent;
        }
        colls.Remove(go);
    }
    void Teleport(GameObject go)
    {
        if (otherPortal.gameObject.GetComponent<PortalTrigger>().placed)
        {
            if (name == "Blue Portal")
            {
                go.transform.position = otherPortal.position + (otherPortal.forward * 0.1f * 2.5f);
            }
            else
            {
                go.transform.position = otherPortal.position + (otherPortal.forward * 0.1f * 2.5f);
            }

            Quaternion delta = Quaternion.FromToRotation(-this.transform.forward, otherPortal.transform.forward);

            if (otherPortal.transform.rotation.eulerAngles.x != 0 || this.transform.rotation.eulerAngles.x != 0)
            {
                go.transform.rotation = Quaternion.Euler(new Vector3(go.transform.rotation.eulerAngles.x + delta.eulerAngles.x, go.transform.rotation.eulerAngles.y + delta.eulerAngles.y + 180, go.transform.rotation.eulerAngles.z + delta.eulerAngles.z));
            }
            else
            {
                go.transform.rotation = Quaternion.Euler(new Vector3(go.transform.rotation.eulerAngles.x + delta.eulerAngles.x, go.transform.rotation.eulerAngles.y + delta.eulerAngles.y, go.transform.rotation.eulerAngles.z + delta.eulerAngles.z));
            }
            go.GetComponent<Rigidbody>().velocity = delta * go.GetComponent<Rigidbody>().velocity;
            vel = go.GetComponent<Rigidbody>().velocity;
        }
    }
}
