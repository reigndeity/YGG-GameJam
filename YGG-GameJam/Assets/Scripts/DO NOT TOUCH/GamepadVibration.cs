using UnityEngine;
using UnityEngine.InputSystem;

public class GamepadVibration : MonoBehaviour
{
    public void VibrateController(float lowFrequency, float highFrequency, float duration)
    {
        Gamepad gamepad = Gamepad.current;

        if (gamepad != null)
        {
            // Set motor speeds (lowFrequency for heavy, highFrequency for light)
            gamepad.SetMotorSpeeds(lowFrequency, highFrequency);
            Invoke(nameof(StopVibration), duration); // Stop vibration after the duration
        }
        else
        {
            Debug.LogWarning("No controller connected!");
        }
    }

    private void StopVibration()
    {
        Gamepad gamepad = Gamepad.current;

        if (gamepad != null)
        {
            // Stop motors
            gamepad.SetMotorSpeeds(0, 0);
        }
    }

    public void ClickVibration() 
    {
        VibrateController(1f, 1f, 0.1f);
    }
    public void WalkVibration() 
    {
        VibrateController(0.75f, 0.75f, 0.1f);
    }
}
