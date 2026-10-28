using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct EvdevSantrollerGuitarHeroGuitarState : IGuitarHeroGuitarState
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

        public byte whammy      { get => (byte)state.GetAxis(0); set => state.SetAxis(0, value); }
        public byte rawTouchBar { get => (byte)state.GetAxis(1); set => state.SetAxis(1, value); }
        public sbyte tilt       { get => (sbyte)state.GetAxis(3); set => state.SetAxis(3, value); }
    }

    [InputControlLayout(stateType = typeof(TranslatedGuitarHeroGuitarState), displayName = "Santroller Evdev Guitar Hero Guitar")]
    internal class EvdevSantrollerGuitarHeroGuitar : TranslatingGuitarHeroGuitar_Discrete<EvdevSantrollerGuitarHeroGuitarState>
    {
        private static readonly int[] s_Keys =
        {
            0x130, 0x131, 0x134, 0x133, 0x136, 0x137, 0x13A, 0x13B, 0x13C
        };

        internal new static void Initialize()
        {
            EvdevLayoutFinder.RegisterSantroller<EvdevSantrollerGuitarHeroGuitar>(SantrollerDeviceType.GuitarHeroGuitar, 1);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            var caps = EvdevLayoutFinder.GetCapabilities(this);
            caps.RequireAxes(0, 1, 3, 0x10, 0x11);
            caps.RequireKeys(s_Keys);
        }
    }
}

