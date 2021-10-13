using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtlesimMover : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 0.1f;
    [SerializeField]
    private int rotationSpeed = 30;
    private Dictionary<string, bool> moveKey = new Dictionary<string, bool>
    {
        {"up", false },
        {"down", false },
        {"right", false },
        {"left", false },
    };
    private Vector3 lastPos;

    void Start() {
        lastPos = transform.position;
    }
 
    void Update()
    {
        moveKey["up"] = Input.GetKey(KeyCode.UpArrow);
        moveKey["down"] = Input.GetKey(KeyCode.DownArrow);
        moveKey["right"] = Input.GetKey(KeyCode.RightArrow);
        moveKey["left"] = Input.GetKey(KeyCode.LeftArrow);
    }
 
    void FixedUpdate()
    {
        if (moveKey["up"])
        {
            transform.Translate(0f, 0f, moveSpeed * Time.deltaTime);
        }
        if (moveKey["down"])
        {
            transform.Translate(0f, 0f, -moveSpeed * Time.deltaTime);
        }
        if (moveKey["right"])
        {
            transform.Rotate(new Vector3(0, rotationSpeed, 0) * Time.deltaTime, Space.World);
        }
        if (moveKey["left"])
        {
            transform.Rotate(new Vector3(0, -rotationSpeed, 0) * Time.deltaTime, Space.World);
        }


        var worldPosDiff = transform.position - lastPos;
        var localPosDiff = transform.InverseTransformDirection(worldPosDiff);
        lastPos = transform.position;

    }
}
