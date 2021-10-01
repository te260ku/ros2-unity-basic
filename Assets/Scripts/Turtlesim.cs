using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Turtlesim : MonoBehaviour
{
    private Vector3 linearSpeed;
    private Vector3 angularSpeed;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        Move(this.linearSpeed, this.angularSpeed); 
    }

    private void Move(Vector3 linear, Vector3 angular)
    {
        rb.velocity = transform.TransformVector(linear);
        rb.angularVelocity = angular;
    }

    public void SetSpeed(Vector3 linear, Vector3 angular) {
        this.linearSpeed = linear;
        this.angularSpeed = angular;
    }

    public void SetPose(Vector3 pos, Quaternion rot) {
        transform.position = pos;
        transform.rotation = rot; 
    }
}
