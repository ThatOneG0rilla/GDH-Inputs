# Introduction
Steps:
1. Import the package.
2. Drag prefab into scene, then your done!

The package offers a range of features, including:
- Easy usage of every single XR input
- Subscribing (Meaning you dont need to run an input check every frame)

## Script Usage
### `Inputs`
- Gets an input from a player
Example:
```csharp
GDHInputs.IsTrigger(ControllerHand.Left);  // Left trigger pressed
GDHInputs.IsGrip(ControllerHand.Right);  // Right grip pressed
GDHInputs.IsPrimaryButton(ControllerHand.Left);  // X/A button
GDHInputs.IsSecondaryButton(ControllerHand.Right);  // Y/B button
GDHInputs.IsThumbstickPressed(ControllerHand.Left);  // Stick click
GDHInputs.TriggerValue(ControllerHand.Right);  // Right trigger pressure (0-1)
GDHInputs.GripValue(ControllerHand.Left);  // Left grip pressure (0-1)
GDHInputs.ThumbstickAxis(ControllerHand.Left);  // Returns Vector2 of stick position
GDHInputs.HandPosition(ControllerHand.Right);  // Controller world position
GDHInputs.HandRotation(ControllerHand.Left);  // Controller rotation (Euler angles)
GDHInputs.HandVelocity(ControllerHand.Right);  // Movement velocity
GDHInputs.HandAngularVelocity(ControllerHand.Left);  // Rotation velocity
GDHInputs.SendHapticVibration(ControllerHand.Right, 0.5f, 0.3f);  // (Hand, intensity, duration)
```

### `Subscription`
- Subscribes to an input
Example:
```csharp
// Subscribe
GDHInputs.Subscribe(
    ControllerHand.Left,
    BoolInputType.TriggerPressed,
    () => { /* On pressed */ },
    () => { /* On released */ }
);

// Unsubscribe
GDHInputs.Unsubscribe(ControllerHand.Right, BoolInputType.GripPressed);
```

### `Raw Code (No other methods)`
- Directly uses helper methods, instead of using anything from Inputs
Example:
```csharp
GDHInputs.GetBoolInput(ControllerHand.Left, BoolInputType.PrimaryButtonPressed);
GDHInputs.GetFloatInput(ControllerHand.Right, FloatInputType.TriggerAmount);
GDHInputs.GetVector2Input(ControllerHand.Left, Vector2InputType.ThumbstickPosition);
GDHInputs.GetVector3Input(ControllerHand.Right, Vector3InputType.HandPosition);
```

### `Controller Checks`
- Check if controller is connected
Example:
```csharp
GDHInputs.GetBoolInput(ControllerHand.Left, BoolInputType.HandIsValid);  // Check if controller connected
```

# Info
I will be posting future versions in [Discord](https://discord.gg/gorillasdevhub). Join to get updates.
