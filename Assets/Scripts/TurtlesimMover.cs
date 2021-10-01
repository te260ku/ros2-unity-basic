using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtlesimMover : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 0.05f;
    [SerializeField]
    private int rotationSpeed = 30;
    Dictionary<string, bool> move = new Dictionary<string, bool>
    {
        {"up", false },
        {"down", false },
        {"right", false },
        {"left", false },
    };
 
    void Start()
    {
 
    }
 
    void Update()
    {
        move["up"] = Input.GetKey(KeyCode.UpArrow);
        move["down"] = Input.GetKey(KeyCode.DownArrow);
        move["right"] = Input.GetKey(KeyCode.RightArrow);
        move["left"] = Input.GetKey(KeyCode.LeftArrow);
    }
 
    void FixedUpdate()
    {
        if (move["up"])
        {
            transform.Translate(0f, 0f, moveSpeed);
        }
        if (move["down"])
        {
            transform.Translate(0f, 0f, -moveSpeed);
        }
        if (move["right"])
        {
            transform.Rotate(new Vector3(0, rotationSpeed, 0) * Time.deltaTime, Space.World);
        }
        if (move["left"])
        {
            transform.Rotate(new Vector3(0, -rotationSpeed, 0) * Time.deltaTime, Space.World);
        }
    }
}
