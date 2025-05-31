using UnityEngine;
using UnityEngine.XR;
using System;
using System.Collections.Generic;
public static class GDHInputs
{
    #region Variables
    private static readonly Dictionary<string, Action> PressLeft = new();
    private static readonly Dictionary<string, Action> ReleaseLeft = new();
    private static readonly Dictionary<string, Action> PressRight = new();
    private static readonly Dictionary<string, Action> ReleaseRight = new();
    private static readonly Dictionary<string, bool> StateLeft = new();
    private static readonly Dictionary<string, bool> StateRight = new();
    #endregion

    #region Usage
    public static void Update()
    {
        foreach (BoolInputType Type in Enum.GetValues(typeof(BoolInputType)))
        {
            string Key = Type.ToString();

            bool CurrentLeft = GetBool(ControllerHand.Left, Type);
            bool PreviousLeft = StateLeft.GetValueOrDefault(Key, false);

            if (CurrentLeft && !PreviousLeft && PressLeft.TryGetValue(Key, out Action pressLeft))
                pressLeft.Invoke();
            else if (!CurrentLeft && PreviousLeft && ReleaseLeft.TryGetValue(Key, out Action releaseLeft))
                releaseLeft.Invoke();
            StateLeft[Key] = CurrentLeft;

            bool CurrentRight = GetBool(ControllerHand.Right, Type);
            bool PreviousRight = StateRight.GetValueOrDefault(Key, false);

            if (CurrentRight && !PreviousRight && PressRight.TryGetValue(Key, out Action pressRight))
                pressRight.Invoke();
            else if (!CurrentRight && PreviousRight && ReleaseRight.TryGetValue(Key, out Action releaseRight))
                releaseRight.Invoke();
            StateRight[Key] = CurrentRight;
        }
    }
    /// <summary>
    /// Sends haptic feedback to the specified controller.
    /// </summary>
    /// <param name="Hand">Controller to receive the vibration</param>
    /// <param name="Amplitude">Vibration strength (0-1)</param>
    /// <param name="Duration">Vibration length in seconds</param>
    public static void SendHapticVibration(ControllerHand Hand, float Amplitude, float Duration)
    {
        InputDevice Device = GetDevice(Hand);
        if (!Device.isValid) return;

        if (Device.TryGetHapticCapabilities(out HapticCapabilities Capabilities) && Capabilities.supportsImpulse)
            Device.SendHapticImpulse(0, Mathf.Clamp01(Amplitude), Mathf.Max(0, Duration));
    }
    public static bool GetBoolInput(ControllerHand Hand, BoolInputType Type) => GetBool(Hand, Type);
    public static float GetFloatInput(ControllerHand Hand, FloatInputType Type) => GetFloat(Hand, Type);
    public static Vector2 GetVector2Input(ControllerHand Hand, Vector2InputType Type) => GetVector2(Hand, Type);
    public static Vector3 GetVector3Input(ControllerHand Hand, Vector3InputType Type) => GetVector3(Hand, Type);
    /// <summary>Checks if trigger is pressed</summary>
    public static bool IsTrigger(ControllerHand Hand) => GetBoolInput(Hand, BoolInputType.TriggerPressed);
    /// <summary>Checks if grip is pressed</summary>
    public static bool IsGrip(ControllerHand Hand) => GetBoolInput(Hand, BoolInputType.GripPressed);
    /// <summary>Checks if primary button is pressed</summary>
    public static bool IsPrimaryButton(ControllerHand Hand) => GetBoolInput(Hand, BoolInputType.PrimaryButtonPressed);
    /// <summary>Checks if secondary button is pressed</summary>
    public static bool IsSecondaryButton(ControllerHand Hand) => GetBoolInput(Hand, BoolInputType.SecondaryButtonPressed);
    /// <summary>Checks if thumbstick is pressed</summary>
    public static bool IsThumbstickPressed(ControllerHand Hand) => GetBoolInput(Hand, BoolInputType.ThumbstickPressed);
    /// <summary>Gets trigger pressure amount</summary>
    public static float TriggerValue(ControllerHand Hand) => GetFloatInput(Hand, FloatInputType.TriggerAmount);
    /// <summary>Gets grip pressure amount</summary>
    public static float GripValue(ControllerHand Hand) => GetFloatInput(Hand, FloatInputType.GripAmount);
    /// <summary>Gets thumbstick position</summary>
    public static Vector2 ThumbstickAxis(ControllerHand Hand) => GetVector2Input(Hand, Vector2InputType.ThumbstickPosition);
    /// <summary>Gets controller position</summary>
    public static Vector3 HandPosition(ControllerHand Hand) => GetVector3Input(Hand, Vector3InputType.HandPosition);
    /// <summary>Gets controller rotation</summary>
    public static Vector3 HandRotation(ControllerHand Hand) => GetVector3Input(Hand, Vector3InputType.HandRotation);
    /// <summary>Gets controller velocity</summary>
    public static Vector3 HandVelocity(ControllerHand Hand) => GetVector3Input(Hand, Vector3InputType.HandVelocity);
    /// <summary>Gets controller angular velocity</summary>
    public static Vector3 HandAngularVelocity(ControllerHand Hand) => GetVector3Input(Hand, Vector3InputType.HandAngularVelocity);
    /// <summary>
    /// Subscribes to press and release events for a input type
    /// </summary>
    public static void Subscribe(ControllerHand Hand, BoolInputType Type, Action OnPress, Action OnRelease)
    {
        string Key = Type.ToString();
        var Press = Hand == ControllerHand.Left ? PressLeft : PressRight;
        var Release = Hand == ControllerHand.Left ? ReleaseLeft : ReleaseRight;

        if (OnPress != null) Press[Key] = OnPress;
        if (OnRelease != null) Release[Key] = OnRelease;
    }

    /// <summary>
    /// Unsubscribe
    /// </summary>
    public static void Unsubscribe(ControllerHand Hand, BoolInputType Type)
    {
        string Key = Type.ToString();
        var Press = Hand == ControllerHand.Left ? PressLeft : PressRight;
        var Release = Hand == ControllerHand.Left ? ReleaseLeft : ReleaseRight;
        Press.Remove(Key);
        Release.Remove(Key);
    }
    #endregion

    #region Helper Methods
    private static bool GetBool(ControllerHand Hand, BoolInputType Type)
    {
        InputDevice Device = GetDevice(Hand);
        if (!Device.isValid) return false;

        return Type switch
        {
            BoolInputType.TriggerPressed => Device.TryGetFeatureValue(CommonUsages.triggerButton, out bool val) && val,
            BoolInputType.GripPressed => Device.TryGetFeatureValue(CommonUsages.gripButton, out bool val) && val,
            BoolInputType.PrimaryButtonPressed => Device.TryGetFeatureValue(CommonUsages.primaryButton, out bool val) && val,
            BoolInputType.PrimaryButtonTouched => Device.TryGetFeatureValue(CommonUsages.primaryTouch, out bool val) && val,
            BoolInputType.SecondaryButtonPressed => Device.TryGetFeatureValue(CommonUsages.secondaryButton, out bool val) && val,
            BoolInputType.SecondaryButtonTouched => Device.TryGetFeatureValue(CommonUsages.secondaryTouch, out bool val) && val,
            BoolInputType.ThumbstickPressed => Device.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out bool val) && val,
            BoolInputType.ThumbstickTouched => Device.TryGetFeatureValue(CommonUsages.primary2DAxisTouch, out bool val) && val,
            BoolInputType.MenuButtonPressed => Device.TryGetFeatureValue(CommonUsages.menuButton, out bool val) && val,
            BoolInputType.HandIsValid => Device.isValid,
            _ => false,
        };
    }
    private static float GetFloat(ControllerHand Hand, FloatInputType Type)
    {
        InputDevice Device = GetDevice(Hand);
        if (!Device.isValid) return 0f;

        return Type switch
        {
            FloatInputType.TriggerAmount => Device.TryGetFeatureValue(CommonUsages.trigger, out float val) ? val : 0f,
            FloatInputType.GripAmount => Device.TryGetFeatureValue(CommonUsages.grip, out float val) ? val : 0f,
            _ => 0f,
        };
    }
    private static Vector2 GetVector2(ControllerHand Hand, Vector2InputType Type)
    {
        InputDevice Device = GetDevice(Hand);
        if (!Device.isValid) 
            return Vector2.zero;

        return Type switch
        {
            Vector2InputType.ThumbstickPosition => Device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 val) ? val : Vector2.zero,
            _ => Vector2.zero,
        };
    }
    private static Vector3 GetVector3(ControllerHand Hand, Vector3InputType Type)
    {
        InputDevice Device = GetDevice(Hand);
        if (!Device.isValid) 
            return Vector3.zero;

        return Type switch
        {
            Vector3InputType.HandPosition => Device.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 val) ? val : Vector3.zero,
            Vector3InputType.HandRotation => Device.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rot) ? rot.eulerAngles : Vector3.zero,
            Vector3InputType.HandVelocity => Device.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 val) ? val : Vector3.zero,
            Vector3InputType.HandAngularVelocity => Device.TryGetFeatureValue(CommonUsages.deviceAngularVelocity, out Vector3 val) ? val : Vector3.zero,
            _ => Vector3.zero,
        };
    }
    private static InputDevice GetDevice(ControllerHand Hand)
    {
        InputDeviceCharacteristics Filter = (Hand == ControllerHand.Left)
            ? InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller
            : InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;

        var Devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(Filter, Devices);
        return Devices.Count > 0 ? Devices[0] : default;
    }
    #endregion
}

#region Enums
/// <summary>
/// Which controller to use
/// </summary>
public enum ControllerHand
{
    Left = 4,
    Right = 5
}
/// <summary>
/// Which input to check
/// </summary>
public enum BoolInputType
{
    TriggerPressed,
    TriggerTouched,
    GripPressed,
    PrimaryButtonPressed,
    PrimaryButtonTouched,
    SecondaryButtonPressed,
    SecondaryButtonTouched,
    ThumbstickPressed,
    ThumbstickTouched,
    MenuButtonPressed,
    ThumbRestTouched,
    HandIsValid
}
/// <summary>
/// Inputs usage amount
/// </summary>
public enum FloatInputType
{
    TriggerAmount,
    GripAmount
}
/// <summary>
/// 2D axis of an input
/// </summary>
public enum Vector2InputType
{
    ThumbstickPosition
}
/// <summary>
/// 3D Vector of an input
/// </summary>
public enum Vector3InputType
{
    HandPosition,
    HandRotation,
    HandVelocity,
    HandAngularVelocity
}
#endregion