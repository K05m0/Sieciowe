using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform pointA; // Punkt początkowy (A), który może mieć rodzica
    public Transform pointB; // Punkt końcowy (B), który może mieć rodzica
    public float speed = 3f; // Prędkość poruszania się

    private bool movingToB = true; // Flaga określająca, czy przeciwnik leci do punktu B
 
    public float knockbackForce = 5f; // Siła odpychania w poziomie

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

            // Dodaj siłę odpychania (wektor kierunku razy siła)
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
        }
    }

    private void Update()
    {
        // Przesuwaj przeciwnika w stronę punktu docelowego
        MoveEnemy();
    }

    private void MoveEnemy()
    {
        // Aktualizacja punktów A i B (może się zmieniać, jeśli ich parent się porusza)
        Vector3 currentPointA = pointA.localPosition;
        Vector3 currentPointB = pointB.localPosition;

        Vector3 target;
        if(movingToB)
            target = currentPointB;
        else
            target = currentPointA;

        // Przesuwaj przeciwnika w stronę punktu docelowego
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, speed * Time.deltaTime);

        // Sprawdź, czy przeciwnik dotarł do punktu docelowego
        if (Vector3.Distance(transform.localPosition, target) < 0.2f)
        {
            // Zmiana kierunku po dotarciu do punktu
            SwitchDirection(currentPointA, currentPointB);
        }
    }

    // Zmiana kierunku ruchu
    private void SwitchDirection(Vector3 currentPointA, Vector3 currentPointB)
    {
        // Zmień flagę kierunku
        movingToB = !movingToB;
    }
}
