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
    public List<GameObject> steeringWheelMeshes;
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
            trailRenderers[i] = Instantiate(trail, wheel.transform.position - 2 * Vector3.up * wheel.radius, Quaternion.identity, wheel.transform).GetComponent<TrailRenderer>();
            i++;
        }
        rb.centerOfMass = new Vector3(0f, -0.3f, -0.1f);
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
                wheel.brakeTorque = brakeStrength;
            }
            else
            {
                if (uim.durability.value > 0)
                {
                    uim.durabilityWarning.gameObject.SetActive(false);
                    wheel.motorTorque = strengthCoefficient * im.throttle;
                    currTorque = im.throttle;
                    wheel.brakeTorque = 0f;
                }
                else
                {
                    wheel.motorTorque = 0;
                    uim.durabilityWarning.gameObject.SetActive(true);
                    uim.durabilityWarning.text = "Tires damaged. Pit Stop to repair";
                }
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
                forward.extremumSlip = 1f;
                forward.extremumValue = 1f;
                forward.asymptoteSlip = 1f;
                forward.asymptoteValue = 1f;
                sideways.extremumSlip = 0.1f;
                sideways.extremumValue = 0.1f;
                sideways.asymptoteSlip = 0.1f;
                sideways.asymptoteValue = 0.1f;
                wheel.forwardFriction = forward;
                wheel.sidewaysFriction = sideways;
            }
            else
            {
                wheelPhys(wheel);
            }
            speed = wheel.rpm * 2 * Mathf.PI * 0.8f;
            i++;
        }
        foreach (GameObject wheel in steeringWheels)
        {
            wheel.GetComponent<WheelCollider>().steerAngle = maxTurn * im.steer;
            wheel.transform.localEulerAngles = new Vector3(0f, im.steer * maxTurn, 0f);
        }
        foreach (GameObject wheel in steeringWheelMeshes)
        {
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


    public void wheelPhys(WheelCollider wheel)
    {
        WheelFrictionCurve sideways = wheel.sidewaysFriction;
        WheelFrictionCurve forward = wheel.forwardFriction;

        int tireNum = PlayerPrefs.GetInt("Tire");
        if (tireNum == 0)
        {
            forward.extremumSlip = 0.2f;
            forward.extremumValue = 2.3f;
            forward.asymptoteSlip = 0.5f;
            forward.asymptoteValue = 1.2f;
            sideways.extremumSlip = 0.25f;
            sideways.extremumValue = 2.8f;
            sideways.asymptoteSlip = 0.5f;
            sideways.asymptoteValue = 1.2f;
            forward.stiffness = 1.5f;
            sideways.stiffness = 2.0f;
        }
        if (tireNum == 1)
        {
            forward.extremumSlip = 0.22f;
            forward.extremumValue = 2.7f;
            forward.asymptoteSlip = 0.5f;
            forward.asymptoteValue = 1.4f;
            sideways.extremumSlip = 0.27f;
            sideways.extremumValue = 3.0f;
            sideways.asymptoteSlip = 0.5f;
            sideways.asymptoteValue = 1.4f;
            forward.stiffness = 1.4f;
            sideways.stiffness = 1.0f;
        }
        if (tireNum == 2)
        {
            forward.extremumSlip = 0.25f;
            forward.extremumValue = 2.8f;
            forward.asymptoteSlip = 0.5f;
            forward.asymptoteValue = 1.4f;
            sideways.extremumSlip = 0.25f;
            sideways.extremumValue = 3.0f;
            sideways.asymptoteSlip = 0.5f;
            sideways.asymptoteValue = 1.5f;
            forward.stiffness = 1.3f;
            sideways.stiffness = 1.2f;
        }
        if (tireNum == 3)
        {
            forward.extremumSlip = 0.22f;
            forward.extremumValue = 3.2f;
            forward.asymptoteSlip = 0.45f;
            forward.asymptoteValue = 1.2f;
            sideways.extremumSlip = 0.22f;
            sideways.extremumValue = 3.5f;
            sideways.asymptoteSlip = 0.45f;
            sideways.asymptoteValue = 1.3f;
            forward.stiffness = 1.2f;
            sideways.stiffness = 1.7f;
        }
        if (tireNum == 4)
        {
            forward.extremumSlip = 0.18f;
            forward.extremumValue = 3.6f;
            forward.asymptoteSlip = 0.4f;
            forward.asymptoteValue = 1.0f;
            sideways.extremumSlip = 0.18f;
            sideways.extremumValue = 3.9f;
            sideways.asymptoteSlip = 0.4f;
            sideways.asymptoteValue = 1.1f;
            forward.stiffness = 1.1f;
            sideways.stiffness = 2.2f;
        }
        if (tireNum == 5)
        {
            forward.extremumSlip = 0.15f;
            forward.extremumValue = 3.8f;
            forward.asymptoteSlip = 0.35f;
            forward.asymptoteValue = 0.9f;
            sideways.extremumSlip = 0.15f;
            sideways.extremumValue = 4.1f;
            sideways.asymptoteSlip = 0.35f;
            sideways.asymptoteValue = 1.0f;
            forward.stiffness = 1.0f;
            sideways.stiffness = 2.7f;
        }
        wheel.forwardFriction = forward;
        wheel.sidewaysFriction = sideways;
    }
}