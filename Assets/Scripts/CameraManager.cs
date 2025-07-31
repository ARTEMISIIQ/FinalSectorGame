using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    public GameObject focus;
    [SerializeField]
    public Rigidbody rb;

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
