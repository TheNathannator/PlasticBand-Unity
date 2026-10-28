using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct EvdevSantrollerV2TurntableState : ITurntableState
    {
        public static FourCC Format => new FourCC('E', 'V', 'D', 'V');
        public FourCC format => Format;

        public EvdevState state;

        public bool south { get => state.GetKey(0x130); set => state.SetKey(0x130, value); }
        public bool east  { get => state.GetKey(0x131); set => state.SetKey(0x131, value); }
        public bool west  { get => state.GetKey(0x133); set => state.SetKey(0x133, value); }
        public bool north_euphoria { get => state.GetKey(0x134); set => state.SetKey(0x134, value); }

        public bool select { get => state.GetKey(0x13A); set => state.SetKey(0x13A, value); }
        public bool start  { get => state.GetKey(0x13B); set => state.SetKey(0x13B, value); }
        public bool system { get => state.GetKey(0x13C); set => state.SetKey(0x13C, value); }

        public bool dpadLeft  { get => state.GetAxis(0x10) < 0; set => state.SetAxis(0x10, value ? -1 : 0); }
        public bool dpadRight { get => state.GetAxis(0x10) > 0; set => state.SetAxis(0x10, value ? 1 : 0); }
        public bool dpadUp    { get => state.GetAxis(0x11) < 0; set => state.SetAxis(0x11, value ? -1 : 0); }
        public bool dpadDown  { get => state.GetAxis(0x11) > 0; set => state.SetAxis(0x11, value ? 1 : 0); }

        public bool leftGreen { get => (state.GetAxis(2) & 0x01) != 0; set {} }
        public bool leftRed   { get => (state.GetAxis(2) & 0x02) != 0; set {} }
        public bool leftBlue  { get => (state.GetAxis(2) & 0x04) != 0; set {} }

        public bool rightGreen { get => (state.GetAxis(5) & 0x01) != 0; set {} }
        public bool rightRed   { get => (state.GetAxis(5) & 0x02) != 0; set {} }
        public bool rightBlue  { get => (state.GetAxis(5) & 0x04) != 0; set {} }

        public sbyte leftVelocity  { get => (sbyte)(state.GetAxis(0) & 0xFF); set {} }
        public sbyte rightVelocity { get => (sbyte)(state.GetAxis(1) & 0xFF); set {} }

        public sbyte crossfader { get => (sbyte)(state.GetAxis(4) >> 8); set {} }
        public ushort effectsDial { get => (ushort)state.GetAxis(3); set {} }
    }

    [InputControlLayout(stateType = typeof(TranslatedTurntableState), displayName = "Santroller v2 Evdev DJ Hero Turntable")]
    internal class EvdevSantrollerV2Turntable : TranslatingTurntable<EvdevSantrollerV2TurntableState>
    {
        private static readonly int[] s_Keys =
        {
            0x130, 0x131, 0x133, 0x134, 0x136, 0x137, 0x13A, 0x13B, 0x13C, 0x13D, 0x13E
        };

        internal new static void Initialize()
        {
            EvdevLayoutFinder.RegisterSantroller<EvdevSantrollerV2Turntable>(SantrollerDeviceType.DjHeroTurntable, 2);
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
