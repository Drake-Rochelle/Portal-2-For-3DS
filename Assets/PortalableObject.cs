using UnityEngine;

public class PortalableObject : MonoBehaviour
{
    [SerializeField] private float speed;
    private PortalTrigger[] a;
    void Start()
    {
        a = FindObjectsOfType<PortalTrigger>();
        for (int i = 0; i < a.Length; i++)
        {
            a[i].portalableObjects.Add(gameObject);
        }
    }
    void Update()
    {
        if (gameObject.name == "Player")
        {
            Vector3 targetEuler = new Vector3();
            //targetEuler.x = Mathf.MoveTowardsAngle(transform.rotation.eulerAngles.x, 0f, speed * Time.deltaTime);
            targetEuler.x = 0;
            targetEuler.y = transform.rotation.eulerAngles.y;
            //targetEuler.z = Mathf.MoveTowardsAngle(transform.rotation.eulerAngles.z, 0f, speed * Time.deltaTime);
            targetEuler.z = 0;

            transform.rotation = Quaternion.Euler(targetEuler);
        }
    }
}
