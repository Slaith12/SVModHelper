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

            result.StarterData = GetStarterPlayerData();

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

        public PlayerDataSO GetStarterPlayerData()
        {
	        PlayerDataSO playerDataSO = ScriptableObject.CreateInstance<PlayerDataSO>();

	        playerDataSO.starbucksAmount = 75;

	        playerDataSO.ClassName = ClassName;
            // Need to check ModContentManager to get the automatically assigned ID if not using an override.
	        playerDataSO.PilotName = ModContentManager.moddedPilotDict.First(kvp => kvp.Value.GetType() == this.GetType()).Key;

	        playerDataSO.startingMaxHeat = 0;


	        if (playerDataSO.ClassName == ClassName.Gunner)
	        {
		        playerDataSO.ClassBaseEnergy = EncounterValue.Heat;
		        playerDataSO.startingMaxHeat = 3;
	        }
	        else if (playerDataSO.ClassName == ClassName.Melee)
	        {
		        playerDataSO.ClassBaseEnergy = EncounterValue.Power;
		        playerDataSO.startingMaxPower = 3;
		        playerDataSO.startingPowerCell = 2;
	        }
	        else if (playerDataSO.ClassName == ClassName.Mystic)
	        {
		        playerDataSO.ClassBaseEnergy = EncounterValue.Mana;
		        playerDataSO.startingMaxMana = 5;
	        }

	        foreach (var card in StartingCards)
	        {
		        PlayerCardData cardData = new PlayerCardData(card);
		        playerDataSO.AddCardToDeck(cardData);
	        }

	        foreach (var arti in StartingArtifacts)
	        {
		        playerDataSO.AddArtifact(arti);
	        }

            return playerDataSO;
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
