using UnityEngine;

public class CarController : MonoBehaviour
{    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private float input_Hor;
    private float input_Ver;

    private float currentSteerAngle;
    private bool isBraking;

    [SerializeField] private float motorForce;
    [SerializeField] private float userBrakeForce;
    private float brakeForce;

    [SerializeField] private float maxSteerAngle;

    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider backLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider backRightWheelCollider;

    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform backLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform backRightWheelTransform;

    private void Start()
    {
        motorForce = motorForce * 10000;
        brakeForce = userBrakeForce * 10000;
    }

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void GetInput()
    {
        input_Hor = Input.GetAxis(HORIZONTAL);
        input_Ver = Input.GetAxis(VERTICAL);
        isBraking = Input.GetKey(KeyCode.Space);
    }

    private void HandleMotor()
    {
        backLeftWheelCollider.motorTorque = input_Ver * motorForce;
        backRightWheelCollider.motorTorque = input_Ver * motorForce;

        if (isBraking)
        {
            frontLeftWheelCollider.brakeTorque = brakeForce;
            frontRightWheelCollider.brakeTorque = brakeForce;
            backLeftWheelCollider.brakeTorque = brakeForce;
            backRightWheelCollider.brakeTorque = brakeForce;
        }
        else {
            frontLeftWheelCollider.brakeTorque = 0;
            frontRightWheelCollider.brakeTorque = 0;
            backLeftWheelCollider.brakeTorque = 0;
            backRightWheelCollider.brakeTorque = 0;
        }
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * input_Hor;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(backLeftWheelCollider, backLeftWheelTransform);
        UpdateSingleWheel(backRightWheelCollider, backRightWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
    }
}
