global using Il2CppCollections = Il2CppSystem.Collections.Generic;
global using Il2CppStarVaders;

using MelonLoader;
using SVModHelper;
using SVModHelper.ModContent;

[assembly: MelonInfo(typeof(DemoMod.Core), "Demo Mod", "1.0.0", "Slaith", null)]
[assembly: MelonGame("Pengonauts", "StarVaders")]

namespace DemoMod
{
    internal class Core : SVMod
    {
        protected override void RegisterMod()
        {
            base.RegisterMod();
            LoggerInstance.Msg("Registering modifications");

            RegisterContentMod(new CardModification(CardName.Pyroclasm)
            {
                displayName = "Pyroclasm",
                description =
                "Strike all <nobr>non-<sprite=\"TextIcons\" name=\"Shield\"> <b><color=#FFBF00>Shielded</color></b></nobr>, non-<b><color=#FFBF00>Immune</color></b> invaders, " +
                "then burn all cards in your draw pile.\n" +
                "Purge this card.",
                newRarity = Rarity.Legendary,
                isShowable = true,
                extraCompWL = [ComponentName.Replicating, ComponentName.SwiftPlus],
                extraCompBL = [ComponentName.Swift, ComponentName.Tactical]
            });

            RegisterContentMod(new ArtifactModification(ArtifactName.AntiMag)
            {
                displayName = "Weird Movement Device",
                sprite = GetStandardSprite("NewAntiMag.png"),
                newRarity = Rarity.Rare
            });

            RegisterContentMod(new ComponentModification(ComponentName.Echo)
            {
                description = "You know what this component does."
            });

            LoggerInstance.Msg("Done!");
        }
    }
}