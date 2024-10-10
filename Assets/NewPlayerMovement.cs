using System;
using System.Collections;
using System.Collections.Generic;
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

    [Header("WallSlides")]
    [SerializeField, Range(0f, 0.5f)] private float wallDetectionRange;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float baseWallSlideSpeed = 3f;
    [SerializeField] private float endWallSlideSpeed = 0.3f;
    private float currentWallSlideSpeed;

    [Header("Dash")]
    [SerializeField] private float dashForce = 20;
    [SerializeField] private float dashCooldown = 5;
    private bool canDash = true;

    [Header("Shoot")]
    private Vector2 shootingMovenet;
    private float lastIndicatorAngle;
    [SerializeField] private GameObject shootIndicator;
    [SerializeField] private float indicatorRadius;

    [Header("Reference")]
    private Rigidbody rb;
    private PlayerInput playerInput;
    PlayerInputActions inputActions;

    [Header("States")]
    public bool isSlide = false;
    public bool isMoving = false;
    public bool isFacingRight = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();

        inputActions = new PlayerInputActions();
        inputActions.Horizontal.Enable();
        inputActions.Horizontal.DashButton.performed += DashButton_performed;
        inputActions.Horizontal.ShootButton.performed += ShootButton_performed;

    }

    private void ShootButton_performed(InputAction.CallbackContext context)
    {
        Debug.Log("Shoot");
    }

    private void DashButton_performed(InputAction.CallbackContext obj)
    {
        Debug.Log("Dash");
        if (!canDash)
            return;

        StartCoroutine(Dash());
        var dashDirection = shootIndicator.transform.right;
        rb.AddForce(dashDirection * dashForce, ForceMode.VelocityChange);
    }

    private void FixedUpdate()
    {
        MovementInput();

        if(isSlide)
        {
            WallSlide();
        }

        rb.AddForce(new Vector3(horizontalMovement, 0, 0) * horizontalSpeed);

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
        if (DetectWallOnSides(transform.right) || DetectWallOnSides(-transform.right))
        {
            isSlide = true;
        }
        else if(!DetectWallOnSides(transform.right) && !DetectWallOnSides(-transform.right))
        {
            isSlide = false;
            currentWallSlideSpeed = 0;
        }

        MoveIndicator();
    }
    private void WallSlide()
    {
        if(currentWallSlideSpeed == 0)
        {
            currentWallSlideSpeed = baseWallSlideSpeed;
        }
        else
        {
            currentWallSlideSpeed = Mathf.Lerp(currentWallSlideSpeed, endWallSlideSpeed, Time.deltaTime);
        }
        rb.velocity = new Vector3(horizontalMovement, -currentWallSlideSpeed, 0);
    }
    private void MoveIndicator()
    {
        shootingMovenet = inputActions.Horizontal.ShootMovement.ReadValue<Vector2>();

        if (shootingMovenet.sqrMagnitude > 0.1f)
        {
            // Oblicz kąt na podstawie joysticka
            lastIndicatorAngle = Mathf.Atan2(shootingMovenet.y, shootingMovenet.x) * Mathf.Rad2Deg;
        }

        var angle = Mathf.Atan2(shootingMovenet.y, shootingMovenet.x) * Mathf.Rad2Deg;
        Vector2 indicatorPosition = new Vector2(
            Mathf.Cos(angle * Mathf.Deg2Rad) * indicatorRadius,
            Mathf.Sin(angle * Mathf.Deg2Rad) * indicatorRadius
            );

        shootIndicator.transform.position = transform.position + (Vector3)indicatorPosition;
        shootIndicator.transform.rotation = Quaternion.Euler(0, 0, lastIndicatorAngle);
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
        if (Physics.Raycast(transform.position, direction, out var hit, distance, wallLayer))
        {
            Debug.Log("Wall Detected");
            Debug.DrawRay(transform.position, direction * Vector3.Distance(transform.position, hit.point), Color.red);
            return true;
        }
        else
        {
            Debug.Log("Wall notDetected");

            Debug.DrawRay(transform.position, direction * distance, Color.blue);
            return false;
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = playerSprite.transform.localScale;
        scale.x *= -1;
        playerSprite.transform.localScale = scale;

    }

    IEnumerator Dash()
    {
        canDash = false;
        yield return new WaitForSeconds(1);
        canDash = true;
    }
}
