using UnityEngine;

public class Spike : MonoBehaviour
{
    [Header("Knockback Settings")]
    public float knockbackForce = 5f;
    public float verticalKnockbackMultiplier = 0.5f;

    [Header("Movement Settings")]
    public bool moveUp = false;
    public bool moveDown = false;
    public float moveSpeed = 2f;
    public float moveRange = 3f;
    private Vector3 initialPosition;

    private void Start()
    {
        // Zapisz początkową pozycję obiektu
        initialPosition = transform.position;
    }

    private void Update()
    {
        if (moveUp)
        {
            MoveUp();
        }
        else if (moveDown)
        {
            MoveDown();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<NewPlayerMovement>(out var player))
        {
            DealDmg(player);
            Knockback(player);
        }
    }

    private void DealDmg(NewPlayerMovement target)
    {
        target.DealDmg();
    }

    private void Knockback(NewPlayerMovement target)
    {
        Rigidbody rb = target.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        if (rb != null)
        {
            Vector3 knockbackDirection = (target.transform.position - transform.position).normalized;

            knockbackDirection.y *= verticalKnockbackMultiplier;

            rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
        }
    }

    // Metoda odpowiedzialna za ruch w górę
    private void MoveUp()
    {
        if (transform.position.y < initialPosition.y + moveRange)
        {
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        }
        else
        {
            Destroy(gameObject); // Zniszcz obiekt, gdy osiągnie górny punkt końcowy
        }
    }

    // Metoda odpowiedzialna za ruch w dół
    private void MoveDown()
    {
        if (transform.position.y > initialPosition.y - moveRange)
        {
            transform.position += Vector3.down * moveSpeed * Time.deltaTime;
        }
        else
        {
            Destroy(gameObject); // Zniszcz obiekt, gdy osiągnie dolny punkt końcowy
        }
    }
}
