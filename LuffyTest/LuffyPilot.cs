using Il2CppInterop.Runtime;
using Il2CppStarVaders;
using SVModHelper;
using SVModHelper.ModContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuffyTest
{
	public class LuffyPilot : AModPilot
	{
		public override PilotName PilotNameOverride => (PilotName) 11000;
		public override string DisplayName => "Luffy";
		public override string Description => "- MEAAAAAT!!!!!!";
		public override int Complexity => 2;

		public override ClassName ClassName => ClassName.Melee;

		public override string FrontPortrait => "LuffySelect2.png";

		public override string CampaignPortrait => "LuffyCampaign.png";
		public override string CombatPortraitNeutral => "LuffyCombat.png";

		public override string PilotTitleSprite => "LuffyName.png";

		public override Il2CppCollections.List<CardName> StartingCards => 
			new List<CardName>{
				CardName.Dash,
				CardName.Dash,
				CardName.Dash,
				CardName.Dash,
				(CardName)17001,
				(CardName)17001,
				(CardName)17001,
				(CardName)17001,
				(CardName)17002,
				(CardName)17003,
			}.ToILCPP();

		public override Il2CppCollections.List<ArtifactName> StartingArtifacts => 
			new List<ArtifactName>()
			{
				(ArtifactName)17000
			}.ToILCPP();
	}
}
