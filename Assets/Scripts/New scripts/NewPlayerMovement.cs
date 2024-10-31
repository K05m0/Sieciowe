using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class NewPlayerMovement : MonoBehaviour
{
    [Header("Art")]
    [SerializeField] private SpriteRenderer playerSprite;

    [Header("Movement")]
    [SerializeField] private float horizontalSpeed;
    [SerializeField] private LayerMask floorLayer;
    [SerializeField, Range(0f, 0.5f)] private float floorDetectionRange;
    [SerializeField] private float breakingSpeed;
    [SerializeField] private float resetPositionSpeed;

    private float horizontalMovement;
    private bool wasJoystickHeld = false;

    [Header("Wall Slides")]
    [SerializeField, Range(0f, 0.5f)] private float wallDetectionRange;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField, Range(0f, 1f)] private float slideMaxSpeedSlow = 0.3f;
    [SerializeField] private float wallSlideBreakeSpeed = 1f;

    private float baseSpeed;
    private float baseAcceleration;

    [Header("Stamina")]
    [SerializeField] private Slider staminaSlider;
    [SerializeField] private Image sliderFill;
    [SerializeField] private Color blinkColorA;
    [SerializeField] private Color blinkColorB;
    [SerializeField] private Color normalColor;
    [SerializeField] private float maxStamina = 5f; // Maksymalny czas trwania wall slide (w sekundach)
    [SerializeField] private float staminaUseMultiplayer = 1.5f; // Maksymalny czas trwania wall slide (w sekundach)
    [SerializeField] private float staminaRegenDelay = 2f; // Opóźnienie regeneracji staminy (w sekundach)
    [SerializeField] private float staminaRegenRate = 1f; // Szybkość regeneracji staminy (jednostki na sekundę)

    private float currentStamina; // Aktualna ilość staminy
    private bool canWallSlide = true; // Czy gracz może się ślizgać
    private bool isBlinking = false; // Czy pasek staminy miga
    private float staminaRegenTimer = 0f; // Licznik do opóźnienia regeneracji


    [Header("Dash")]
    [SerializeField] private Slider dashCDSlider;
    [SerializeField] private float dashForce = 20f;
    [SerializeField] private float dashCooldown = 5f;
    private float currDashTime;
    private bool canDash = true;

    [Header("Shoot")]
    private Vector2 shootingMovement;
    private float lastIndicatorAngle;
    [SerializeField] private GameObject shootIndicator;
    [SerializeField] private float indicatorRadius;

    [Header("References")]
    private Rigidbody rb;
    private PlayerInputActions inputActions;
    [SerializeField] private SegmentController segmentController;

    [Header("Health")]
    [SerializeField] private int maxHp;
    public int CurrHp { get; private set; }
    [SerializeField] private HealthElementController hpUiPrefab;
    [SerializeField] private Transform hpContent;
    [SerializeField] private List<HealthElementController> allHealthElement;

    public float invincibilityDuration = 2.0f;
    public float blinkInterval = 0.1f;
    private bool isInvincible = false;

    public delegate void PlayerDeath();
    public static event PlayerDeath OnPlayerDeath;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    [Header("Particle")]
    [SerializeField] private ParticleSystem particle;

    [Header("Audio")]
    [SerializeField] private AudioSource wallSlideAudio;
    [SerializeField] private AudioSource hurtPlayerAudio;
    [SerializeField] private AudioSource dashAudio;
    [SerializeField] private List<AudioClip> dashPlayerAudio = new List<AudioClip>();

    [Header("States")]
    public bool isSlide = false;
    public bool isMoving = false;
    public bool isFacingRight = false;

    private void Awake()
    {
        SetUpHealth();
        currDashTime = dashCooldown;
        currentStamina = maxStamina;
        rb = GetComponent<Rigidbody>();
        inputActions = new PlayerInputActions();
        inputActions.Horizontal.Enable();
        inputActions.Horizontal.DashButton.performed += DashButton_performed;
    }

    private void FixedUpdate()
    {
        UpdateDashCooldown();
        UpdateMovement();
        HandleHorizontalMovement();
        HandleWallSliding();
        UpdateAnimation();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            DealDmg();

        if (DetectWallOnSides(Vector3.right) && canWallSlide)
        {
            Vector3 currPos = particle.gameObject.transform.localPosition;
            if (currPos.x < 0)
            {
                currPos.x = -currPos.x;
                particle.gameObject.transform.localPosition = currPos;
            }

        }
        else if (DetectWallOnSides(Vector3.left) && canWallSlide)
        {
            Vector3 currPos = particle.gameObject.transform.localPosition;
            if (currPos.x > 0)
            {
                currPos.x = -currPos.x;
                particle.gameObject.transform.localPosition = currPos;
            }
        }


        isSlide = (DetectWallOnSides(Vector3.right) || DetectWallOnSides(Vector3.left) && canWallSlide);
        animator.SetBool("IsWallSliding", isSlide);




        MoveIndicator();
        DetectJoystickReleaseForDash();
    }

    private void UpdateDashCooldown()
    {
        currDashTime = Mathf.Clamp(currDashTime + Time.fixedDeltaTime, 0, dashCooldown);
        dashCDSlider.value = currDashTime / dashCooldown;
    }

    private void UpdateMovement()
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

    private void HandleHorizontalMovement()
    {
        if (rb.velocity.y != 0)
        {
            rb.velocity = new Vector3(rb.velocity.x, Mathf.Lerp(rb.velocity.y, 0, breakingSpeed));
            if (rb.velocity.y < 0.1 && rb.velocity.y > -0.1)
                rb.velocity = new Vector3(rb.velocity.x, 0);

        }

        if (rb.velocity.y < 0.05 && rb.velocity.y > -0.05) ;
        {
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, 0, resetPositionSpeed));
        }

        rb.AddForce(new Vector3(horizontalMovement, 0, 0) * horizontalSpeed);
        FlipSprite();
    }

    private void FlipSprite()
    {
        if (rb.velocity.x < 0 && isFacingRight && !isSlide)
            Flip();
        else if (rb.velocity.x > 0 && !isFacingRight && !isSlide)
            Flip();
    }

    private void HandleWallSliding()
    {
        staminaSlider.value = currentStamina / maxStamina;

        if (isSlide)
        {
            particle.Play();

            // Sprawdź, czy dźwięk jest już odtwarzany; jeśli nie, rozpocznij odtwarzanie
            if (!wallSlideAudio.isPlaying)
            {
                wallSlideAudio.loop = true;
                wallSlideAudio.Play();
            }

            if (currentStamina <= 0)
            {
                StartBlinkingStaminaBar();
                canWallSlide = false;
            }
            else
            {
                // Zużywaj staminę podczas wall slide
                currentStamina -= Time.deltaTime * staminaUseMultiplayer;

                // Ustawianie prędkości ślizgu po ścianie
                if (baseSpeed == 0 || baseAcceleration == 0)
                {
                    baseSpeed = segmentController.CurrSegmentSpeed;
                    baseAcceleration = segmentController.CurrAcceleration;
                }

                // Spowalnianie prędkości i przyspieszenia segmentu
                segmentController.CurrSegmentSpeed = Mathf.Lerp(segmentController.CurrSegmentSpeed, baseSpeed * slideMaxSpeedSlow, wallSlideBreakeSpeed);
            }

            if (currentStamina == maxStamina && !canWallSlide)
            {
                StopBlinkingStaminaBar();
                canWallSlide = true;
            }
        }
        else
        {
            particle.Stop();

            // Zatrzymaj dźwięk, gdy wall slide się kończy
            if (wallSlideAudio.isPlaying)
            {
                wallSlideAudio.loop = false;
                wallSlideAudio.Stop();
            }

            if (currentStamina == maxStamina)
            {
                staminaRegenTimer = 0;
            }
            else
            {
                staminaRegenTimer += Time.deltaTime;

                if (staminaRegenTimer >= staminaRegenDelay)
                {
                    currentStamina += staminaRegenRate * Time.deltaTime;
                    currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

                    if (currentStamina == maxStamina && !canWallSlide)
                    {
                        StopBlinkingStaminaBar();
                        canWallSlide = true;
                    }
                }
            }

            baseSpeed = 0;
            baseAcceleration = 0;
        }
    }

    private void UpdateAnimation()
    {
        bool isOnFloor = DetectFloor();
        animator.SetBool("Flying Up", rb.velocity.y > 0.1 && !isOnFloor);
        animator.SetBool("isFlying", rb.velocity.y <= 0.1 && !isOnFloor);
    }

    private void DashButton_performed(InputAction.CallbackContext context)
    {
        if (!canDash) return;

        currDashTime = 0.01f;
        StartCoroutine(Dash());
        PerformDash();
    }

    private void PerformDash()
    {
        Vector3 dashDirection = shootIndicator.transform.right;
        rb.velocity = Vector3.zero;
        rb.AddForce(dashDirection * dashForce, ForceMode.VelocityChange);
    }

    private void MoveIndicator()
    {
        shootingMovement = inputActions.Horizontal.ShootMovement.ReadValue<Vector2>();

        if (shootingMovement.sqrMagnitude > 0.1f)
        {
            lastIndicatorAngle = Mathf.Atan2(shootingMovement.y, shootingMovement.x) * Mathf.Rad2Deg;
        }

        Vector2 indicatorPosition = new Vector2(
            Mathf.Cos(lastIndicatorAngle * Mathf.Deg2Rad) * indicatorRadius,
            Mathf.Sin(lastIndicatorAngle * Mathf.Deg2Rad) * indicatorRadius
        );

        shootIndicator.transform.position = transform.position + (Vector3)indicatorPosition;
        shootIndicator.transform.rotation = Quaternion.Euler(0, 0, lastIndicatorAngle);
    }

    private void DetectJoystickReleaseForDash()
    {
        if (shootingMovement.sqrMagnitude > 0.1f)
        {
            wasJoystickHeld = true;
        }
        else if (wasJoystickHeld && shootingMovement == Vector2.zero)
        {
            DashButton_performed(new InputAction.CallbackContext());
            wasJoystickHeld = false;
        }
    }

    private bool DetectWallOnSides(Vector3 direction)
    {
        return Physics.Raycast(transform.position, direction, out _, wallDetectionRange + transform.localScale.x / 2, wallLayer);
    }

    private bool DetectFloor()
    {
        return Physics.Raycast(transform.position, -transform.up, out _, floorDetectionRange + transform.localScale.y / 2, floorLayer);
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
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    public void DealDmg()
    {
        if (isInvincible) return;

        allHealthElement[CurrHp - 1].ChangeElementToEmpty();
        CurrHp--;

        if (CurrHp <= 0)
        {
            Death();
        }
        else
        {
            StartCoroutine(BecomeInvincible());
        }
    }

    private void Death()
    {
        OnPlayerDeath?.Invoke();
        Destroy(gameObject);
    }

    public void SetUpHealth()
    {
        CurrHp = maxHp;
        for (int i = 0; i < maxHp; i++)
        {
            var element = Instantiate(hpUiPrefab, hpContent);
            allHealthElement.Add(element);
            element.ChangeElementToFull();
        }
    }
    private IEnumerator BecomeInvincible()
    {
        isInvincible = true;
        float timer = 0f;

        while (timer < invincibilityDuration)
        {
            playerSprite.enabled = !playerSprite.enabled;
            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval;
        }

        playerSprite.enabled = true;
        isInvincible = false;
    }
    private IEnumerator BlinkStaminaBar()
    {
        while (isBlinking)
        {
            sliderFill.color = blinkColorA;
            yield return new WaitForSeconds(blinkInterval);
            sliderFill.color = blinkColorB;
            yield return new WaitForSeconds(blinkInterval);
        }
    }

    private void StartBlinkingStaminaBar()
    {
        isBlinking = true;
        StartCoroutine(BlinkStaminaBar());
    }

    private void StopBlinkingStaminaBar()
    {
        isBlinking = false;
        sliderFill.color = normalColor;
    }

}
