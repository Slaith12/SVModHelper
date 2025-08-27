using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SVModHelper.ModContent
{
    public class ModPilotViewData
    {
        /// <summary>
        /// The main data object that contains most of the pilot's sprites, as well as some gameplay data such as their class and starting loadout.
        /// </summary>
        public PilotDataSO dataSO;
        /// <summary>
        /// The sprite shown in the second panel of the true ending cutscene when playing as this pilot.
        /// </summary>
        public Sprite handshakeSprite;
        /// <summary>
        /// The sprite shown in the final panel of the true ending cutscene when playing as this pilot or you've gotten the "win" achievement for this pilot.
        /// <para>NOTE: Modded pilots do not have "win" achievements, so they only show up in the final panel when playing as that pilot.</para>
        /// </summary>
        public Sprite lineupSprite;
    }
}
