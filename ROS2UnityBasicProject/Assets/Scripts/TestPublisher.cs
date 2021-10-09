using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Geometry;


public class TestPublisher : MonoBehaviour
{
    [SerializeField]
    private string topicName;
    private ROSConnection ros;
    private float timeElapsed;
    private float publishMessageFrequency = 0.5f;

 

    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
   
        ros.RegisterPublisher<TwistMsg>(topicName);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) {
            ros.RegisterPublisher<TwistMsg>(topicName); 
        } 
        if (Input.GetKeyDown(KeyCode.S)) {
            ros.RegisterPublisher<PoseMsg>(topicName); 
        } 
        
    }



    private void SendTwistMsg(TwistMsg msg)
    {
        ros.Publish(topicName, msg);
    }
}
