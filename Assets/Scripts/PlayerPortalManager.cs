using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPortalManager : MonoBehaviour 
{
    [SerializeField] private LayerMask portalableMask;
    [SerializeField] private LayerMask[] raycastMasks;
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
                PortalGunAnimator.Instance.color = 1;
                PortalGunAnimator.Instance.update = true;
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
                PortalGunAnimator.Instance.color = 0;
                PortalGunAnimator.Instance.update = true;
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
        if (Physics.Raycast(cam.position, cam.forward, out hit, 200, raycastMasks[color]))
        {
            bool portalable = false;
            portalable = (portalableMask.value & (1 << hit.transform.gameObject.layer)) != 0;
            if (portalable)
            {
                portals[color].transform.position = hit.point + hit.normal * offset;
                portals[color].transform.rotation = Quaternion.LookRotation(hit.normal);
                portalScripts[color].placed = true;
                return true;
            }
        }
        return false;
    }
}
