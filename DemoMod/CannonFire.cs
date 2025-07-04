﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Il2CppStarVaders;
using SVModHelper;
using SVModHelper.ModContent;

namespace DemoMod
{
    public class CannonFire : AModCard
    {
        public override string DisplayName => "Cannon Fire!";

        public override string Description => "Fire 1 <nobr><sprite=\"TextIcons\" name=\"Bullet\"> <b><color=#FFBF00>Bullet</color></b></nobr> upwards that pushes the struck entity.";

        public override Il2CppCollections.HashSet<CardTrait> Traits => new HashSet<CardTrait>() { CardTrait.Attack }.ToILCPP();
        public override Il2CppCollections.HashSet<CardTrait> HiddenTraits => new HashSet<CardTrait>() { CardTrait.Fire, CardTrait.Push }.ToILCPP();

        public override Rarity Rarity => Rarity.Common;
        public override ClassName Class => ClassName.Gunner;
        public override int ClassBaseCost => 1;
        public override bool RequiresPlayerEntity => true;

        public override Il2CppCollections.HashSet<MoreInfoWordName> MoreInfoWords => new HashSet<MoreInfoWordName>() { MoreInfoWordName.Bullet, MoreInfoWordName.Push }.ToILCPP();

        public override Il2CppCollections.HashSet<ComponentTrait> AllowedComponentTraits => new HashSet<ComponentTrait>() { ComponentTrait.Basic, ComponentTrait.SelectionLess, ComponentTrait.Attack, ComponentTrait.Bullet, ComponentTrait.Push }.ToILCPP();
        public override Il2CppCollections.HashSet<ComponentTrait> BlockedComponentTraits => new();
        public override Il2CppCollections.HashSet<ComponentName> AllowedComponentNames => new HashSet<ComponentName>() { ModContentManager.GetModComponentName<HeavyCannonFire>() }.ToILCPP();
        public override Il2CppCollections.HashSet<ComponentName> BlockedComponentNames => new HashSet<ComponentName>() { ComponentName.Breezy, ComponentName.Fiery }.ToILCPP();

        public override Il2CppCollections.List<ATask> GetPreSelectionTaskList(OnCreateIDValue cardID)
        {
            return new List<ATask>()
            {
                new FireBulletTask(tileEffect: new StrikeTileEffectTask(Coord.zero.BoxIl2CppObject(), Distance: 1, skipTaskQueue: true), animationSourceCoord: new())
            }.ToILCPP();
        }
    }
}
