using UnityEngine;

public class Camera_Follow : MonoBehaviour
{
    //Camera Follow Player Script.
    [SerializeField, Tooltip("What you want the Camera to Follow")]
    public Transform target;

    [SerializeField, Tooltip("Edit Camera Smoothness Default (0.125f)")]
    public float damping = 0.125f;

    [SerializeField, Tooltip("Offset Camera")]
    public Vector3 offset;

    private Vector3 velocity = Vector3.zero;

    void FixedUpdate (){
        Vector3 desiredPos = target.position + offset;

        transform.position = Vector3.SmoothDamp(transform.position, desiredPos, ref velocity, damping);

    }
}
