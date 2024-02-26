using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireworkImageSpawner : MonoBehaviour
{
    public RectTransform parentTransform; // The parent transform to spawn images within
    public GameObject imagePrefab; // Prefab of the image to spawn
    public float minY; // Minimum y position
    public float maxY; // Maximum y position
    public float spawnInterval = 3f; // Time between spawns
    public float destroyDelay = 1f; // Time before destroying spawned image

    private void Start()
    {
        // Invoke the SpawnImage method repeatedly with the specified spawn interval
        InvokeRepeating("SpawnImage", 0f, spawnInterval);
    }

    private void SpawnImage()
    {
        // Calculate random y position within the specified range
        float randomY = Random.Range(minY, maxY);

        // Create a new image object at a random position within the specified y range
        GameObject newImage = Instantiate(imagePrefab, parentTransform);

        // Set the position of the new image
        RectTransform rectTransform = newImage.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0f, randomY);

        // Destroy the spawned image after the specified delay
        Destroy(newImage, destroyDelay);
    }
}