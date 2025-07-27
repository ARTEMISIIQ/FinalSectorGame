using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    public GameObject focus;
    [SerializeField]
    public Rigidbody rb;
    public float distance = 7f;
    public float height = 2f;
    public float dampening = 0.01f;
    public float h2 = 0f;
    public float d2 = 0f;
    public float l = 0f;

    private void Start()
    {
        transform.LookAt(focus.transform);
    }
    // Update is called once per frame
    void Update()
    {
        Camera.main.fieldOfView = 90f + rb.velocity.magnitude * 2.23694f / 4;
    }
}
