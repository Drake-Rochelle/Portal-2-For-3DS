using UnityEngine;

public class PlayerSphereCollider : MonoBehaviour 
{
    [HideInInspector] public bool colliding;
    public static PlayerSphereCollider Instance 
    { 
        get; private set; 
    }
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("More than one " + this.name + ", ya chump");
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        colliding = true;
    }
    void OnCollisionExit(Collision col)
    {
        colliding = false;
    }
    void Update()
    {
        Debug.Log(colliding);
    }
}
