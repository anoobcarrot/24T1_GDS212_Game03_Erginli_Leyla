using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private LayerMask arrowLayerMask; // Set this in the inspector to only detect arrows
    [SerializeField] private Transform[] targetPositions; // Array of target positions
    [SerializeField] private Sprite[] newSprites; // Array of new sprites for each target position

    private Sprite[] originalSprites; // Array to store original sprites for reverting

    private GameManager gameManager;

    private void Start()
    {
        // Initialize array to store original sprites
        originalSprites = new Sprite[targetPositions.Length];

        gameManager = FindObjectOfType<GameManager>();

        // Store original sprites for each target position
        for (int i = 0; i < targetPositions.Length; i++)
        {
            SpriteRenderer spriteRenderer = targetPositions[i].GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                originalSprites[i] = spriteRenderer.sprite;
            }
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Check if the tap position is within the range of any target position
            foreach (Transform targetPosition in targetPositions)
            {
                if (Vector2.Distance(touchPosition, targetPosition.position) < 1f) // Adjust the threshold as needed
                {
                    // Tap position is within range of a target position
                    RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero, Mathf.Infinity, arrowLayerMask);

                    if (hit.collider != null)
                    {
                        // Arrow hit, handle scoring and feedback
                        FallingArrow fallingArrow = hit.collider.GetComponent<FallingArrow>();
                        if (fallingArrow != null)
                        {
                            fallingArrow.OnArrowHit();
                        }
                    }
                    else
                    {
                        // No arrow hit, reduce health in the GameManager
                        gameManager.ReduceHealth(0.01f);
                    }

                    // Change target sprite to new sprite corresponding to the target position
                    int index = System.Array.IndexOf(targetPositions, targetPosition);
                    if (index != -1 && index < newSprites.Length)
                    {
                        SpriteRenderer spriteRenderer = targetPosition.GetComponent<SpriteRenderer>();
                        if (spriteRenderer != null && newSprites[index] != null)
                        {
                            spriteRenderer.sprite = newSprites[index];
                        }
                    }

                    break; // Exit the loop since the tap position is within range of a target position
                }
            }
            for (int i = 0; i < targetPositions.Length; i++)
            {
                if (i < newSprites.Length) // Make sure index is within the bounds of newSprites
                {

                    // Check if the tap position is within the range of the current target position
                    if (Vector2.Distance(touchPosition, targetPositions[i].position) < 1f)
                    {
                        SpriteRenderer spriteRenderer = targetPositions[i].GetComponent<SpriteRenderer>();
                        if (spriteRenderer != null && newSprites[i] != null)
                        {
                            spriteRenderer.sprite = newSprites[i];
                        }
                    }
                }
            }
        }

        else if (Input.GetMouseButtonUp(0))
        {
            // Revert all target sprites back to original
            for (int i = 0; i < targetPositions.Length; i++)
            {
                SpriteRenderer spriteRenderer = targetPositions[i].GetComponent<SpriteRenderer>();
                if (spriteRenderer != null && originalSprites[i] != null)
                {
                    spriteRenderer.sprite = originalSprites[i];
                }
            }
        }
    }
}




