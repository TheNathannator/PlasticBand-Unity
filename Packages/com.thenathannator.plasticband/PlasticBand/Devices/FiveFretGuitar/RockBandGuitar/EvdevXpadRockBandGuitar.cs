using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct EvdevXpadRockBandGuitarState : IRockBandGuitarState_Flags
    {
        public static FourCC Format => new FourCC('E', 'V', 'D', 'V');
        public FourCC format => Format;

        public EvdevState state;

        public bool green  { get => state.GetKey(0x130); set => state.SetKey(0x130, value); }
        public bool red    { get => state.GetKey(0x131); set => state.SetKey(0x131, value); }
        public bool yellow { get => state.GetKey(0x134); set => state.SetKey(0x134, value); }
        public bool blue   { get => state.GetKey(0x133); set => state.SetKey(0x133, value); }
        public bool orange { get => state.GetKey(0x136); set => state.SetKey(0x136, value); }

        public bool solo { get => state.GetKey(0x13D); set => state.SetKey(0x13D, value); }

        public bool select { get => state.GetKey(0x13A); set => state.SetKey(0x13A, value); }
        public bool start  { get => state.GetKey(0x13B); set => state.SetKey(0x13B, value); }
        public bool system { get => state.GetKey(0x13C); set => state.SetKey(0x13C, value); }

        public bool dpadLeft  { get => state.GetAxis(0x10) < 0 || state.GetKey(0x222); set => state.SetAxis(0x10, value ? -1 : 0); }
        public bool dpadRight { get => state.GetAxis(0x10) > 0 || state.GetKey(0x223); set => state.SetAxis(0x10, value ? 1 : 0); }
        public bool dpadUp    { get => state.GetAxis(0x11) < 0 || state.GetKey(0x220); set => state.SetAxis(0x11, value ? -1 : 0); }
        public bool dpadDown  { get => state.GetAxis(0x11) > 0 || state.GetKey(0x221); set => state.SetAxis(0x11, value ? 1 : 0); }

        public byte whammy       { get => (byte)((state.GetAxis(3) >> 8) + 0x80); set => state.SetAxis(3, (value - 0x80) << 8); }
        public sbyte tilt        { get => (sbyte)((short)~state.GetAxis(4) >> 8); set => state.SetAxis(4, ~(value << 8)); }
        public int pickupSwitch { get => RockBandGuitarState.GetPickupSwitchNotch((byte)state.GetAxis(2)); set => state.SetAxis(2, RockBandGuitarState.SetPickupSwitchNotch(value)); }
    }

    [InputControlLayout(stateType = typeof(TranslatedRockBandGuitarState), displayName = "Evdev XInput Rock Band Guitar")]
    internal class EvdevXpadRockBandGuitar : TranslatingRockBandGuitar_Flags<EvdevXpadRockBandGuitarState>
    {
        internal new static void Initialize()
        {
            EvdevLayoutFinder.RegisterXInput<EvdevXpadRockBandGuitar>(6);
            EvdevLayoutFinder.RegisterXInput<EvdevXpadRockBandGuitar>(11);
            EvdevLayoutFinder.Register<EvdevXpadRockBandGuitar>("xpad", 0x1BAD, 0x0002);
            EvdevLayoutFinder.Register<EvdevXpadRockBandGuitar>("Xbox 360 Wireless Guitar");
            EvdevLayoutFinder.Register<EvdevXpadRockBandGuitar>("Xbox 360 Wireless Bass Guitar");
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            EvdevLayoutFinder.GetCapabilities(this).RequireAxes(2, 3, 4);
        }
    }
}
