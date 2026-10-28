using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct EvdevSantrollerV2ProGuitarState : IProGuitarState
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

        public ushort frets1 => (ushort)((byte)state.GetAxis(2) | ((byte)state.GetAxis(5) << 8));
        public ushort frets2 => (ushort)((ushort)state.GetAxis(0) & 0x7FFF);

        public bool solo { get => ((ushort)state.GetAxis(0) & 0x8000) != 0; set {} }

        public byte fret1 { get => (byte)frets1.GetMask(0x1F, 0); set {} }
        public byte fret2 { get => (byte)frets1.GetMask(0x1F, 5); set {} }
        public byte fret3 { get => (byte)frets1.GetMask(0x1F, 10); set {} }
        public byte fret4 { get => (byte)frets2.GetMask(0x1F, 0); set {} }
        public byte fret5 { get => (byte)frets2.GetMask(0x1F, 5); set {} }
        public byte fret6 { get => (byte)frets2.GetMask(0x1F, 10); set {} }

        public byte velocity1 { get => (byte)(state.GetAxis(1) & 0x7F); set {} }
        public byte velocity2 { get => (byte)((state.GetAxis(1) >> 8) & 0x7F); set {} }
        public byte velocity3 { get => (byte)(state.GetAxis(3) & 0x7F); set {} }
        public byte velocity4 { get => (byte)((state.GetAxis(3) >> 8) & 0x7F); set {} }
        public byte velocity5 { get => (byte)(state.GetAxis(4) & 0x7F); set {} }
        public byte velocity6 { get => (byte)((state.GetAxis(4) >> 8) & 0x7F); set {} }

        public bool green  { get => (state.GetAxis(1) & 0x80) != 0; set {} }
        public bool red    { get => ((state.GetAxis(1) >> 8) & 0x80) != 0; set {} }
        public bool yellow { get => (state.GetAxis(3) & 0x80) != 0; set {} }
        public bool blue   { get => ((state.GetAxis(3) >> 8) & 0x80) != 0; set {} }
        public bool orange { get => (state.GetAxis(4) & 0x80) != 0; set {} }

        public bool tilt { get => false; set {} }
        public bool digitalPedal { get => false; set {} }
    }

    [InputControlLayout(stateType = typeof(TranslatedProGuitarState), displayName = "Santroller v2 Evdev Rock Band Pro Guitar")]
    internal class EvdevSantrollerV2ProGuitar : TranslatingProGuitar<EvdevSantrollerV2ProGuitarState>
    {
        private static readonly int[] s_Keys =
        {
            0x130, 0x131, 0x133, 0x134, 0x136, 0x137, 0x13A, 0x13B, 0x13C, 0x13D, 0x13E
        };

        internal new static void Initialize()
        {
            EvdevLayoutFinder.RegisterSantroller<EvdevSantrollerV2ProGuitar>(SantrollerDeviceType.ProGuitarMustang, 2);
            EvdevLayoutFinder.RegisterSantroller<EvdevSantrollerV2ProGuitar>(SantrollerDeviceType.ProGuitarSquire, 2);
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
