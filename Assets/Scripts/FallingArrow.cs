using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FallingArrow : MonoBehaviour
{
    public float fallSpeed = 5f; // Adjust as needed
    public Transform targetPosition; // The position at the bottom where the arrow needs to align
    public float perfectThreshold = 0.3f; // Threshold for perfect alignment
    public float greatThreshold = 0.6f; // Threshold for great alignment
    public float earlyLateThreshold = 0.9f; // Threshold for early and late alignment
    public Image perfectImage; // UI Image for perfect hit
    public Image greatImage; // UI Image for great hit
    public Image earlyImage; // UI Image for early hit
    public Image lateImage; // UI Image for late hit
    public Image missImage; // UI Image for miss hit
    public Image comboImage; // Combo image
    public TextMeshProUGUI comboText;

    private Canvas canvas; // Reference to the canvas for instantiating UI elements
    private Vector2 topScreenPosition; // Random position near the top of the screen
    private static int consecutiveHits = 0; // Tracks the number of consecutive hits across all instances
    private static int comboCount = 0;

    private GameManager gameManager;

    private void Start()
    {
        // Find the canvas in the scene
        canvas = FindObjectOfType<Canvas>();

        // Calculate the random position near the top of the screen
        topScreenPosition = new Vector2(Random.Range(0f, Screen.width), Random.Range(Screen.height * 0.8f, Screen.height));

        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        // Move the arrow downwards
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

        // Check if the arrow is below the screen bounds
        if (IsBelowScreen())
        {
            // Miss hit
            InstantiateUIElement(missImage);
            Debug.Log("ya missed");
            // deplete health
            gameManager.ReduceHealth(0.01f);
            Debug.Log("deplete health");
            ResetCombo();
            // Destroy the arrow if it's below the screen bounds
            Destroy(gameObject);
        }
    }

    private bool IsBelowScreen()
    {
        // Get the screen position of the arrow
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.position);

        // Check if the arrow's screen position is below the screen bounds
        return screenPoint.y < 0;
    }

    private void IncrementCombo()
    {
        consecutiveHits++; // Increment consecutive hits
        Debug.Log("Consecutive Hits Count: " + consecutiveHits);

        if (consecutiveHits >= 5)
        {
            // Start the combo
            comboCount++;
            comboText.text = comboCount.ToString();
            ScoreManager.Instance.AddScore(5);
            Debug.Log("Combo Started! Combo Count: " + comboCount);

            // Instantiate combo image
            InstantiateUIElement(comboImage);
        }
    }

    private void ResetCombo()
    {
        consecutiveHits = 0; // Reset consecutive hits
        comboCount = 0; // Reset combo count
        comboText.text = ""; // Clear combo text
    }

    public void OnArrowHit()
    {
        // Calculate position difference between the arrow and the target on the y-axis
        float positionDifference = Mathf.Abs(transform.position.y - targetPosition.position.y);

        IncrementCombo();

        if (positionDifference <= perfectThreshold)
        {
            // Perfect hit
            ScoreManager.Instance.AddScore(4);
            InstantiateUIElement(perfectImage);
        }
        else if (positionDifference <= greatThreshold)
        {
            // Great hit
            ScoreManager.Instance.AddScore(2);
            InstantiateUIElement(greatImage);
        }
        else if (positionDifference <= earlyLateThreshold)
        {
            // Early or late hit
            ScoreManager.Instance.AddScore(1);

            // Check if the arrow is early or late based on its y-position in world space
            if (transform.position.y > targetPosition.position.y)
            {
                // Early hit
                InstantiateUIElement(earlyImage);
            }
            else
            {
                // Late hit
                InstantiateUIElement(lateImage);
            }
        }

        // Destroy the arrow
        Destroy(gameObject);
    }

    private void InstantiateUIElement(Image imagePrefab)
    {
        // Instantiate the UI image element on the canvas
        Image newImage = Instantiate(imagePrefab, canvas.transform);
        Vector2 nonOverlappingPosition = CalculateNonOverlappingPosition(topScreenPosition, newImage.rectTransform.rect.size);
        newImage.rectTransform.position = nonOverlappingPosition;

        // Destroy the instantiated image after 1 second
        Destroy(newImage.gameObject, 1f);
    }

    private Vector2 CalculateNonOverlappingPosition(Vector2 originalPosition, Vector2 imageSize)
    {
        // Adjust the position to prevent overlap with existing UI elements
        Vector2 nonOverlappingPosition = originalPosition;

        // Check if the new position overlaps with any existing UI elements
        foreach (var existingUI in canvas.GetComponentsInChildren<RectTransform>())
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(existingUI, nonOverlappingPosition))
            {
                // Shift the position vertically to prevent overlap
                nonOverlappingPosition.y += imageSize.y;
            }
        }

        return nonOverlappingPosition;
    }
}















