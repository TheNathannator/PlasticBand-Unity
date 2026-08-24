using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// Borrowed from YARG

namespace PlasticBand.Test
{
    // While we can technically just do something like `stateFormat = "BYTE"`,
    // it ends up tripping an assert since there are no control items present in the layout
    internal struct UnsupportedGameInputState : IInputStateTypeInfo
    {
        public FourCC format => new FourCC('G', 'I', 'P');

        [InputControl(layout = "Integer")]
#pragma warning disable CS0649
        public byte dummy; // Required by InputSystem layout, never accessed in code
#pragma warning restore CS0649
    }

#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoad]
#endif
    [InputControlLayout(stateType = typeof(UnsupportedGameInputState), hideInUI = true)]
    internal class UnsupportedGameInputDevice : InputDevice
    {
#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
#else
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.BeforeSceneLoad)]
#endif
        internal static void Initialize()
        {
            InputSystem.RegisterLayout<UnsupportedGameInputDevice>(
                matches: new InputDeviceMatcher().WithInterface("GameInput")
            );
        }
    }
}