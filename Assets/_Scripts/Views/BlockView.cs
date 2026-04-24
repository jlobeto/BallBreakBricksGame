using System;

namespace Quantum
{
    using UnityEngine;

    public class BlockView : QuantumEntityViewComponent
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private BlockHealthColors blockHealthColors;
        
        public override void OnInitialize()
        {
            base.OnInitialize();
            QuantumEvent.Subscribe<EventOnBlockReceivedDamage>(this, handler: OnDamageReceived);
        }

        public override void OnActivate(Frame frame)
        {
            base.OnActivate(frame);
        }


        public override void OnDeactivate()
        {
            base.OnDeactivate();
        }
        
        
        private void OnDamageReceived(EventOnBlockReceivedDamage data)
        {
            if (data.entityRef != _entityView.EntityRef)
                return;
            
            var currentHealth = data.currentLives;
            if (currentHealth <= 0)
                return;

            var color = blockHealthColors.GetColorByHealth(currentHealth);
            spriteRenderer.color = color;
        }

        private void OnDestroy()
        {
            QuantumEvent.UnsubscribeListener(this);
        }
    }
}
