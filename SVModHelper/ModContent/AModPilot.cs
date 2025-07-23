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

        public PilotDataSO GetPilotData()
        {
            var result = ScriptableObject.CreateInstance<PilotDataSO>();

            result.StarterData = GetStarterPlayerData();

			result.ClassName = ClassName;
            result.PilotName = ModContentManager.moddedPilotDict
                .First(kvp => kvp.Value.GetType() == this.GetType())
                .Key;

            result.Complexity = Complexity;

            // Use centralized sprite management with filenames as keys
            result.FrontPortrait = ModContentManager.GetPilotSprite(result.PilotName, FrontPortrait);
            result.FrontPortraitParallax = ModContentManager.GetPilotSprite(result.PilotName, FrontPortraitParallax);
            result.PilotTitleSprite = ModContentManager.GetPilotSprite(result.PilotName, PilotTitleSprite);
            result.CombatPortraitNeutral = ModContentManager.GetPilotSprite(result.PilotName, CombatPortraitNeutral);

            // Default to CombatPortraitNeutral if other CombatPortraits are not provided.
            result.CombatPortraitPositive = ModContentManager.GetPilotSprite(result.PilotName, 
                string.IsNullOrEmpty(CombatPortraitPositive) ? CombatPortraitNeutral : CombatPortraitPositive);
            result.CombatPortraitNegative = ModContentManager.GetPilotSprite(result.PilotName, 
                string.IsNullOrEmpty(CombatPortraitNegative) ? CombatPortraitNeutral : CombatPortraitNegative);
            result.CombatPortraitBurning = ModContentManager.GetPilotSprite(result.PilotName, 
                string.IsNullOrEmpty(CombatPortraitBurning) ? CombatPortraitNeutral : CombatPortraitBurning);

            result.CampaignPortrait = ModContentManager.GetPilotSprite(result.PilotName, CampaignPortrait);
            
            result.VictoryPhoto = ModContentManager.GetPilotSprite(result.PilotName, VictoryPhoto);

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
        
        public Sprite CreateSprite(string imageName)
        {
            if (string.IsNullOrEmpty(imageName))
                return SpriteHelper.GetTransparentSprite();
            return GetStandardSprite(imageName);
        }

    }
}
