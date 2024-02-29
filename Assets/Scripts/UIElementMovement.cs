using UnityEngine;
using System.Collections;

public class UIElementMovement : MonoBehaviour
{
    public RectTransform targetTransform;
    public Vector3 targetPosition;
    public float moveDuration = 1.0f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = targetTransform.localPosition;
        MoveElement();
    }

    private void MoveElement()
    {
        StartCoroutine(MoveCoroutine());
    }

    private IEnumerator MoveCoroutine()
    {
        float timer = 0.0f;
        while (timer < moveDuration)
        {
            float progress = timer / moveDuration;
            targetTransform.localPosition = Vector3.Lerp(startPosition, targetPosition, progress);
            timer += Time.deltaTime;
            yield return null;
        }
        targetTransform.localPosition = targetPosition;
    }
}

