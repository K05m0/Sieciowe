using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : StaminaBar
{
    public Vector3Value positionValue;
    public Camera camera2;

    public Animator animator;
    public float maxSpeed;
    public Rigidbody rb;
    private float horizontal;
    public float lastYPosition;
    public Canvas canvas;
    public int DashCost;
    public int JumpCost;
    public int ShootingCost;
    public float HP = 3;
    public int ammoCount = 999;
    private bool isFacingRight = true;
    public float wallSlideMinX;
    public float wallSlideMaxX;
    private bool isFlying = false;


    [SerializeField] private SpriteRenderer playerTestSprite;
    [SerializeField] private Transform PlayerCamera;
    [SerializeField] private float MovingSpeed;
    [SerializeField] private float JumpForce;
    [SerializeField] private float DashForce;
    [SerializeField] private float Velocity;

    private Vector3 dashDirection;

    public GameObject bullet;
    public Transform bulletTransform;
    public bool canFire;
    private float timer;
    public float timeBetweenFiring;

    public GameEvent onPlayerdamage;
    public playerhealth Hp;
    public GameEvent onPlayerdeath;

    public ParticleSystem mainBurst;
    public ParticleSystem secondaryBurst;

    private void Start()
    {
        camera2 = Camera.main;
        mainBurst.Stop();
        secondaryBurst.Stop();
    }

    private void Update()
    {
        positionValue.position = transform.position;
        Vector2 positionOnScreen = (Vector2)Camera.main.WorldToViewportPoint(transform.position);
        Vector2 mouseOnScreen = (Vector2)camera2.ScreenToViewportPoint(Input.mousePosition);
        Vector2 mouseOnScreenScaled = positionOnScreen - mouseOnScreen;

        

        if (transform.position.x < wallSlideMinX || transform.position.x > wallSlideMaxX)
        {
            animator.SetBool("IsWallSliding", true);
        }
        else
        {
            animator.SetBool("IsWallSliding", false);
        }
        
        animator.SetBool("Flying Up", rb.velocity.y > 0f);

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (staminaBar.value >= DashCost)
            {
                rb.velocity = Vector3.zero;
                DashToMousePosition();
                StaminaBar.instance.UseStamina(DashCost);
            }
        }

        if (!canFire)
        {
            timer += Time.deltaTime;
            if (timer > timeBetweenFiring)
            {
                canFire = true;
                timer = 0;
            }
        }

        if (Input.GetMouseButton(0) && canFire && ammoCount > 0 && staminaBar.value >= DashCost)
        {
            canFire = false;
            Instantiate(bullet, bulletTransform.position, Quaternion.identity);
            ammoCount--;
            StaminaBar.instance.UseStamina(ShootingCost);
            Debug.Log("Ammo Count: " + ammoCount);
        }

        horizontal = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(horizontal * MovingSpeed, rb.velocity.y);

        animator.SetFloat("Speed", Mathf.Abs(horizontal));
        
        isFlying = rb.velocity.y !=0;
        animator.SetBool("isFlying", isFlying);

        if (horizontal < 0 && !isFacingRight)
        {
            Flip();
        }
        else if (horizontal > 0 && isFacingRight)
        {
            Flip();
        }

        if (Input.GetKeyDown(KeyCode.Space) && staminaBar.value >= JumpCost)
        {
            PlayerJump();
        }

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }

    }
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = playerTestSprite.transform.localScale;
        scale.x *= -1;
        playerTestSprite.transform.localScale = scale;

    }

    private void DashToMousePosition()
    {
        Vector3 mousePosition = GetMouseWorldPosition();
        dashDirection = (mousePosition - transform.position).normalized;
        rb.AddForce(dashDirection * DashForce, ForceMode.VelocityChange);
    }

    private void PlayerJump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // Reset vertical velocity to 0
        Vector3 jumpVector = new Vector3(0, JumpForce, 0);
        rb.AddForce(jumpVector);
        StaminaBar.instance.UseStamina(JumpCost);

        ActivateBurst();
    }

    private void ActivateBurst()
    {
        mainBurst.Play();
        secondaryBurst.Play();
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -camera2.transform.position.z;
        return camera2.ScreenToWorldPoint(mousePosition);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "enemy" || collision.gameObject.tag == "Spikes")
        {
            Damage();
        }
       

    }



    private void Damage()
    {
        Hp.value -= 1;
        if (onPlayerdamage != null)
            onPlayerdamage.Fire();
        camera2.transform.DOShakePosition(1f);
    }
}
