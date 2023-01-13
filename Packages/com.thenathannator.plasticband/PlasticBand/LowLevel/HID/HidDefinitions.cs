using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.LowLevel
{
    /// <summary>
    /// Various definitions for HID devices.
    /// </summary>
    internal static class HidDefinitions
    {
        /// <summary>
        /// The format for HID input reports.
        /// </summary>
        public static readonly FourCC InputFormat = new FourCC('H', 'I', 'D');

        /// <summary>
        /// The format for HID output reports.
        /// </summary>
        public static readonly FourCC OutputFormat = new FourCC('H', 'I', 'D', 'O');
    }
}