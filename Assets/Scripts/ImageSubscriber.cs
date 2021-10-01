using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Sensor;

public class ImageSubscriber : MonoBehaviour
{
    public Renderer targetRenderer;
    [SerializeField]
    private string topicName;
    private ROSConnection ros;
    private Texture2D tex;
    
    void Start()
    {
        ros = ROSConnection.instance;
        ros.Subscribe<ImageMsg>(topicName, ReceiveMsg);

        tex = new Texture2D(640, 480, TextureFormat.RGB24, false);
    }

    void ReceiveMsg(ImageMsg compressedImage)
    {
        byte[] imageData = compressedImage.data;
        RenderTexture(imageData);
    }

    private void RenderTexture(byte[] data)
    {        
        tex.LoadRawTextureData(data);
        targetRenderer.material.mainTexture = tex;
        tex.Apply();
    }
}