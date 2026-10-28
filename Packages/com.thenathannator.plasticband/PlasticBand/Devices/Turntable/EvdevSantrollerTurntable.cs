using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct EvdevSantrollerTurntableState : ITurntableState
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

        public bool leftGreen { get => state.GetKey(0x137); set => state.SetKey(0x137, value); }
        public bool leftRed   { get => state.GetKey(0x138); set => state.SetKey(0x138, value); }
        public bool leftBlue  { get => state.GetKey(0x139); set => state.SetKey(0x139, value); }

        public bool rightGreen { get => state.GetKey(0x132); set => state.SetKey(0x132, value); }
        public bool rightRed   { get => state.GetKey(0x135); set => state.SetKey(0x135, value); }
        public bool rightBlue  { get => state.GetKey(0x136); set => state.SetKey(0x136, value); }

        public sbyte leftVelocity  { get => (sbyte)(state.GetAxis(0) - 128); set => state.SetAxis(0, (short)(value + 128)); }
        public sbyte rightVelocity { get => (sbyte)(state.GetAxis(1) - 128); set => state.SetAxis(1, (short)(value + 128)); }

        public sbyte crossfader { get => (sbyte)(state.GetAxis(4) - 128); set => state.SetAxis(4, (short)(value + 128)); }
        public ushort effectsDial { get => (ushort)(state.GetAxis(3) << 8); set => state.SetAxis(3, (short)(value >> 8)); }
    }

    [InputControlLayout(stateType = typeof(TranslatedTurntableState), displayName = "Santroller Evdev DJ Hero Turntable")]
    internal class EvdevSantrollerTurntable : TranslatingTurntable<EvdevSantrollerTurntableState>
    {
        private static readonly int[] s_Keys =
        {
            0x130, 0x131, 0x133, 0x134, 0x13A, 0x13B, 0x13C, 0x132, 0x135, 0x136, 0x137, 0x138, 0x139
        };

        internal new static void Initialize()
        {
            EvdevLayoutFinder.RegisterSantroller<EvdevSantrollerTurntable>(SantrollerDeviceType.DjHeroTurntable, 1);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            var caps = EvdevLayoutFinder.GetCapabilities(this);
            caps.RequireAxes(0, 1, 3, 4, 0x10, 0x11);
            caps.RequireKeys(s_Keys);
        }
    }
}
