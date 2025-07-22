using SVModHelper;
using SVModHelper.ModContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMod
{
    public class Landmine : AModCard
    {
        public override string DisplayName => "Landmine";

        public override string Description => "Spawn a <b><color=#FFBF00>Landmine</color></b> up to 4 tiles away.";

        public override Il2CppCollections.HashSet<CardTrait> Traits => new HashSet<CardTrait>() { CardTrait.Tactic }.ToILCPP();
        public override Il2CppCollections.HashSet<CardTrait> HiddenTraits => new HashSet<CardTrait>() { CardTrait.Summon }.ToILCPP();

        public override ClassName Class => ClassName.Gunner;

        public override Rarity Rarity => Rarity.Common;

        public override int ClassBaseCost => 1;

        public override Il2CppCollections.HashSet<ComponentTrait> AllowedComponentTraits => new HashSet<ComponentTrait>()
        {
            ComponentTrait.Basic,
            ComponentTrait.Selection
        }.ToILCPP();

        public override Il2CppCollections.HashSet<MoreInfoWordName> MoreInfoWords => new HashSet<MoreInfoWordName>() { MoreInfoWordName.Bomb }.ToILCPP();
        public override Il2CppCollections.HashSet<ItemName> MoreInfoItems => new HashSet<ItemName>() { ModContentManager.GetModItemName<LandmineItem>() }.ToILCPP();

        public override bool RequiresPlayerEntity => true;

        public override Il2CppCollections.List<Selection> GetSelections(OnCreateIDValue cardID)
        {
            Il2CppCollections.List<Selection> selections = new();

            selections.Add(new Selection(new AndCondition(
                new IsTypeCondition<Coord>(new TargetValue()),
                new IsMoveableCoordCondition(new TargetValue()),
                new DistanceCondition(new PlayerCoordValue(), new TargetValue(), 1, 4)),
                selectionDescriptor: SelectionDescriptor.Tile));

            return selections;
        }

        public override Il2CppCollections.List<ATask> GetPostSelectionTaskList(OnCreateIDValue cardID)
        {
            Il2CppCollections.List<ATask> taskList = new();

            taskList.Add(new CreateItemTask((int)ModContentManager.GetModItemName<LandmineItem>(), new TargetValue(), new PlayerCoordValue()));

            return taskList;
        }
    }
}
