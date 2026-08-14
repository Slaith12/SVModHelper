using Il2CppInterop.Runtime;
using Il2CppStarVaders;
using SVModHelper;
using SVModHelper.ModContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LuffyTest
{
	public class LuffyPilot : AModPilot
	{

		public override string DisplayName => "Luffy";
		public override string Description => "- MEAAAAAT!!!!!!";
		public override int Complexity => 2;

		public override ClassName ClassName => ClassName.Melee;

        public override string BaseImagePath => "Luffy";
        public override SpriteDescriptor FrontPortrait => GetStandardSprite("LuffySelect2.png");
        public override SpriteDescriptor CombatPortraitNeutral => GetStandardSprite("LuffyCombat.png");

        //surpress warnings for missing sprites
		//(the combat portraits default to the neutral portrait anyways; none of this is strictly necessary, it's just nice for not showing errors)
        public override SpriteDescriptor CombatPortraitPositive => CombatPortraitNeutral;
        public override SpriteDescriptor CombatPortraitNegative => CombatPortraitNeutral;
        public override SpriteDescriptor CombatPortraitBurning => CombatPortraitNeutral;
        public override SpriteDescriptor VictoryPhoto => new();
        public override SpriteDescriptor FrontPortraitParallax => new();
        public override SpriteDescriptor TrueEndHandshake => new();
        public override SpriteDescriptor TrueEndLineup => new();

		public override Il2CppCollections.List<PlayerCardData> StartingCards => 
			new List<PlayerCardData>{
				new(CardName.Dash),
				new(CardName.Dash),
				new(CardName.Dash),
                new(CardName.Dash),
				new(ModContentManager.GetModCardName<Pistol>()),
				new(ModContentManager.GetModCardName<Pistol>()),
				new(ModContentManager.GetModCardName<Pistol>()),
				new(ModContentManager.GetModCardName<Pistol>()),
				new(ModContentManager.GetModCardName<Gatling>()),
                new(ModContentManager.GetModCardName<Rocket>()),
			}.ToILCPP();

		public override Il2CppCollections.List<ArtifactName> StartingArtifacts => 
			new List<ArtifactName>()
			{
				ModContentManager.GetModArtifactName<GearFive>()
			}.ToILCPP();
	}
}
