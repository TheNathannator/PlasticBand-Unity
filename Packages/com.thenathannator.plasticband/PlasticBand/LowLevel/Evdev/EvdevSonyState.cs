using PlasticBand.Devices;
using PlasticBand.Devices.LowLevel;

namespace PlasticBand.LowLevel
{
    internal static class EvdevSonyState
    {
        internal static PS4Button1 GetButtons(ref EvdevState state, int buttonBase)
            => (PS4Button1)((((ushort)state.GetSonyButtons(buttonBase) & 0xFFF) << 4) |
                (byte)state.GetDpad());

        internal static PS4Button2 GetSystemButtons(ref EvdevState state, int buttonBase)
            => (state.GetSonyButtons(buttonBase) & PS3Button.PlayStation) != 0
                ? PS4Button2.PlayStation : 0;

        internal static byte GetFrets(PS4Button1 buttons)
            => (byte)TranslatingRockBandGuitar.TranslateFrets(
                (buttons & PS4Button1.Cross) != 0, (buttons & PS4Button1.Circle) != 0,
                (buttons & PS4Button1.Triangle) != 0, (buttons & PS4Button1.Square) != 0,
                (buttons & PS4Button1.L1) != 0);
    }
}
