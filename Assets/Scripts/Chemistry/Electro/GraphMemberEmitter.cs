using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChemistryEngine
{
    public class GraphMemberEmitter : AChemistryEmitter
    {
        [SerializeField] GraphMember _member;
        public string ORIGINAL_GRAPH_NAME
        {
            get
            {
                return _member._originalGraph;
            }
        }
        public string GRAPH_NAME
        {
            get
            {
                return _member.GraphName;
            }
        }
        public GraphMember MEMBER
        {
            get
            {
                return _member;
            }
        }
        private void Awake()
        {
            _emitType = IChemistryEmitter.Type.GRID_MEMBER;

            if (!_types.Contains(IChemistry.ChemistryTypes.ELECTRICITY)) _types.Add(IChemistry.ChemistryTypes.ELECTRICITY);
            if (_radiance.Count != _types.Count) _radiance.Add(0f);
        }

        public override void AddType(IChemistry.ChemistryTypes type, float radiance)
        {
            int position = Types.IndexOf(type);
            bool isFromZero = _radiance[position] == 0f;
            _radiance[position] += radiance;
            if (isFromZero)
            {
                foreach (IChemistryReceiver receiver in _activeReceiver)
                {
                    receiver.NewEmitType(this, type);
                }
            }

            UpdateVisualEffects(true, type);
        }

        public override void RemoveType(IChemistry.ChemistryTypes type, float radiance)
        {
            int position = Types.IndexOf(type);
            if (position != -1) _radiance[position] = Mathf.Clamp(_radiance[position] - radiance, 0f, 1f);
            bool isToZero = _radiance[position] == 0f;
            if (isToZero)
            {
                foreach (IChemistryReceiver receiver in _activeReceiver)
                {
                    receiver.RemoveEmitType(this, type);
                }
            }

            UpdateVisualEffects(false, type);
        }
    }
}
