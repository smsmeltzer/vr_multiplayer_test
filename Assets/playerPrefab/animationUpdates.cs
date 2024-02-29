using UnityEngine;

public class animationUpdates : MonoBehaviour
{
    public Animator animator;
    public Rigidbody rb;

    // Previous position of the object
    private Vector3 previousPosition;

    // Threshold to consider the object as moving
    public float movementThreshold = 0.01f;

    // Update is called once per frame
    void Update()
    {
        // Calculate the distance moved since the last frame
        float distanceMoved = Vector3.Distance(transform.position, previousPosition);

        // Check if the distance moved is greater than the threshold
        bool isMoving = distanceMoved > movementThreshold;

        // Update the animator's boolean parameter
        animator.SetBool("IsMoving", isMoving);

        // Update the previous position for the next frame
        previousPosition = transform.position;
    }
}

