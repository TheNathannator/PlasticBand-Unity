using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct EvdevXpadFiveLaneDrumkitState : IFiveLaneDrumkitState
    {
        public static FourCC Format => new FourCC('E', 'V', 'D', 'V');
        public FourCC format => Format;

        public EvdevState state;

        public bool green_south { get => state.GetKey(0x130); set => state.SetKey(0x130, value); }
        public bool red_east    { get => state.GetKey(0x131); set => state.SetKey(0x131, value); }
        public bool yellow_north{ get => state.GetKey(0x134); set => state.SetKey(0x134, value); }
        public bool blue_west   { get => state.GetKey(0x133); set => state.SetKey(0x133, value); }
        public bool orange      { get => state.GetKey(0x137); set => state.SetKey(0x137, value); }
        public bool kick        { get => state.GetKey(0x136); set => state.SetKey(0x136, value); }

        public bool select { get => state.GetKey(0x13A); set => state.SetKey(0x13A, value); }
        public bool start  { get => state.GetKey(0x13B); set => state.SetKey(0x13B, value); }
        public bool system { get => state.GetKey(0x13C); set => state.SetKey(0x13C, value); }

        public bool dpadLeft  { get => state.GetAxis(0x10) < 0 || state.GetKey(0x222); set => state.SetAxis(0x10, value ? -1 : 0); }
        public bool dpadRight { get => state.GetAxis(0x10) > 0 || state.GetKey(0x223); set => state.SetAxis(0x10, value ? 1 : 0); }
        public bool dpadUp    { get => state.GetAxis(0x11) < 0 || state.GetKey(0x220); set => state.SetAxis(0x11, value ? -1 : 0); }
        public bool dpadDown  { get => state.GetAxis(0x11) > 0 || state.GetKey(0x221); set => state.SetAxis(0x11, value ? 1 : 0); }

        public byte greenVelocity  { get => (byte)~state.GetAxis(1); set {} }
        public byte redVelocity    { get => (byte)(~state.GetAxis(1) >> 8); set {} }
        public byte yellowVelocity { get => (byte)state.GetAxis(3); set {} }
        public byte blueVelocity   { get => (byte)(state.GetAxis(3) >> 8); set {} }
        public byte orangeVelocity { get => (byte)~state.GetAxis(4); set {} }
        public byte kickVelocity   { get => (byte)(~state.GetAxis(4) >> 8); set {} }
    }

    [InputControlLayout(stateType = typeof(TranslatedFiveLaneState), displayName = "Evdev XInput Guitar Hero Drumkit")]
    internal class EvdevXpadFiveLaneDrumkit : TranslatingFiveLaneDrumkit<EvdevXpadFiveLaneDrumkitState>
    {
        internal new static void Initialize()
        {
            EvdevLayoutFinder.RegisterXInput<EvdevXpadFiveLaneDrumkit>(8, "Xbox 360 Wireless Guitar Hero Drum Kit");
            EvdevLayoutFinder.RegisterXInput<EvdevXpadFiveLaneDrumkit>(8, 0x1430, 0x0805);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            EvdevLayoutFinder.RequireXInputAxes(this);
        }
    }
}
