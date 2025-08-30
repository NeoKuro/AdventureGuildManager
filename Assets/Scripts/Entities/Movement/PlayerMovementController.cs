using System;
using System.Threading.Tasks;

using Hzn.Framework;

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : EntityMovementController, AdventureGuildManagerControls.IPlayerActions
{
    private static AdventureGuildManagerControls _controls;

    [SerializeField]
    private float _baseMoveSpeed = 5f;

    [SerializeField]
    private float _turnSpeed = 5f;

    [SerializeField]
    private float _sprintMultiplier = 4;

    [SerializeField]
    private float _jumpForce = 5;

    private Vector2 _lastMovement = Vector2.zero;
    private Vector3 _lastLook     = Vector3.zero;
    private bool    _lastSprint   = false;
    private bool    _doJump       = false;
    private bool    _canJump      = true;

    private Camera    _mainCamera;
    private Rigidbody _rb;
    private Action    _onMoveCallback;
    private Action    _onRotateCallback;

    private void Awake()
    {
        _onMoveCallback = _onRotateCallback = null;
        _controls = new AdventureGuildManagerControls();
        _controls.Player.AddCallbacks(this);
        _mainCamera = Camera.main;
        _rb         = GetComponent<Rigidbody>();
        InputContextManager.RegisterInputContextChanged(ToggleInputTarget);
    }

    private void Start()
    {
        ToggleInputTarget(InputContextManager.PeekContext());
    }

    private void OnDestroy()
    {
        _controls.Player.RemoveCallbacks(this);
        _controls.Disable();
        InputContextManager.UnregisterInputContextChanged(ToggleInputTarget);
    }

    public void SetOnMoveCallback(Action callback)
    {
        _onMoveCallback += callback;
    }

    public void UnsetOnMoveCallback(Action callback)
    {
        _onMoveCallback -= callback;
    }

    public void SetOnRotateCallback(Action callback)
    {
        _onRotateCallback += callback;
    }

    public void UnsetOnRotateCallback(Action callback)
    {
        _onRotateCallback -= callback;
    }

    private void ToggleInputTarget(EInputContext context)
    {
        Dbg.Log(Log.Player, $"Toggling input target to {context}");
        if (context != EInputContext.Game)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible   = true;

            // if (context != EInputContext.UI)
            // {
                _controls.Disable();
            // }
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible   = false;
            _controls.Enable();
        }
    }

    protected override void DoMovement()
    {
        Vector3 movement         = new Vector3(_lastMovement.x, 0f, _lastMovement.y);
        Vector3 modifiedMovement = movement * (_baseMoveSpeed + (_lastSprint ? _sprintMultiplier : 1f));
        transform.Translate(modifiedMovement, Space.Self);
    }

    protected override void DoRotation()
    {
        Vector2 yawPitchRotation = _lastLook * _turnSpeed;

        // NOTE: This will gimbal lock a CAMERA
        //       I'm ok with this because I don't want the camera to go upside down
        //       And this prevents that 

        // Get the current rotation as Euler angles
        Vector3 currentEulerAngles = transform.eulerAngles;

        // Apply yaw (X-axis) and pitch (Y-axis) while preserving Z-axis
        currentEulerAngles.x =  0f;                 // Adjust pitch
        currentEulerAngles.y += yawPitchRotation.x; // Adjust yaw
        currentEulerAngles.z =  0f;                 // Lock Z-axis rotation to 0

        // Apply the modified rotation back to transform
        transform.eulerAngles = currentEulerAngles;

        Vector3 cameraEulerAngles = _mainCamera.transform.eulerAngles;
        cameraEulerAngles.x                    -= yawPitchRotation.y; // Adjust pitch
        cameraEulerAngles.y                    =  0f;                 // Adjust yaw
        cameraEulerAngles.z                    =  0f;                 // Lock Z-axis rotation to 0
        _mainCamera.transform.localEulerAngles =  cameraEulerAngles;
    }

    protected override void DoActions()
    {
        ProcessJump();
    }

    protected override void DoInteract()
    {
        LocalPlayerEntity.LocalPlayer.OnInteract();
    }

    private void ProcessJump()
    {
        if (!_doJump)
        {
            return;
        }

        if (!_canJump)
        {
            WaitForLand();
            return;
        }

        _canJump = false;
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.VelocityChange);
    }


    private void WaitForLand()
    {
        Ray        ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit, 0.15f))
        {
            return;
        }

        _canJump = true;
    }


    #region -- ACTION CALLBACKS --

    public void OnMovement(InputAction.CallbackContext context)
    {
        _lastMovement = context.ReadValue<Vector2>() * Time.deltaTime;
        if (_lastMovement.magnitude > 0f)
        {
            _onMoveCallback?.Invoke();
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        _lastLook = context.ReadValue<Vector2>() * Time.deltaTime;
        if (_lastLook.magnitude > 0f)
        {
            _onRotateCallback?.Invoke();
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        _lastSprint = context.performed;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        _doJump = context.performed;
        if (_doJump && _canJump)
        {
            _onMoveCallback?.Invoke();
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        DoInteract();
    }

    #endregion


}