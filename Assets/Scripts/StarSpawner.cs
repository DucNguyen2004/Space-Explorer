using System.Collections.Generic;
using UnityEngine;

public class StarSpawner : MonoBehaviour
{
    [SerializeField] private Star starPrefab;
    [SerializeField] private int numberOfStars = 10;
    [SerializeField] private float minDistanceBetweenStars = 1.5f;

    private Camera _mainCamera;
    private List<Vector2> starPositions = new List<Vector2>();

    private void Start()
    {
        _mainCamera = Camera.main;
        SpawnStars();
    }

    private void SpawnStars()
    {
        for (int i = 0; i < numberOfStars; i++)
        {
            Vector2 position = GetValidSpawnPosition();
            var star = Instantiate(starPrefab, position, Quaternion.identity);
            star.Setup(this);
            starPositions.Add(position);
        }
    }

    public void RespawnStar()
    {
        Vector2 position = GetValidSpawnPosition();
        var star = Instantiate(starPrefab, position, Quaternion.identity);
        star.Setup(this);
        starPositions.Add(position);
    }

    private Vector2 GetValidSpawnPosition()
    {
        int maxAttempts = 20;
        Vector2 newPosition;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            newPosition = GetRandomPositionInView();

            if (IsPositionValid(newPosition))
            {
                return newPosition;
            }
        }

        return GetRandomPositionInView();
    }

    private Vector2 GetRandomPositionInView()
    {
        float _screenBoundaryY = _mainCamera.orthographicSize;
        float _screenBoundaryX = _mainCamera.orthographicSize * _mainCamera.aspect;

        float x = Random.Range(-_screenBoundaryX, _screenBoundaryX);
        float y = Random.Range(-_screenBoundaryY, _screenBoundaryY);

        return (Vector2)_mainCamera.transform.position + new Vector2(x, y);
    }

    private bool IsPositionValid(Vector2 position)
    {
        foreach (Vector2 existingPosition in starPositions)
        {
            if (Vector2.Distance(position, existingPosition) < minDistanceBetweenStars)
            {
                return false;
            }
        }
        return true;
    }
}
