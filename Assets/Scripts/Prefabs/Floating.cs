using UnityEngine;

public class Floating : MonoBehaviour
{
    [SerializeField] private SliderJoint2D sliderJoint;
    [SerializeField] private float positiveSpeed = 0.5f;
    [SerializeField] private float negativeSpeed = -0.5f;
    [SerializeField] private float motorForce = 10000f;

    private void Start()
    {
        SetMotorSpeed(positiveSpeed);
    }

    private void Update()
    {
        // Define a tolerance range
        float tolerance = 0.1f;

        if (Mathf.Abs(sliderJoint.limits.max - sliderJoint.jointTranslation) < tolerance)
        {
            SetMotorSpeed(negativeSpeed);
        }
        else if (Mathf.Abs(sliderJoint.limits.min - sliderJoint.jointTranslation) < tolerance)
        {
            SetMotorSpeed(positiveSpeed);
        }
    }

    private void SetMotorSpeed(float speed)
    {
        JointMotor2D motor = sliderJoint.motor;
        motor.motorSpeed = speed;
        motor.maxMotorTorque = motorForce;
        sliderJoint.motor = motor;
    }
}
