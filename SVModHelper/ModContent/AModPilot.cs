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
        /// If assigned, this pilot will be registered with this PilotName instead of the default one. Must be greater or equal to 1000.
        /// This is useful when setting up custom pilot names.
        /// </summary>
        public virtual PilotName PilotNameOverride => ModContentManager.INVALIDPILOTID;

        /// <summary>
        /// The class that this pilot belongs to.
        /// </summary>
        public abstract ClassName ClassName { get; }

        public abstract string DisplayName { get; }
        public abstract string Description { get; }

        public abstract int Complexity { get; }

        public virtual string FrontPortrait => null;
        public virtual string FrontPortraitParallax => null;
		public virtual string PilotTitleSprite => null;
		public virtual string CombatPortraitNeutral => null;
        public virtual string CombatPortraitPositive => null;
        public virtual string CombatPortraitNegative => null;
        public virtual string CombatPortraitBurning => null;
        public virtual string CampaignPortrait => null;
        public virtual string VictoryPhoto => null;

        /// <summary>
        /// This pilot's starting cards.
        /// </summary>
        public abstract Il2CppCollections.List<CardName> StartingCards { get; }

        /// <summary>
        /// This pilot's starting artifact.
        /// </summary>
        public abstract Il2CppCollections.List<ArtifactName> StartingArtifacts { get; }

        private Dictionary<string, Sprite> _spriteCache = new();

        public PilotDataSO GetPilotData()
        {
            var result = ScriptableObject.CreateInstance<PilotDataSO>();
            result.ClassName = ClassName;
            result.PilotName = ModContentManager.moddedPilotDict
                .First(kvp => kvp.Value.GetType() == this.GetType())
                .Key;

            result.Complexity = Complexity;

            // Use cached sprites or create new ones.
            result.FrontPortrait = GetCachedSprite(FrontPortrait);
            result.FrontPortraitParallax = GetCachedSprite(FrontPortraitParallax);
            result.PilotTitleSprite = GetCachedSprite(PilotTitleSprite);
            result.CombatPortraitNeutral = GetCachedSprite(CombatPortraitNeutral);

            // Default to CombatPortraitNeutral if other CombatPortraits are not provided.
            result.CombatPortraitPositive = GetCachedSprite(string.IsNullOrEmpty(CombatPortraitPositive) ? CombatPortraitNeutral : CombatPortraitPositive);
            result.CombatPortraitNegative = GetCachedSprite(string.IsNullOrEmpty(CombatPortraitNegative) ? CombatPortraitNeutral : CombatPortraitNegative);
            result.CombatPortraitBurning = GetCachedSprite(string.IsNullOrEmpty(CombatPortraitBurning) ? CombatPortraitNeutral : CombatPortraitBurning);

            result.CampaignPortrait = GetCachedSprite(CampaignPortrait);
            
            result.VictoryPhoto = GetCachedSprite(VictoryPhoto);

            return result;
        }
        
        
        private Sprite GetCachedSprite(string imageName)
        {
            if (string.IsNullOrEmpty(imageName))
                return SpriteHelper.GetTransparentSprite();
                
            if (_spriteCache.TryGetValue(imageName, out Sprite cachedSprite))
                return cachedSprite;
                
            Sprite newSprite = GetStandardSprite(imageName);
            _spriteCache[imageName] = newSprite;
            return newSprite;
        }
    }
}
