using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private LayerMask arrowLayerMask; // Set this in the inspector to only detect arrows
    [SerializeField] private Transform[] targetPositions; // Array of target positions

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
                    break; // Exit the loop since the tap position is within range of a target position
                }
            }
        }
    }
}


