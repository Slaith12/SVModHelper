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
        public int? complexity;
        //public ClassName? className;

        public Sprite frontPortrait;
        public Sprite frontPortraitParallax;
        public Sprite pilotTitleSprite;
        public Sprite combatPortraitNeutral;
        public Sprite combatPortraitPositive;
        public Sprite combatPortraitNegative;
        public Sprite combatPortraitBurning;
        public Sprite campaignPortrait;
        public Sprite victoryPhoto;
        public Sprite trueEndHandshake;
        public Sprite trueEndLineup;
        public string trueEndDialogue1;
        public string trueEndDialogue2;

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
            if (trueEndDialogue1 != null)
            {
                other.trueEndDialogue1 = trueEndDialogue1;
            }
            if (trueEndDialogue2 != null)
            {
                other.trueEndDialogue2 = trueEndDialogue2;
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

        internal void ApplyTo(ModPilotViewData pilotData)
        {
            if (complexity != null)
                pilotData.dataSO.Complexity = complexity.Value;
            if (frontPortrait != null)
                pilotData.dataSO.FrontPortrait = frontPortrait;
            if (frontPortraitParallax != null)
                pilotData.dataSO.FrontPortraitParallax = frontPortraitParallax;
            if (pilotTitleSprite != null)
                pilotData.dataSO.PilotTitleSprite = pilotTitleSprite;
            if (combatPortraitNeutral != null)
                pilotData.dataSO.CombatPortraitNeutral = combatPortraitNeutral;
            if (combatPortraitPositive != null)
                pilotData.dataSO.CombatPortraitPositive = combatPortraitPositive;
            if (combatPortraitNegative != null)
                pilotData.dataSO.CombatPortraitNegative = combatPortraitNegative;
            if (combatPortraitBurning != null)
                pilotData.dataSO.CombatPortraitBurning = combatPortraitBurning;
            if (campaignPortrait != null)
                pilotData.dataSO.CampaignPortrait = campaignPortrait;
            if (victoryPhoto != null)
                pilotData.dataSO.VictoryPhoto = victoryPhoto;

            if (startingCards != null)
                pilotData.dataSO.StarterData.deckCardDataList = startingCards;
            if (startingArtifacts != null)
                pilotData.dataSO.StarterData.artifactList = startingArtifacts;

            if (trueEndHandshake != null)
                pilotData.handshakeSprite = trueEndHandshake;
            if (trueEndLineup != null)
                pilotData.lineupSprite = trueEndLineup;
        }
    }
}
