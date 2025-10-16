using UnityEngine;

public class PlayerController3D : MonoBehaviour 
{
    [SerializeField] private Transform cam;
    [SerializeField] private float accelSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float lookSpeed;
    [SerializeField] private float raycastDistance;
    [SerializeField] private float jumpForce;
    [SerializeField] private float friction;
    private Rigidbody rb;
    private Transform player;
    private float angle;
    private bool grounded;
    public static PlayerController3D Instance
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
        player = transform;
        rb = GetComponent<Rigidbody>();
    }
	void Update () 
	{
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Escape))
#else
        if (UnityEngine.N3DS.GamePad.GetButtonTrigger(N3dsButton.Start))
#endif
        {
            TransitionManager.Instance.Scene("Main Menu");
        }
#if UNITY_EDITOR
        angle = cam.rotation.eulerAngles.x;
        if (angle > 180)
        {
            angle = angle - 360;
        }
        if (angle - (Input.GetAxis("Mouse Y") * lookSpeed * 2) < -90)
        {
            cam.rotation = Quaternion.Euler(-90f, cam.rotation.eulerAngles.y, cam.rotation.eulerAngles.z);
        }
        else if (angle - (Input.GetAxis("Mouse Y") * lookSpeed * 2) > 90)
        {
            cam.rotation = Quaternion.Euler(90f, cam.rotation.eulerAngles.y, cam.rotation.eulerAngles.z);
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                player.rotation = Quaternion.Euler(
                    new Vector3(
                        player.rotation.eulerAngles.x,
                        player.rotation.eulerAngles.y + (Input.GetAxis("Mouse X") * lookSpeed * 2),
                        player.rotation.eulerAngles.z
                        )
                    );
                cam.rotation = Quaternion.Euler(
                    new Vector3(
                        cam.rotation.eulerAngles.x - (Input.GetAxis("Mouse Y") * lookSpeed * 2),
                        cam.rotation.eulerAngles.y,
                        cam.rotation.eulerAngles.z
                        )
                    );
            }
        }
#else
        
        if(Input.touchCount != 0)
        {
            angle = cam.rotation.eulerAngles.x;
            if (angle > 180)
            {
                angle = angle - 360;
            }
            if (angle - (Input.GetTouch(0).deltaPosition.y * lookSpeed / 5) < -90)
            {
                cam.rotation = Quaternion.Euler(-90f, cam.rotation.eulerAngles.y, cam.rotation.eulerAngles.z);
            }
            else if (angle - (Input.GetTouch(0).deltaPosition.y * lookSpeed / 5) > 90)
            {
                cam.rotation = Quaternion.Euler(90f, cam.rotation.eulerAngles.y, cam.rotation.eulerAngles.z);
            }
            else
            {
                if(Input.GetTouch(0).position != new Vector2(0, 0))
                {
                    player.rotation = Quaternion.Euler(
                        new Vector3(
                            player.rotation.eulerAngles.x,
                            player.rotation.eulerAngles.y + (Input.GetTouch(0).deltaPosition.x * lookSpeed / 5),
                            player.rotation.eulerAngles.z
                            )
                        );
                    cam.rotation = Quaternion.Euler(
                        new Vector3(
                            cam.rotation.eulerAngles.x - (Input.GetTouch(0).deltaPosition.y * lookSpeed / 5),
                            cam.rotation.eulerAngles.y,
                            cam.rotation.eulerAngles.z
                            )
                        );
                }
            }
        }
#endif

        float playerAngle = UnityEngine.N3DS.GamePad.CirclePadPro.x * lookSpeed;
        float camAngle = UnityEngine.N3DS.GamePad.CirclePadPro.y * lookSpeed;
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.RightArrow))
        {
            playerAngle += lookSpeed/5;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            playerAngle -= lookSpeed / 5;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            camAngle += lookSpeed / 5;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            camAngle -= lookSpeed / 5;
        }
#endif
        rb.freezeRotation = false;
        player.rotation = Quaternion.Euler(player.rotation.eulerAngles.x, player.rotation.eulerAngles.y + playerAngle, player.rotation.eulerAngles.z);
        rb.freezeRotation = true;
        angle = cam.rotation.eulerAngles.x;
        if (angle > 180)
        {
            angle = angle - 360;
        }
        if (angle - camAngle < -90)
        {
            cam.rotation = Quaternion.Euler(-90f, cam.rotation.eulerAngles.y, cam.rotation.eulerAngles.z);
        }
        else if (angle - camAngle > 90)
        {
            cam.rotation = Quaternion.Euler(90f, cam.rotation.eulerAngles.y, cam.rotation.eulerAngles.z);
        }
        else
        {
            cam.rotation = Quaternion.Euler(cam.rotation.eulerAngles.x - camAngle, cam.rotation.eulerAngles.y, cam.rotation.eulerAngles.z);
        }
        Vector2 horVel = new Vector2(rb.velocity.x, rb.velocity.z);
        float forwardForce = UnityEngine.N3DS.GamePad.CirclePad.y;
        float sidewaysForce = UnityEngine.N3DS.GamePad.CirclePad.x;
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.W))
        {
            forwardForce += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            forwardForce -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            sidewaysForce += 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            sidewaysForce -= 1;
        }
#endif
        if (forwardForce!=0 || sidewaysForce != 0)
        {
            PortalGunAnimator.Instance.animate = true;
        }
        else
        {
            PortalGunAnimator.Instance.animate = false;
        }
        if (horVel.magnitude < maxSpeed)
        {
            rb.velocity += transform.forward * forwardForce * accelSpeed;
            rb.velocity += transform.right * sidewaysForce * accelSpeed;
        }
        else
        {
            horVel = horVel.normalized * maxSpeed;
            rb.velocity = new Vector3(horVel.x, rb.velocity.y, horVel.y);
        }
        RaycastHit hit;
        if ((sidewaysForce == 0 && forwardForce == 0) && grounded)
        {
            rb.velocity = new Vector3(rb.velocity.x * (1 / friction), rb.velocity.y, rb.velocity.z * (1 / friction));
        }
        grounded = false;
        if (Physics.SphereCast(transform.position, gameObject.GetComponent<CapsuleCollider>().radius-0.05f, new Vector3(0,-1,0), out hit, raycastDistance))
        {
            grounded = true;
            if (UnityEngine.N3DS.GamePad.GetButtonTrigger(N3dsButton.A) || Input.GetKeyDown(KeyCode.Space)) 
            {
                rb.AddForce(new Vector3(0,jumpForce,0),ForceMode.Impulse);
            }
        }
    }
}
