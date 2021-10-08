using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Geometry;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;

public class PosePublisher : MonoBehaviour
{
    [SerializeField]
    private string topicName;
    [SerializeField]
    private Turtlesim turtlesim;
    private ROSConnection ros;
    private float timeElapsed;
    private float publishMessageFrequency = 0.5f;

    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<PoseMsg>(topicName); 
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed > publishMessageFrequency)
        {
            Pose p = turtlesim.GetPose();
            Vector3 pos = p.position;
            Quaternion rot = p.rotation;
            Vector3<FLU> rosPos = pos.To<FLU>();
            Quaternion<FLU> rosRot = rot.To<FLU>();

            PoseMsg poseMsg = new PoseMsg(rosPos, rosRot);

            SendPoseMsg(poseMsg);
            
            timeElapsed = 0;
        }
    }

    private void SendPoseMsg(PoseMsg msg)
    {
        ros.Publish(topicName, msg);
    }
    
}
