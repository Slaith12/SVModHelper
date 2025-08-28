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
    public class HeavyCannonFire : AModComponent
    {
        public override string DisplayName => "Heavy";

        public override string Description => "Increases heat cost by 1. Bullet pushes struck entity and any colliding entities until they hit a wall.";

        public override ClassName Class => ClassName.Gunner;

        //No sprite included for this component
        //Overriding sprite property to suppress warning
        public override Sprite Sprite => null;

        public override int BaseCostModifier => 1;
        public override void ModifyCardModel(CardModel cardModel)
        {
            cardModel.SelectionTaskGroups = new CloneableList<SelectionTaskGroup>(new List<SelectionTaskGroup>()
            {
                new SelectionTaskGroup(new List<ATask>()
                {
                    new FireBulletTask(tileEffect: new HeavyCannonFireTask().Convert(), animationSourceCoord: new())
                }.ToILCPP(),
                selections: new List<Selection>() { new Selection(new DefaultSelectionCondition()) }.ToILCPP())
            }.ToILCPPEnumerable());
        }
    }
}
