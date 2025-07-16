using System;
using System.Collections.Generic;
using SVModHelper;
using SVModHelper.ModContent;

namespace DemoMod
{
    public class RapidFire : AModCard
    {
        //Required properties
        public override string DisplayName => "Rapid Fire!";

        public override string Description => "Fire a <nobr><sprite=\"TextIcons\" name=\"Bullet\"> <b><color=#FFBF00>Bullet</color></b></nobr> upwards.\n" +
                                            "This card has +5 <b><color=#FFBF00>Repeat</color></b>.";

        public override Il2CppCollections.HashSet<CardTrait> Traits => new HashSet<CardTrait>() { CardTrait.Attack }.ToILCPP();

        public override ClassName Class => ClassName.Gunner;

        public override Rarity Rarity => Rarity.Legendary;

        //Component properties
        public override Il2CppCollections.HashSet<ComponentTrait> AllowedComponentTraits => new HashSet<ComponentTrait>()
        {
            ComponentTrait.Basic,
            ComponentTrait.Bullet,
            ComponentTrait.SelectionLess,
            ComponentTrait.Attack
        }.ToILCPP();

        public override Il2CppCollections.HashSet<ComponentTrait> BlockedComponentTraits => new HashSet<ComponentTrait>()
        {
            ComponentTrait.Repeated
        }.ToILCPP();

        public override Il2CppCollections.HashSet<ComponentName> BlockedComponentNames => new HashSet<ComponentName>()
        {
            ComponentName.Tactical
        }.ToILCPP();

        //Other properties (still important!)
        public override Il2CppCollections.HashSet<CardTrait> HiddenTraits => new HashSet<CardTrait>() { CardTrait.Fire }.ToILCPP();
        public override Il2CppCollections.HashSet<MoreInfoWordName> MoreInfoWords => new HashSet<MoreInfoWordName>() { MoreInfoWordName.Bullet, MoreInfoWordName.Repeat }.ToILCPP();
        public override int ClassBaseCost => 3;
        public override bool RequiresPlayerEntity => true;

        //Main functionality properties
        public override int RepeatAmount => 5;

        public override Il2CppCollections.List<ATask> GetPreSelectionTaskList(OnCreateIDValue cardID)
        {
            Il2CppCollections.List<ATask> taskList = new();

            taskList.Add(new FireBulletTask(animationSourceCoord: new()));

            return taskList;
        }
    }
}
