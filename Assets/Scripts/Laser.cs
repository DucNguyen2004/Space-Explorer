using UnityEngine;

public class Laser : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 3;
    private float lifeTimer;
    void Update()
    {
        lifeTimer += Time.deltaTime;
        if (lifeTimer > lifeTime)
        {
            Destroy(gameObject);
        }
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Asteroid"))
        {
            var asteroid = other.GetComponent<Asteroid>();
            if (asteroid != null)
            {
                asteroid.Break();

            }
            Destroy(gameObject);
        }
    }
}