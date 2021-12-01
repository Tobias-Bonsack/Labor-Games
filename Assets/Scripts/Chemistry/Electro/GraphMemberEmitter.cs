using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChemistryEngine
{
    public class GraphMemberEmitter : AbstractEmitter
    {
        [SerializeField] GraphMember _member;
        public string ORIGINAL_GRAPH_NAME
        {
            get
            {
                return _member._originalGraph;
            }
        }
        public Stack<string> GRPAH_STACK
        {
            get
            {
                return _member._graphNameStack;
            }
        }
        public string GRAPH_NAME
        {
            get
            {
                return _member.GraphName;
            }
        }
        private void Awake()
        {
            _emitType = IChemistryEmitter.Type.GRID_MEMBER;

            if (!_types.Contains(IChemistry.ChemistryTypes.ELECTRICITY)) _types.Add(IChemistry.ChemistryTypes.ELECTRICITY);
            if (_radiance.Count != _types.Count) _radiance.Add(0f);
        }

        public override void RemoveType(IChemistry.ChemistryTypes type, float radiance)
        {
            int position = Types.IndexOf(type);
            if (position != -1) _radiance[position] = Mathf.Clamp(_radiance[position] - radiance, 0f, 1f);

            UpdateVisualEffects(false, type);
        }
    }
}
