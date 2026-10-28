using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct EvdevSantrollerV2ProKeyboardState : IProKeyboardState
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

        public bool dpadLeft  { get => state.GetAxis(0x10) < 0; set => state.SetAxis(0x10, value ? -1 : 0); }
        public bool dpadRight { get => state.GetAxis(0x10) > 0; set => state.SetAxis(0x10, value ? 1 : 0); }
        public bool dpadUp    { get => state.GetAxis(0x11) < 0; set => state.SetAxis(0x11, value ? -1 : 0); }
        public bool dpadDown  { get => state.GetAxis(0x11) > 0; set => state.SetAxis(0x11, value ? 1 : 0); }

        public byte keys1 { get => (byte)state.GetAxis(2); set {} }
        public byte keys2 { get => (byte)state.GetAxis(5); set {} }
        public byte keys3 { get => (byte)state.GetAxis(0); set {} }

        public byte velocity1 { get => (byte)(state.GetAxis(0) >> 8); set {} }
        public byte velocity2 { get => (byte)state.GetAxis(1); set {} }
        public byte velocity3 { get => (byte)(state.GetAxis(1) >> 8); set {} }
        public byte velocity4 { get => (byte)state.GetAxis(3); set {} }
        public byte velocity5 { get => (byte)(state.GetAxis(3) >> 8); set {} }

        public byte overdrive { get => (byte)state.GetAxis(4); set {} }
        public byte pedal     { get => (byte)(state.GetAxis(4) >> 8); set {} }
        public byte touchStrip { get => 0; set {} }
    }

    [InputControlLayout(stateType = typeof(TranslatedProKeyboardState), displayName = "Santroller v2 Evdev Rock Band Pro Keyboard")]
    internal class EvdevSantrollerV2ProKeyboard : TranslatingProKeyboard<EvdevSantrollerV2ProKeyboardState>
    {
        private static readonly int[] s_Keys =
        {
            0x130, 0x131, 0x133, 0x134, 0x136, 0x137, 0x13A, 0x13B, 0x13C, 0x13D, 0x13E
        };

        internal new static void Initialize()
        {
            EvdevLayoutFinder.RegisterSantroller<EvdevSantrollerV2ProKeyboard>(SantrollerDeviceType.ProKeyboard, 2);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            var caps = EvdevLayoutFinder.GetCapabilities(this);
            caps.RequireAxes(0, 1, 2, 3, 4, 5, 0x10, 0x11);
            caps.RequireKeys(s_Keys);
        }
    }
}
