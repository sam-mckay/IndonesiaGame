using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    Rigidbody rigidbody;
    Vector3 velocity;

    public float maxDistance = 10.0f;
    public float maxWidth = 10.0f;
    public float maxBrightness = 10.0f;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized * 10;
    }

    void FixedUpdate()
    {
        rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);
    }
}

