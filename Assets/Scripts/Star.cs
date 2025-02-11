using UnityEngine;

public class Star : MonoBehaviour
{
    private StarSpawner _starSpawner;
    public void Setup(StarSpawner spawner)
    {
        _starSpawner = spawner;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameController.Instance.AddScore(1);
            _starSpawner.RespawnStar();
            Destroy(gameObject);
        }
    }
}
