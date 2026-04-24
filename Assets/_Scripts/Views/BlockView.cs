namespace Quantum
{
    using UnityEngine;

    public class BlockView : QuantumEntityViewComponent
    {
        public override void OnInitialize()
        {
            base.OnInitialize();
        }

        public override void OnActivate(Frame frame)
        {
            base.OnActivate(frame);
            //Debug.LogError("ACTIVATED");
        }

        public override void OnDeactivate()
        {
            base.OnDeactivate();
        }
    }
}
