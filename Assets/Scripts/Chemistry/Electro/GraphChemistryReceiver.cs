using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChemistryEngine
{
    public class GraphChemistryReceiver : AChemistryReceiver
    {
        protected override void TriggerEvents(IChemistryReceiver.Status status, Collider other)
        {
            if (other.gameObject.TryGetComponent<AChemistryEmitter>(out AChemistryEmitter chemistryEmitter))
            {
                for (int i = 0; i < chemistryEmitter.Types.Count; i++)
                {
                    IChemistry.ChemistryTypes type = chemistryEmitter.Types[i];
                    float radiance = chemistryEmitter.Radiance[i];
                    TriggerElementEvents(status, chemistryEmitter, type, radiance);
                }
            }
        }

        //TODO override from new emit type ... no enter or exit event

        public override void RemoveEmitType(AChemistryEmitter emitter, IChemistry.ChemistryTypes type)
        {
            if (emitter is GraphMemberEmitter) _activeEmitter.Remove((IChemistryEmitter)emitter);
        }
    }
}
