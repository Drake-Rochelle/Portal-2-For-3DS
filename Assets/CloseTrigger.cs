using UnityEngine;

public class CloseTrigger : TriggerPrimitive 
{
    [SerializeField] private ChamberlockDoor door;
    public override void Enter()
    {
        door.Close();
    }
}
