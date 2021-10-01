using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using RosMessageTypes.Nav;

public class OdomSubscriber : MonoBehaviour
{
    [SerializeField] private Turtlesim turtlesim;
    [SerializeField] private string topicName;
    private ROSConnection ros;
    private Pose initialPose;
    private bool receivedFisrtMsg;

    void Start()
    {
        ros = ROSConnection.instance;
        ros.Subscribe<OdometryMsg>(topicName, ReceiveOdomMsg);
    }

    private void ReceiveOdomMsg(OdometryMsg msg)
    {
        Vector3 pos = msg.pose.pose.position.From<FLU>();
        Quaternion rot = msg.pose.pose.orientation.From<FLU>();

        if (!receivedFisrtMsg) {
            // 初期位置を保存
            initialPose.position = pos;
            initialPose.rotation = rot;
            receivedFisrtMsg = true;
        }

        // 原点をリセット
        Vector3 adjustedPos = pos - initialPose.position;
        Quaternion adjustedRot = rot * Quaternion.Inverse(initialPose.rotation);
        
        // turtesimにサブスクライブした位置と角度を設定
        turtlesim.SetPose(adjustedPos, adjustedRot);
    }
}
