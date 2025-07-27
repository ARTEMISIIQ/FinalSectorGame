using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;

[RequireComponent(typeof(InputManager))]
[RequireComponent(typeof(Rigidbody))]
public class CarController: MonoBehaviour
{
    public InputManager im;
    public UIManager uim;
    public AudioScript am;
    public List<WheelCollider> throttleWheels;
    public List<GameObject> steeringWheels;
    public List<GameObject> meshes;
    public List<TrailRenderer> trailRenderers;
    public float strengthCoefficient = 7500;
    public float maxTurn = 20f;
    public Rigidbody rb;
    public float brakeStrength;

    public GameObject trail;

    public float speed;
    public float currTorque;
    public float lastTorque;

    void Start()
    {
        im = GetComponent<InputManager>();
        rb = GetComponent<Rigidbody>();
        lastTorque = 0;
        int i = 0;
        foreach (WheelCollider wheel in throttleWheels)
        {
            trailRenderers[i] = Instantiate(trail, wheel.transform.position - Vector3.up * wheel.radius, Quaternion.identity, wheel.transform).GetComponent<TrailRenderer>();
            i++;
        }
    }

    void Update()
    {
        uim.changeText(transform.InverseTransformVector(rb.velocity).z);
    }

    void FixedUpdate()
    {
        int i = 0;
        foreach (WheelCollider wheel in throttleWheels)
        {
            WheelHit hit;
            if (im.brake)
            {
                wheel.motorTorque = 0f;
                currTorque = 0;
                wheel.brakeTorque = brakeStrength * Time.deltaTime;
            }
            else
            {
                wheel.motorTorque = strengthCoefficient * Time.deltaTime * im.throttle;
                currTorque = im.throttle;
                wheel.brakeTorque = 0f;
            }
            float slipAllowance = 0.29f;
            wheel.GetGroundHit(out hit);
            if (Mathf.Abs(hit.sidewaysSlip) > slipAllowance)
            {
                trailRenderers[i].emitting = true;
            }
            else
            {
                trailRenderers[i].emitting = false;
            }
            WheelFrictionCurve sideways = wheel.sidewaysFriction;
            WheelFrictionCurve forward = wheel.forwardFriction;
            if (wheel.GetGroundHit(out hit) && hit.collider.gameObject.tag == "Ice")
            {
                forward.extremumSlip = 0.1f;
                forward.extremumValue = 0.1f;
                forward.asymptoteSlip = 0.1f;
                forward.asymptoteValue = 0.1f;
                sideways.extremumSlip = 0.01f;
                sideways.extremumValue = 0.1f;
                sideways.asymptoteSlip = 0.1f;
                sideways.asymptoteValue = 0.1f;
            }
            else
            {
                forward.extremumSlip = 0.3f;
                forward.extremumValue = 1f;
                forward.asymptoteSlip = 4f;
                forward.asymptoteValue = 1f;
                sideways.extremumSlip = 0.05f;
                sideways.extremumValue = 0.9f;
                sideways.asymptoteSlip = 0.285f;
                sideways.asymptoteValue = 0.85f;
            }
            wheel.forwardFriction = forward;
            wheel.sidewaysFriction = sideways;
            speed = wheel.rpm * 2 * Mathf.PI * 0.8f;
            i++;
        }
        foreach (GameObject wheel in steeringWheels)
        {
            wheel.GetComponent<WheelCollider>().steerAngle = maxTurn * im.steer;
            wheel.transform.localEulerAngles = new Vector3(0f, im.steer * maxTurn, 0f);
        }
        foreach (GameObject mesh in meshes)
        {
            mesh.transform.Rotate(rb.velocity.magnitude * (transform.InverseTransformDirection(rb.velocity).z >= 0 ? -1 : 1) / (2 * Mathf.PI * 0.8f), 0f, 0f);
        }
        if (lastTorque == 0 && im.throttle > 0 && speed < 1 && !im.brake)
        {
            am.StartEngine();
        }    
        lastTorque = currTorque;
    }
}