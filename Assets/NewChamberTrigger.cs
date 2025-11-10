using System.Collections;
using UnityEngine;

public class NewChamberTrigger : TriggerPrimitive 
{
    [SerializeField] private string chamber;
    [SerializeField] private float delay;
    public override void Enter()
    {
        StartCoroutine(Load());
    }

    private IEnumerator Load()
    {
        yield return new WaitForSeconds(delay);
        Bootloader.Instance.NextChmaber(0);
    }
}
