using Il2CppStarVaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SVModHelper.ModContent
{
    public class PilotModification
    {
        internal SVMod m_Source;
        //public SVMod sourceMod => m_Source;
        public PilotName targetPilot;
        public int priority;

        public string displayName;
        public string description;
        //Pilot names aren't localized
        public Dictionary<string, string> localizedDescriptions = new();
        public int? complexity;
        //public ClassName? className;

        public SpriteDescriptor? frontPortrait;
        public SpriteDescriptor? frontPortraitParallax;
        public SpriteDescriptor? pilotTitleSprite;
        public SpriteDescriptor? combatPortraitNeutral;
        public SpriteDescriptor? combatPortraitPositive;
        public SpriteDescriptor? combatPortraitNegative;
        public SpriteDescriptor? combatPortraitBurning;
        public SpriteDescriptor? campaignPortrait;
        public SpriteDescriptor? victoryPhoto;
        public SpriteDescriptor? trueEndHandshake;
        public SpriteDescriptor? trueEndLineup;
        public Dictionary<string, (string dialogue1, string dialogue2)> localizedTrueEndDialogues = new();

        public Il2CppCollections.List<PlayerCardData> startingCards;

        public Il2CppCollections.List<ArtifactName> startingArtifacts;

        public PilotModification(PilotName target, int priority = 0)
        {
            this.targetPilot = target;
            this.priority = priority;
        }

        public void CopyTo(PilotModification other)
        {
            if (displayName != null)
            {
                other.displayName = displayName;
            }
            if (description != null)
            {
                other.description = description;
            }
            foreach (var locDesc in localizedDescriptions)
            {
                other.localizedDescriptions[locDesc.Key] = locDesc.Value;
            }
            if (complexity != null)
            {
                other.complexity = complexity;
            }
            //if (className != null)
            //{
            //    other.className = className;
            //}
            if (frontPortrait != null)
            {
                other.frontPortrait = frontPortrait;
            }
            if (frontPortraitParallax != null)
            {
                other.frontPortraitParallax = frontPortraitParallax;
            }
            if (pilotTitleSprite != null)
            {
                other.pilotTitleSprite = pilotTitleSprite;
            }
            if (combatPortraitNeutral != null)
            {
                other.combatPortraitNeutral = combatPortraitNeutral;
            }
            if (combatPortraitPositive != null)
            {
                other.combatPortraitPositive = combatPortraitPositive;
            }
            if (combatPortraitNegative != null)
            {
                other.combatPortraitNegative = combatPortraitNegative;
            }
            if (combatPortraitBurning != null)
            {
                other.combatPortraitBurning = combatPortraitBurning;
            }
            if (campaignPortrait != null)
            {
                other.campaignPortrait = campaignPortrait;
            }
            if (victoryPhoto != null)
            {
                other.victoryPhoto = victoryPhoto;
            }
            if (trueEndHandshake != null)
            {
                other.trueEndHandshake = trueEndHandshake;
            }
            if (trueEndLineup != null)
            {
                other.trueEndLineup = trueEndLineup;
            }
            foreach (var locDialogue in localizedTrueEndDialogues)
            {
                other.localizedTrueEndDialogues[locDialogue.Key] = locDialogue.Value;
            }
            if (startingCards != null)
            {
                other.startingCards = startingCards;
            }
            if (startingArtifacts != null)
            {
                other.startingArtifacts = startingArtifacts;
            }
        }

        internal void ApplyTo(ModPilotDescriptor pilotData)
        {
            if (complexity != null)
                pilotData.Complexity = complexity.Value;
            if (frontPortrait != null)
                pilotData.FrontPortrait = frontPortrait.Value;
            if (frontPortraitParallax != null)
                pilotData.FrontPortraitParallax = frontPortraitParallax.Value;
            if (pilotTitleSprite != null)
                pilotData.PilotTitleSprite = pilotTitleSprite.Value;
            if (combatPortraitNeutral != null)
                pilotData.CombatPortraitNeutral = combatPortraitNeutral.Value;
            if (combatPortraitPositive != null)
                pilotData.CombatPortraitPositive = combatPortraitPositive.Value;
            if (combatPortraitNegative != null)
                pilotData.CombatPortraitNegative = combatPortraitNegative.Value;
            if (combatPortraitBurning != null)
                pilotData.CombatPortraitBurning = combatPortraitBurning.Value;
            if (campaignPortrait != null)
                pilotData.CampaignPortrait = campaignPortrait.Value;
            if (victoryPhoto != null)
                pilotData.VictoryPhoto = victoryPhoto.Value;

            if (startingCards != null)
                pilotData.StartingCards = startingCards;
            if (startingArtifacts != null)
                pilotData.StartingArtifacts = startingArtifacts;
        }

        internal void ApplyTo(PilotDataSO pilotData)
        {
            if (complexity != null)
                pilotData.Complexity = complexity.Value;
            if (frontPortrait != null)
                pilotData.FrontPortrait = SpriteHelper.GetSprite(frontPortrait.Value);
            if (frontPortraitParallax != null)
                pilotData.FrontPortraitParallax = SpriteHelper.GetSprite(frontPortraitParallax.Value);
            if (pilotTitleSprite != null)
                pilotData.PilotTitleSprite = SpriteHelper.GetSprite(pilotTitleSprite.Value);
            if (combatPortraitNeutral != null)
                pilotData.CombatPortraitNeutral = SpriteHelper.GetSprite(combatPortraitNeutral.Value);
            if (combatPortraitPositive != null)
                pilotData.CombatPortraitPositive = SpriteHelper.GetSprite(combatPortraitPositive.Value);
            if (combatPortraitNegative != null)
                pilotData.CombatPortraitNegative = SpriteHelper.GetSprite(combatPortraitNegative.Value);
            if (combatPortraitBurning != null)
                pilotData.CombatPortraitBurning = SpriteHelper.GetSprite(combatPortraitBurning.Value);
            if (campaignPortrait != null)
                pilotData.CampaignPortrait = SpriteHelper.GetSprite(campaignPortrait.Value);
            if (victoryPhoto != null)
                pilotData.VictoryPhoto = SpriteHelper.GetSprite(victoryPhoto.Value);

            if (startingCards != null)
                pilotData.StarterData.deckCardDataList = startingCards;
            if (startingArtifacts != null)
                pilotData.StarterData.artifactList = startingArtifacts;
        }

        internal void LoadSprites(SpriteHelper.LogLevel logLevel)
        {
            if (frontPortrait != null)
                SpriteHelper.LoadSprite(frontPortrait.Value, out _, logLevel);
            if (frontPortraitParallax != null)
                SpriteHelper.LoadSprite(frontPortraitParallax.Value, out _, logLevel);
            if (pilotTitleSprite != null)
                SpriteHelper.LoadSprite(pilotTitleSprite.Value, out _, logLevel);
            if (combatPortraitNeutral != null)
                SpriteHelper.LoadSprite(combatPortraitNeutral.Value, out _, logLevel);
            if (combatPortraitPositive != null)
                SpriteHelper.LoadSprite(combatPortraitPositive.Value, out _, logLevel);
            if (combatPortraitNegative != null)
                SpriteHelper.LoadSprite(combatPortraitNegative.Value, out _, logLevel);
            if (combatPortraitBurning != null)
                SpriteHelper.LoadSprite(combatPortraitBurning.Value, out _, logLevel);
            if (campaignPortrait != null)
                SpriteHelper.LoadSprite(campaignPortrait.Value, out _, logLevel);
            if (victoryPhoto != null)
                SpriteHelper.LoadSprite(victoryPhoto.Value, out _, logLevel);
        }
    }
}
