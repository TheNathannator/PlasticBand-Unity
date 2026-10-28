using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct EvdevXpadGuitarHeroGuitarState : IGuitarHeroGuitarState
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

        public bool dpadLeft  { get => state.GetAxis(0x10) < 0 || state.GetKey(0x222); set => state.SetAxis(0x10, value ? -1 : 0); }
        public bool dpadRight { get => state.GetAxis(0x10) > 0 || state.GetKey(0x223); set => state.SetAxis(0x10, value ? 1 : 0); }
        public bool dpadUp    { get => state.GetAxis(0x11) < 0 || state.GetKey(0x220); set => state.SetAxis(0x11, value ? -1 : 0); }
        public bool dpadDown  { get => state.GetAxis(0x11) > 0 || state.GetKey(0x221); set => state.SetAxis(0x11, value ? 1 : 0); }

        public byte rawTouchBar { get => (byte)((state.GetAxis(0) >> 8) ^ 0x80); set => state.SetAxis(0, (value ^ 0x80) << 8); }
        public byte whammy      { get => (byte)((state.GetAxis(3) >> 8) + 0x80); set => state.SetAxis(3, (value - 0x80) << 8); }
        public sbyte tilt       { get => (sbyte)((short)~state.GetAxis(4) >> 8); set => state.SetAxis(4, ~(value << 8)); }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct EvdevXpadGuitarHeroGuitarState_GH5 : IGuitarHeroGuitarState
    {
        public static FourCC Format => new FourCC('E', 'V', 'D', 'V');
        public FourCC format => Format;

        public EvdevXpadGuitarHeroGuitarState baseState;

        public bool green   { get => baseState.green; set => baseState.green = value; }
        public bool red     { get => baseState.red; set => baseState.red = value; }
        public bool yellow  { get => baseState.yellow; set => baseState.yellow = value; }
        public bool blue    { get => baseState.blue; set => baseState.blue = value; }
        public bool orange  { get => baseState.orange; set => baseState.orange = value; }
        public bool spPedal { get => baseState.spPedal; set => baseState.spPedal = value; }

        public bool select { get => baseState.select; set => baseState.select = value; }
        public bool start  { get => baseState.start; set => baseState.start = value; }
        public bool system { get => baseState.system; set => baseState.system = value; }

        public bool dpadLeft  { get => baseState.dpadLeft; set => baseState.dpadLeft = value; }
        public bool dpadRight { get => baseState.dpadRight; set => baseState.dpadRight = value; }
        public bool dpadUp    { get => baseState.dpadUp; set => baseState.dpadUp = value; }
        public bool dpadDown  { get => baseState.dpadDown; set => baseState.dpadDown = value; }

        public byte rawTouchBar { get => (byte)(baseState.state.GetAxis(0) ^ 0x80); set => baseState.state.SetAxis(0, value ^ 0x80); }
        public byte whammy      { get => baseState.whammy; set => baseState.whammy = value; }
        public sbyte tilt       { get => baseState.tilt; set => baseState.tilt = value; }
    }

    [InputControlLayout(stateType = typeof(TranslatedGuitarHeroGuitarState), displayName = "Evdev XInput Guitar Hero Guitar")]
    internal class EvdevXpadGuitarHeroGuitar : TranslatingGuitarHeroGuitar_Ranges<EvdevXpadGuitarHeroGuitarState>
    {
        internal new static void Initialize()
        {
            EvdevLayoutFinder.RegisterXInput<EvdevXpadGuitarHeroGuitar>(7);
            EvdevLayoutFinder.Register<EvdevXpadGuitarHeroGuitar>("xpad", 0x1430, 0x4748);
            EvdevLayoutFinder.Register<EvdevXpadGuitarHeroGuitar>("xpad", 0x1430, 0x4734);
            EvdevLayoutFinder.Register<EvdevXpadGuitarHeroGuitar_GH5>("xpad", 0x1430, 0x0705);
            EvdevLayoutFinder.Register<EvdevXpadGuitarHeroGuitar_GH5>("xpad", 0x1430, 0x0706);
            EvdevLayoutFinder.Register<EvdevXpadGuitarHeroGuitar>("Xbox 360 Wireless Alternate Guitar");
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            EvdevLayoutFinder.GetCapabilities(this).RequireAxes(0, 1, 3, 4);
        }
    }

    [InputControlLayout(stateType = typeof(TranslatedGuitarHeroGuitarState), displayName = "Evdev XInput Guitar Hero 5 Guitar", hideInUI = true)]
    internal class EvdevXpadGuitarHeroGuitar_GH5 : TranslatingGuitarHeroGuitar_Discrete<EvdevXpadGuitarHeroGuitarState_GH5>
    {
        protected override void FinishSetup()
        {
            base.FinishSetup();
            EvdevLayoutFinder.GetCapabilities(this).RequireAxes(0, 1, 3, 4);
        }
    }
}
