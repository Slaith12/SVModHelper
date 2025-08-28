using SVModHelper;
using SVModHelper.ModContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DemoMod
{
    public class MedusaSpell : AModSpell
    {
        public override string DisplayName => "Medusa";

        public override string Description => "Transform a non-Boss entity into a Boulder.";

        public override int Cooldown => 3;

        //No sprite included for this spell
        //Overriding sprite property to suppress warning
        public override Sprite Sprite => null;

        public override Il2CppCollections.HashSet<ItemName> MoreInfoItems => new HashSet<ItemName> { ItemName.Boulder }.ToILCPP();

        public override Il2CppCollections.List<Selection> GetSpellSelections(OnCreateIDValue artifactID)
        {
            return new List<Selection>()
            {
                new Selection(
                    new AndCondition(new IsTypeCondition<Coord>(new TargetValue()),
                    new NotCondition(new IsCoordEmptyCondition(new TargetValue())),
                    new NotCondition(new IsEntityStaticCondition(new CoordEntityValue(new TargetValue()))),
                    new NotCondition(new IsBossCondition(new CoordEntityValue(new TargetValue()))),
                    new IsMoveableEntityCondition(new CoordEntityValue(new TargetValue()))
                    ))
            }.ToILCPP();
        }

        public override Il2CppCollections.List<ATask> GetTaskList(OnCreateIDValue artifactID)
        {
            return new List<ATask>()
            {
                new DissipateEntityTask(new CoordEntityValue(new TargetValue())),
                new CreateItemTask((int)ItemName.Boulder, new TargetValue(), new TargetValue(), true)
            }.ToILCPP();
        }
    }
}
