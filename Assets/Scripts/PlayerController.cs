using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    private void Awake()
    {
        instance = this;
    }

    public CharacterController charCon;

    public Transform m_transform;
    public float moveSpeed = 3.0f;
    public float m_gravity = 2.0f;

    public InputActionReference moveAction;
    public InputActionReference lookAction;
    public InputActionReference jumpAction;

    private Vector2 rotStore;
    private Vector3 currentMovement;

    public float lookSpeed = 4;
    public float jumpPower = 0.5f;

    public float minViewAngle = -85;
    public float maxViewAngle = 75;

    public float gravity = -9.81f;
    public float runSpeed = 6.0f;
    public InputActionReference sprintAction;

    public float camZoomNormal = 60f;
    public float camZoomOut = 70f;
    public float camZoomSpeed = 5f;

    public Camera cam;

    public WeaponsController weaponCon;
    public InputActionReference shootAction;
    public InputActionReference reloadAction;

    public bool isDead = false;

    public InputActionReference nextWeapon, previousWeapon;

    // Start is called before the first frame update
    void Start()
    {
        m_transform = this.m_transform;
        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update()
    {
        if(isDead)
        {
            return;
        }
        if(Time.timeScale == 0f)
        {
            return;
        }
        Control();
    }
    void OnEnable()
    {
        moveAction.action.Enable();
        lookAction.action.Enable();
        jumpAction.action.Enable();
        sprintAction.action.Enable();
        shootAction.action.Enable();
        reloadAction.action.Enable();
        nextWeapon.action.Enable();
        previousWeapon.action.Enable();
    }
    void OnDisable()
    {
        moveAction.action.Disable();
        lookAction.action.Disable();
        jumpAction.action.Disable();
        sprintAction.action.Disable();
        shootAction.action.Disable();
        reloadAction.action.Disable();
        nextWeapon.action.Disable();
        previousWeapon.action.Disable();
    }
    void Control()
    {
        float yStore = currentMovement.y;

        Vector2 moveInput = moveAction.action.ReadValue<Vector2>();
        //currentMovement = new Vector3(moveInput.x , 0f, moveInput.y );


        Vector3 moveForward = m_transform.forward * moveInput.y;
        Vector3 moveSideways = m_transform.right * moveInput.x;

        //handle sprinting
        if(sprintAction.action.IsPressed())
        {
            currentMovement = (moveForward + moveSideways) * runSpeed;
            if(currentMovement != Vector3.zero)
            {
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, camZoomOut, camZoomSpeed * Time.deltaTime);
            }
            
        }
        else
        {
            currentMovement = (moveForward + moveSideways) * moveSpeed;
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, camZoomNormal, camZoomSpeed * Time.deltaTime);
        }
            

        //handle gravity
        if(charCon.isGrounded)
        {
            yStore = 0f;
        }
        currentMovement.y = yStore + gravity * Time.deltaTime;

        //handle jumping
        if(jumpAction.action.WasPressedThisFrame() && charCon.isGrounded == true)
        {
            currentMovement.y = jumpPower;
            AudioManager.instance.PlaySFX(8);
        }


        charCon.Move(currentMovement * Time.deltaTime);


        //handle looking
        Vector2 lookInput = lookAction.action.ReadValue<Vector2>();
        lookInput.y = -lookInput.y;
        rotStore = rotStore + (lookInput * lookSpeed * Time.deltaTime);
        rotStore.y = Mathf.Clamp(rotStore.y, minViewAngle, maxViewAngle);

        m_transform.rotation = Quaternion.Euler(0f, rotStore.x, 0f);
        cam.transform.localRotation = Quaternion.Euler(rotStore.y, 0f, 0f);
       
        //handle shooting
        if(shootAction.action.WasPressedThisFrame())
        {
            weaponCon.Shoot();
        }
        if(shootAction.action.IsPressed())
        {
            weaponCon.ShootHeld();
        }

        //handling reload
        if(reloadAction.action.WasPressedThisFrame())
        {
            weaponCon.Reload();
        }

        if(nextWeapon.action.WasPressedThisFrame())
        {
            weaponCon.NextWeapon();
        }
        if (previousWeapon.action.WasPressedThisFrame())
        {
            weaponCon.PreviousWeapon();
        }
    }
}
