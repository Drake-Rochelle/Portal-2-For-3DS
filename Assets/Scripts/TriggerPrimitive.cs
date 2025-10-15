using UnityEngine;

public class TriggerPrimitive : MonoBehaviour 
{
    [SerializeField] private int maxTriggers;
    [SerializeField] private string player;
    private bool inTrigger;
    private int triggered;
    private int exited;
    public virtual void Enter()
    {

    }
    public virtual void In()
    {

    }
    public virtual void Exit()
    {

    }
    void Update()
    {
        if (inTrigger)
        {
            In();
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == player)
        {
            if (triggered >= maxTriggers) { return; }
            triggered++;
            Enter();
            inTrigger = true;
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.name == player)
        {
            if (exited >= maxTriggers) { return; }
            exited++;
            Exit();
            inTrigger = false;
        }
    }
}
