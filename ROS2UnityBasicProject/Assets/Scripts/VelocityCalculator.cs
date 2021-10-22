using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityCalculator : MonoBehaviour
{
    [SerializeField]
    private GameObject turtlesim;
    private Vector3 lastPos;
    private Vector3 lastRot;

    void Start()
    {
        lastPos = turtlesim.transform.position;
        lastRot = turtlesim.transform.rotation.eulerAngles;
    }

    void Update()
    {
        // 直進速度を計算
        var worldPosDiff = turtlesim.transform.position - lastPos;
        var localPosDiff = turtlesim.transform.InverseTransformDirection(worldPosDiff);
        lastPos = turtlesim.transform.position;

        // 角速度を計算
        var worldRotDiff = turtlesim.transform.rotation.eulerAngles - lastRot;
        var localRotDiff = turtlesim.transform.InverseTransformDirection(worldRotDiff);
        lastRot = turtlesim.transform.rotation.eulerAngles;

        Debug.Log(localPosDiff*50 + ", " + localRotDiff*50);

        Vector3 linear = localPosDiff*50;
        Vector3 angular = localRotDiff*50;
    }
}
