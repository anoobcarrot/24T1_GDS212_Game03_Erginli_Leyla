using UnityEngine;

public class FallingArrow : MonoBehaviour
{
    public float fallSpeed = 5f; // Adjust as needed
    public Transform targetPosition; // The position at the bottom where the arrow needs to align
    public float perfectThreshold = 0.1f; // Threshold for perfect alignment

    private void Update()
    {
        // Move the arrow downwards
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

        // Check if the arrow is below the screen bounds
        if (IsBelowScreen())
        {
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

    public void OnArrowHit()
    {
        // Calculate position difference between the arrow and the target
        float positionDifference = Mathf.Abs(transform.position.y - targetPosition.position.y);

        // Determine the score based on the position difference
        int score = (positionDifference <= perfectThreshold) ? 4 : 2;

        // Handle scoring and feedback
        ScoreManager.Instance.AddScore(score);

        // Destroy the arrow
        Destroy(gameObject);
    }
}




