using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedGuitarHeroGuitarState), displayName = "PC Guitar Hero Guitar")]
    internal class PCGuitarHeroGuitar : TranslatingGuitarHeroGuitar_Ranges<PS3GuitarHeroGuitarState_NoReportId>
    {
        internal new static void Initialize()
        {
            HidLayoutFinder.RegisterLayout<PCGuitarHeroGuitar_ReportId, PCGuitarHeroGuitar>(0x1430, 0x474C);
        }
    }

    [InputControlLayout(stateType = typeof(TranslatedGuitarHeroGuitarState), displayName = "PC Guitar Hero Guitar", hideInUI = true)]
    internal class PCGuitarHeroGuitar_ReportId : TranslatingGuitarHeroGuitar_Ranges<PS3GuitarHeroGuitarState_ReportId> { }
}
