using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Geometry;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;

public class TwistPublisher : MonoBehaviour
{
    [SerializeField]
    private string topicName;
    [SerializeField]
    private MANIPULATION_MODE manipulationMode;
    private enum MANIPULATION_MODE {
        EXTERNAL, 
        KEY
    }
    private ROSConnection ros;
    private float timeElapsed;
    private float publishMessageFrequency = 0.5f;
    private float linearSpeed = 0.5f;
    private float angularSpeed = 0.2f;
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
            switch (manipulationMode)
            {
                case MANIPULATION_MODE.EXTERNAL:
                    break;

                case MANIPULATION_MODE.KEY:
                    // 移動と回転の速度を初期化
                    Vector3 linear = new Vector3(0f, 0f, 0f);
                    Vector3 angular = new Vector3(0f, 0f, 0f);
        
                    // 押された矢印キーの方向によって速度を与える
                    if (Input.GetKey(KeyCode.UpArrow)) {
                        linear.z = linearSpeed;
                    }
                    if (Input.GetKey(KeyCode.DownArrow)) {
                        linear.z = -linearSpeed;
                    }
                    if (Input.GetKey(KeyCode.LeftArrow)) {
                        angular.y = -angularSpeed;
                    }
                    if (Input.GetKey(KeyCode.RightArrow)) {
                        angular.y = angularSpeed;
                    }

                    // 速度をTwistMsgインスタンスにセット
                    SetTwistMsgValue(linear, angular);

                    break;

                default:
                    break;
            }
            
            
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
