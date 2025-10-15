using System.Collections.Generic;
using UnityEngine;

public class PlayerPortalManager : MonoBehaviour 
{
    [SerializeField] private LayerMask portalableMask;
    [SerializeField] private Transform cam;
    [SerializeField] private GameObject[] portals;
    [SerializeField] private float offset;
    [SerializeField] private PortalTrigger[] portalScripts;
    void Update () 
	{
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.E))
#else
        if (UnityEngine.N3DS.GamePad.GetButtonTrigger(N3dsButton.R))
#endif
        {
            if (ShootPortal(1))
            {
                int curr = CrosshairManager.Instance.crosshair;
                if (curr == 0) { curr = 1; }
                else if (curr == 1) { return; }
                else if (curr == 2) { curr = 3; }
                else { return; }
                CrosshairManager.Instance.crosshair = curr;
                CrosshairManager.Instance.update = true;
            }
        }
#if UNITY_EDITOR
        else if (Input.GetKeyDown(KeyCode.Q))
#else
        else if (UnityEngine.N3DS.GamePad.GetButtonTrigger(N3dsButton.L))
#endif
        {
            if (ShootPortal(0))
            {
                int curr = CrosshairManager.Instance.crosshair;
                if (curr == 0) { curr = 2; }
                else if (curr == 1) { curr = 3; }
                else { return; }
                CrosshairManager.Instance.crosshair = curr;
                CrosshairManager.Instance.update = true;
            }
        }
    }
    bool ShootPortal(int color)
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, 200, portalableMask))
        {
            portals[color].transform.position = hit.point+hit.normal*offset;
            portals[color].transform.rotation = Quaternion.LookRotation(hit.normal);
            portalScripts[color].SetBoundCollider((BoxCollider)hit.collider);
            portalScripts[color].placed = true;
            PortalGunAnimator.Instance.color = color+1;
            return true;
        }
        return false;
    }
}
