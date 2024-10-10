using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class NewPlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float horizontalSpeed;
    private float horizontalMovement;

    [Header("Shoot")]
    private Vector2 shootingMovenet;
    private float lastIndicatorAngle;
    [SerializeField] private GameObject shootIndicator;
    [SerializeField] private float indicatorRadius;

    private Rigidbody rb;
    private PlayerInput playerInput;
    PlayerInputActions inputActions;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();

        inputActions = new PlayerInputActions();
        inputActions.Horizontal.Enable();
    }
    private void FixedUpdate()
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
        rb.AddForce(new Vector3(horizontalMovement, 0, 0) * horizontalSpeed);
    }

    private void Update()
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
        Debug.DrawRay(shootIndicator.transform.position, shootIndicator.transform.forward);

        Debug.Log(angle);
    }
}
