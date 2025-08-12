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
        public override Sprite FrontPortrait => GetStandardSprite("LuffySelect2.png");

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
