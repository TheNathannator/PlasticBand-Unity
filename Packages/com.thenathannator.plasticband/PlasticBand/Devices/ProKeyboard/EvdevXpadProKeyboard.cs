using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct EvdevXpadProKeyboardState : IProKeyboardState
    {
        public static FourCC Format => new FourCC('E', 'V', 'D', 'V');
        public FourCC format => Format;

        public EvdevState state;

        public bool south { get => state.GetKey(0x130); set => state.SetKey(0x130, value); }
        public bool east  { get => state.GetKey(0x131); set => state.SetKey(0x131, value); }
        public bool west  { get => state.GetKey(0x133); set => state.SetKey(0x133, value); }
        public bool north { get => state.GetKey(0x134); set => state.SetKey(0x134, value); }

        public bool select { get => state.GetKey(0x13A); set => state.SetKey(0x13A, value); }
        public bool start  { get => state.GetKey(0x13B); set => state.SetKey(0x13B, value); }
        public bool system { get => state.GetKey(0x13C); set => state.SetKey(0x13C, value); }

        public bool dpadLeft  { get => state.GetAxis(0x10) < 0 || state.GetKey(0x222); set => state.SetAxis(0x10, value ? -1 : 0); }
        public bool dpadRight { get => state.GetAxis(0x10) > 0 || state.GetKey(0x223); set => state.SetAxis(0x10, value ? 1 : 0); }
        public bool dpadUp    { get => state.GetAxis(0x11) < 0 || state.GetKey(0x220); set => state.SetAxis(0x11, value ? -1 : 0); }
        public bool dpadDown  { get => state.GetAxis(0x11) > 0 || state.GetKey(0x221); set => state.SetAxis(0x11, value ? 1 : 0); }

        public byte keys1 { get => (byte)state.GetAxis(2); set {} }
        public byte keys2 { get => (byte)state.GetAxis(5); set {} }
        public byte keys3 { get => (byte)state.GetAxis(0); set {} }

        public byte velocity1 { get => (byte)(state.GetAxis(0) >> 8); set {} }
        public byte velocity2 { get => (byte)~state.GetAxis(1); set {} }
        public byte velocity3 { get => (byte)(~state.GetAxis(1) >> 8); set {} }
        public byte velocity4 { get => (byte)state.GetAxis(3); set {} }
        public byte velocity5 { get => (byte)(state.GetAxis(3) >> 8); set {} }

        public byte overdrive { get => (byte)~state.GetAxis(4); set {} }
        public byte pedal     { get => (byte)(~state.GetAxis(4) >> 8); set {} }
        public byte touchStrip { get => 0; set {} }
    }

    // Xpad's standard trigger and stick events preserve the keyboard key bitmap,
    // overdrive and pedal bytes. Velocity bytes are retained in state but have no
    // public InputControl; the XInput layout does not expose velocity1..5. XInput
    // also omits the touch strip, so xpad has no corresponding evdev field.
    [InputControlLayout(stateType = typeof(TranslatedProKeyboardState), displayName = "Evdev XInput Rock Band Pro Keyboard")]
    internal class EvdevXpadProKeyboard : TranslatingProKeyboard<EvdevXpadProKeyboardState>
    {
        internal new static void Initialize()
        {
            EvdevLayoutFinder.RegisterXInput<EvdevXpadProKeyboard>(15);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            EvdevLayoutFinder.RequireXInputAxes(this);
        }
    }
}
