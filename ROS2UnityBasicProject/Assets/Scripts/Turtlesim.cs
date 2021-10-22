using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Turtlesim : MonoBehaviour
{
    private Vector3 linearSpeed;
    private Vector3 angularSpeed;
    private Rigidbody rb;
    private Vector3 lastPos;
    private Vector3 lastRot;
    private (Vector3 linear, Vector3 angular) velocity = (new Vector3(), new Vector3());
    private float timeElapsed;
    private float publishMessageFrequency = 0.05f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        lastPos = transform.position;
        lastRot = transform.rotation.eulerAngles;
    }

    private void CalculateVelocity() {
        // 直進速度を計算
        var worldPosDiff = transform.position - lastPos;
        var localPosDiff = transform.InverseTransformDirection(worldPosDiff);
        lastPos = transform.position;

        // 角速度を計算
        var worldRotDiff = transform.rotation.eulerAngles - lastRot;
        var localRotDiff = transform.InverseTransformDirection(worldRotDiff);
        lastRot = transform.rotation.eulerAngles;

        Debug.Log(localPosDiff*50 + ", " + localRotDiff);

        Vector3 linear = localPosDiff*50;
        Vector3 angular = localRotDiff;

        velocity.linear = linear;
        velocity.angular = angular;
    }

    public (Vector3 linear, Vector3 angular) GetVelocity() {
        return velocity;
    }

    private void FixedUpdate() {
        Move(this.linearSpeed, this.angularSpeed); 

        CalculateVelocity();
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

    public Pose GetPose() {
        Pose pose = new Pose(transform.position, transform.rotation);
        return pose;
    }
}
