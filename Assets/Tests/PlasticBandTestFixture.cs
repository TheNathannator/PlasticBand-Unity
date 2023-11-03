using UnityEngine.InputSystem;

namespace PlasticBand.Tests
{
    public class PlasticBandTestFixture : InputTestFixture
    {
        public override void Setup()
        {
            base.Setup();

            // The input test fixture resets the input system, so we must re-initialize everything here
            Initialization.Initialize();
        }
    }
}