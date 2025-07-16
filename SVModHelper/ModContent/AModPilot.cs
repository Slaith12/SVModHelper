using Il2CppInterop.Runtime.Injection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SVModHelper.ModContent
{
    public abstract class AModPilot : AModContent
    {
        /// <summary>
        /// The class that this pilot belongs to.
        /// </summary>
        public abstract ClassName ClassName { get; }

        public abstract string DisplayName { get; }
        public abstract string Description { get; }

        /// <summary>
        /// This pilot's starting cards.
        /// </summary>
        public abstract Il2CppCollections.List<CardName> StartingCards { get; }

        /// <summary>
        /// This pilot's starting artifact.
        /// </summary>
        public abstract Il2CppCollections.List<ArtifactName> StartingArtifacts { get; }

    }
}
