namespace SVModHelper.ModContent
{
    public class MissingItem : AModItem
    {
        public override string ID => stringID;
        public override string DisplayName => $"{stringID} [Missing]";
        public override string Description => $"This should not be visible. If it is, something is wrong with one of the currently enabled mods.";
        public override ItemViewDescriptor ItemViewData => null;

        internal ItemName intID;
        internal string stringID;

        public MissingItem(ItemName itemName, string id)
        {
            intID = itemName;
            stringID = id;
        }
    }
}
