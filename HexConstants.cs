using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Wunderwunsch.HexMapLibrary
{
    internal static class HexConstants 
    {
        /// <summary>
        /// used to slightly push a coordinate off-center to avoid ambiguities/inconsistencies when interpolated coordinates would end up exactly between 2 tile/edge/corner coordinates 
        /// </summary>
        public const float NudgePositive = +0.001f;
        /// <summary>
        /// used to slightly push a coordinate off-center to avoid ambiguities/inconsistencies when interpolated coordinates would end up exactly between 2 tile/edge/corner coordinates 
        /// </summary>
        public const float NudgeNegative = -0.001f;
    }
}