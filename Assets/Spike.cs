using UnityEngine;

public class Spike : MonoBehaviour
{
    public float knockbackForce = 5f; // Siła odpychania w poziomie
    public float verticalKnockbackMultiplier = 0.5f; // Zmniejszenie siły w pionie (50%)

    private void OnTriggerEnter(Collider other)
    {
        // Sprawdź, czy obiekt, który wszedł w kolizję, to gracz
        if (other.TryGetComponent<NewPlayerMovement>(out var player))
        {
            DealDmg(player); // Zadaj obrażenia
            Knockback(player); // Odpchnij gracza
        }
    }

    // Metoda zadająca obrażenia graczowi
    private void DealDmg(NewPlayerMovement target)
    {
        target.DealDmg(); // Wywołaj metodę DealDmg z klasy NewPlayerMovement
    }

    // Metoda do odpychania gracza
    private void Knockback(NewPlayerMovement target)
    {
        Rigidbody rb = target.GetComponent<Rigidbody>(); // Pobierz Rigidbody gracza
        rb.velocity = Vector3.zero;
        if (rb != null)
        {
            // Oblicz kierunek odpychania - od środka kolca do gracza
            Vector3 knockbackDirection = (target.transform.position - transform.position).normalized;

            // Modyfikacja komponentu Y (pionowego) - zmniejsz o 50% lub wartość zdefiniowaną
            knockbackDirection.y *= verticalKnockbackMultiplier;

            // Dodaj siłę odpychania (wektor kierunku razy siła)
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
        }
    }
}
