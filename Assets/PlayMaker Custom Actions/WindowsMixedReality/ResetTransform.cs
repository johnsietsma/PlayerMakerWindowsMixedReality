using UnityEngine;


/**
 * Resets the position, rotation and scale of a Transform to it's initial state
 *   when an "OnReset" message is received.
 */
public class ResetTransform : MonoBehaviour {

    private Vector3 startPosition;
    private Quaternion startRotation;
    private Vector3 startScale;

    private void Awake()
    {
        startPosition = transform.localPosition;
        startRotation = transform.localRotation;
        startScale = transform.localScale;
    }

    void OnReset()
    {
        Debug.Log("Reseting transform: " + gameObject.name);
        transform.localPosition = startPosition;
        transform.localRotation = startRotation;
        transform.localScale = startScale;
    }
}
