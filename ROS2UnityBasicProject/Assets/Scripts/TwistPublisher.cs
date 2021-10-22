using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Geometry;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;

public class TwistPublisher : MonoBehaviour
{
    [SerializeField]
    private string topicName;
    
    private ROSConnection ros;
    private float timeElapsed;
    private float publishMessageFrequency = 0.5f;

    private TwistMsg twist = new TwistMsg();

    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<TwistMsg>(topicName); 
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;

        // 一定周期で中の処理を実行する
        if (timeElapsed > publishMessageFrequency)
        {
            SendTwistMsg(twist);
            
            timeElapsed = 0;
        }
    }

    private void SendTwistMsg(TwistMsg msg)
    {
        ros.Publish(topicName, msg);
    }

    public void SetTwistMsgValue(Vector3 linearVector, Vector3 angularVector) {
        // 移動速度と回転速度をそれぞれROSの座標系に変換
        Vector3<FLU> rosLinear = linearVector.To<FLU>();
        Vector3<FLU> rosAngular = angularVector.To<FLU>();
        rosAngular.z *= -1f;

        // 速度をTwistMsgインスタンスにセット
        twist.linear = rosLinear;
        twist.angular = rosAngular;
    }
}
