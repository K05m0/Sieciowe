using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class NewPlayerMovement : MonoBehaviour
{
    [Header("art")]
    [SerializeField] private SpriteRenderer playerSprite;

    [Header("Movement")]
    [SerializeField] private float horizontalSpeed;
    private float horizontalMovement;
    private bool wasJoystickHeld = false;

    [Header("WallSlides")]
    [SerializeField, Range(0f, 0.5f)] private float wallDetectionRange;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float baseWallSlideSpeed = 3f;
    [SerializeField] private float endWallSlideSpeed = 0.3f;
    [SerializeField] private float wallSideSpeed = 1f;
    private float currentWallSlideSpeed;

    private float baseSpeed;
    private float baseAcceleration;

    // Original acceleration to restore later
    private float originalAcceleration;

    [Header("Dash")]
    [SerializeField] private Slider dashCDSlider;
    [SerializeField] private float dashForce = 20;
    [SerializeField] private float dashCooldown = 5;
    private float currDashTime;
    private bool canDash = true;

    [Header("Shoot")]
    private Vector2 shootingMovement;
    private float lastIndicatorAngle;
    [SerializeField] private GameObject shootIndicator;
    [SerializeField] private float indicatorRadius;

    [Header("Reference")]
    private Rigidbody rb;
    private PlayerInput playerInput;
    private PlayerInputActions inputActions;
    [SerializeField] private SegmentController segmentController;

    [Header("States")]
    public bool isSlide = false;
    public bool isMoving = false;
    public bool isFacingRight = false;

    private void Awake()
    {

        currDashTime = dashCooldown;

        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();

        inputActions = new PlayerInputActions();
        inputActions.Horizontal.Enable();
        inputActions.Horizontal.DashButton.performed += DashButton_performed;
        inputActions.Horizontal.ShootButton.performed += ShootButton_performed;

        // Store the original acceleration
        originalAcceleration = segmentController.CurrAcceleration;
    }

    private void FixedUpdate()
    {
        currDashTime += Time.fixedDeltaTime;
        currDashTime = Mathf.Clamp(currDashTime,0,dashCooldown);

        dashCDSlider.value = currDashTime/dashCooldown;
        Debug.Log(dashCooldown / currDashTime);

        MovementInput();

        if (isSlide)
        {
            WallSlide();
        }
        else
        {
            if (baseSpeed != 0 || baseAcceleration != 0)
            {
                baseSpeed = 0;
                baseAcceleration = 0;
            }
            segmentController.CurrAcceleration = segmentController.BaseAcceleration;
        }

        // Add horizontal force
        rb.AddForce(new Vector3(horizontalMovement, 0, 0) * horizontalSpeed);

        // Flip player sprite based on direction
        if (horizontalMovement < 0 && !isFacingRight)
        {
            Flip();
        }
        else if (horizontalMovement > 0 && isFacingRight)
        {
            Flip();
        }
    }

    private void Update()
    {
        // Detect walls on sides
        isSlide = DetectWallOnSides(transform.right) || DetectWallOnSides(-transform.right);

        if (!isSlide)
        {
            currentWallSlideSpeed = 0;

        }

        MoveIndicator();

        // Detect joystick release for dash
        DetectJoystickReleaseForDash();
    }

    private void ShootButton_performed(InputAction.CallbackContext context)
    {
        Debug.Log("Shoot");
    }

    private void DashButton_performed(InputAction.CallbackContext obj)
    {
        if (!canDash)
            return;

        Debug.Log("Dash");
        currDashTime = 0.01f;
        StartCoroutine(Dash());
        var dashDirection = shootIndicator.transform.right;
        rb.velocity = Vector3.zero;
        rb.AddForce(dashDirection * dashForce, ForceMode.VelocityChange);
    }

    private void WallSlide()
    {
        if(baseSpeed == 0 || baseAcceleration == 0)
        {
            baseSpeed = segmentController.CurrSegmentSpeed;
            baseAcceleration = segmentController.CurrAcceleration;
        }

        // Reduce segment speed and acceleration when sliding
        segmentController.CurrSegmentSpeed = Mathf.Max(baseSpeed * 0.5f, segmentController.CurrSegmentSpeed * Time.deltaTime * 6); // Reduce speed
        segmentController.CurrAcceleration = Mathf.Max(baseAcceleration * 0.2f, segmentController.CurrAcceleration * Time.deltaTime * 6); // Reduce acceleration
    }

    private void MoveIndicator()
    {
        shootingMovement = inputActions.Horizontal.ShootMovement.ReadValue<Vector2>();

        if (shootingMovement.sqrMagnitude > 0.1f)
        {
            lastIndicatorAngle = Mathf.Atan2(shootingMovement.y, shootingMovement.x) * Mathf.Rad2Deg;
        }

        var angle = Mathf.Atan2(shootingMovement.y, shootingMovement.x) * Mathf.Rad2Deg;
        Vector2 indicatorPosition = new Vector2(
            Mathf.Cos(angle * Mathf.Deg2Rad) * indicatorRadius,
            Mathf.Sin(angle * Mathf.Deg2Rad) * indicatorRadius
        );

        shootIndicator.transform.position = transform.position + (Vector3)indicatorPosition;
        shootIndicator.transform.rotation = Quaternion.Euler(0, 0, lastIndicatorAngle);
    }

    private void DetectJoystickReleaseForDash()
    {
        if (shootingMovement.sqrMagnitude > 0.1f)
        {
            // Joystick is held
            wasJoystickHeld = true;
        }
        else if (wasJoystickHeld && shootingMovement == Vector2.zero)
        {
            // Joystick was held but released
            Debug.Log("Joystick released, performing dash");
            DashButton_performed(new InputAction.CallbackContext()); // Invoke dash

            // Restore original acceleration
            segmentController.CurrAcceleration = originalAcceleration;

            // Reset the flag
            wasJoystickHeld = false;
        }
    }

    private void MovementInput()
    {
        try
        {
            horizontalMovement = inputActions.Horizontal.HorizontalMove.ReadValue<float>();
        }
        catch
        {
            try
            {
                horizontalMovement = inputActions.Horizontal.HorizontalMove.ReadValue<Vector2>().x;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }

    private bool DetectWallOnSides(Vector3 direction)
    {
        var distance = wallDetectionRange + transform.localScale.x / 2;
        return Physics.Raycast(transform.position, direction, out var hit, distance, wallLayer);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = playerSprite.transform.localScale;
        scale.x *= -1;
        playerSprite.transform.localScale = scale;
    }

    private IEnumerator Dash()
    {
        canDash = false;
        yield return new WaitForSeconds(dashCooldown);  // Set dash cooldown time
        canDash = true;
    }
}
