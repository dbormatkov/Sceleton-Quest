using UnityEngine;

public class ResetSkeleton : MonoBehaviour
{
    public Transform skeletonTransform; 
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 originalScale;

    void Start()
    {
        originalPosition = skeletonTransform.position;
        originalRotation = skeletonTransform.rotation;
        originalScale = skeletonTransform.localScale;
    }

    public void ResetToOriginalState()
    {
        if (skeletonTransform != null)
        {
            skeletonTransform.position = Vector3.zero;
            skeletonTransform.rotation = originalRotation;
            skeletonTransform.localScale = Vector3.one;
        }
        else
        {
            Debug.LogError("Skeleton Transform nicht zugewiesen!");
        }
    }
}