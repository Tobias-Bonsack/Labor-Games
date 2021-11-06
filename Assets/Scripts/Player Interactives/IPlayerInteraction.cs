using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerInteraction
{
    public interface IPlayerInteraction
    {
        public void MudChanges(bool isEnterTrigger, float[] changeArray);
    }
}
