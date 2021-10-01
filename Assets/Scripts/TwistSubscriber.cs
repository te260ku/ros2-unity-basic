using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Geometry;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;

public class TwistSubscriber : MonoBehaviour
{
    [SerializeField]
    private Turtlesim turtlesim;
    [SerializeField]
    private string topicName;
    private ROSConnection ros;
    
    void Start()
    {
        ros = ROSConnection.instance;
        // topicNameに入力したトピック名をサブスクライブするように設定
        // ReceiveTwistMsg関数をコールバック関数として指定
        ros.Subscribe<TwistMsg>(topicName, ReceiveTwistMsg);
    }

    private void ReceiveTwistMsg(TwistMsg msg)
    {   
        // 移動の速度
        Vector3 linear = msg.linear.From<FLU>();
        // 回転の速度
        Vector3 angular = msg.angular.From<FLU>();
        angular.y *= -1f;

        // turtesimにサブスクライブした移動速度と回転速度を設定
        turtlesim.SetSpeed(linear, angular);
    }
}
