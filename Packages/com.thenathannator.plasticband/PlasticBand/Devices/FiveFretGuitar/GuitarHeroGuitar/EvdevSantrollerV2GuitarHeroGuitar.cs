using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct EvdevSantrollerV2GuitarHeroGuitarState : IGuitarHeroGuitarState
    {
        public static FourCC Format => new FourCC('E', 'V', 'D', 'V');
        public FourCC format => Format;

        public EvdevState state;

        public bool green   { get => state.GetKey(0x130); set => state.SetKey(0x130, value); }
        public bool red     { get => state.GetKey(0x131); set => state.SetKey(0x131, value); }
        public bool yellow  { get => state.GetKey(0x134); set => state.SetKey(0x134, value); }
        public bool blue    { get => state.GetKey(0x133); set => state.SetKey(0x133, value); }
        public bool orange  { get => state.GetKey(0x136); set => state.SetKey(0x136, value); }
        public bool spPedal { get => state.GetKey(0x137); set => state.SetKey(0x137, value); }

        public bool select { get => state.GetKey(0x13A); set => state.SetKey(0x13A, value); }
        public bool start  { get => state.GetKey(0x13B); set => state.SetKey(0x13B, value); }
        public bool system { get => state.GetKey(0x13C); set => state.SetKey(0x13C, value); }

        public bool dpadLeft  { get => state.GetAxis(0x10) < 0; set => state.SetAxis(0x10, value ? -1 : 0); }
        public bool dpadRight { get => state.GetAxis(0x10) > 0; set => state.SetAxis(0x10, value ? 1 : 0); }
        public bool dpadUp    { get => state.GetAxis(0x11) < 0; set => state.SetAxis(0x11, value ? -1 : 0); }
        public bool dpadDown  { get => state.GetAxis(0x11) > 0; set => state.SetAxis(0x11, value ? 1 : 0); }

        public byte rawTouchBar { get => (byte)(state.GetAxis(0) ^ 0x80); set => state.SetAxis(0, value ^ 0x80); }
        public byte whammy      { get => (byte)((state.GetAxis(3) >> 8) + 0x80); set => state.SetAxis(3, (value - 0x80) << 8); }
        public sbyte tilt       { get => (sbyte)(state.GetAxis(4) >> 8); set => state.SetAxis(4, value << 8); }
    }

    [InputControlLayout(stateType = typeof(TranslatedGuitarHeroGuitarState), displayName = "Santroller v2 Evdev Guitar Hero Guitar")]
    internal class EvdevSantrollerV2GuitarHeroGuitar : TranslatingGuitarHeroGuitar_Discrete<EvdevSantrollerV2GuitarHeroGuitarState>
    {
        private static readonly int[] s_Keys =
        {
            0x130, 0x131, 0x133, 0x134, 0x136, 0x137, 0x13A, 0x13B, 0x13C, 0x13D, 0x13E
        };

        internal new static void Initialize()
        {
            EvdevLayoutFinder.RegisterSantroller<EvdevSantrollerV2GuitarHeroGuitar>(SantrollerDeviceType.GuitarHeroGuitar, 2);
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
