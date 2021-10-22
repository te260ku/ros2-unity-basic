using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtlesimTwistController : MonoBehaviour
{
    [SerializeField]
    private MANIPULATION_MODE manipulationMode;
    [SerializeField]
    private Turtlesim turtlesim;
    [SerializeField]
    private TwistPublisher twistPublisher;
    private enum MANIPULATION_MODE {
        VELOCITY, 
        KEY, 
        EXTERNAL
    }
    private float linearSpeed = 0.5f;
    private float angularSpeed = 0.2f;

    void Start()
    {
        
    }

    void Update()
    {
        // 移動と回転の速度を初期化
        Vector3 linear = new Vector3(0f, 0f, 0f);
        Vector3 angular = new Vector3(0f, 0f, 0f);

        switch (manipulationMode)
        {
            case MANIPULATION_MODE.VELOCITY:
                if (turtlesim == null) break;
                var velocity = turtlesim.GetVelocity();
                linear = velocity.linear;
                angular = velocity.angular;
                break;

            case MANIPULATION_MODE.KEY:
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

                break;

            default:
                break;
        }

        // 速度をTwistMsgインスタンスにセット
        twistPublisher.SetTwistMsgValue(linear, angular);
    }
}
