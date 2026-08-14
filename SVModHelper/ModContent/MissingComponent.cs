using UnityEngine;

namespace SVModHelper.ModContent
{
    public class MissingComponent : AModComponent
    {
        public override string ID => stringID;
        public override string DisplayName => $"{stringID} [Missing]";
        public override string Description => $"This component was added by a mod when this run was active.\n" +
                                              $"That mod has either since been disabled or no longer includes this component.";
        public override SpriteDescriptor Sprite => new(); //TODO: Replace this so there's a distinct sprite in the run history

        public override ClassName Class => ClassName.Neutral;

        public override Rarity Rarity => Rarity.TempRemoved;

        internal ComponentName intID;
        internal string stringID;

        public MissingComponent(ComponentName componentName, string id)
        {
            intID = componentName;
            stringID = id;
        }
    }
}
