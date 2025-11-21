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
        protected override void EarlyRegisterMod()
        {
            RegisterMoreInfoPanel("CascadingPush",
                "<b><color=#FFBF00>Cascading Push</color></b>: When a pushed entity collides with another entity while cascading, both entities continue " +
                "getting pushed until the original entity has finished moving or any of the pushed entities collide with a wall.\n\n" +
                "The extra pushed entities also cascade. Colliding entities are <b>struck</b> once in total, regardless of how many times they cascade.");
            base.EarlyRegisterMod();
        }

        protected override void LateRegisterMod()
        {
            LoggerInstance.Msg("Registering modifications");

            RegisterContentMod(new CardModification(CardName.Pyroclasm)
            {
                displayName = "Pyroclasm",
                description =
                "Strike all <nobr>non-<sprite=\"TextIcons\" name=\"Shield\"> <b><color=#FFBF00>Shielded</color></b></nobr>, non-<b><color=#FFBF00>Immune</color></b> invaders, " +
                "then burn all cards in your draw pile.\n" +
                "Purge this card.",
                newClass = ClassName.UniquePack,
                newRarity = Rarity.Legendary,
                isShowable = true,
                extraCompWL = [ComponentName.Replicating, ComponentName.SwiftPlus],
                extraCompBL = [ComponentName.Swift, ComponentName.Tactical]
            });

            RegisterContentMod(new ArtifactModification(ArtifactName.AntiMag)
            {
                displayName = "Weird Movement Device",
                sprite = GetStandardSprite("NewAntiMag.png"),
                newRarity = Rarity.Rare,
                isCurseModifier = true
            });

            RegisterContentMod(new ComponentModification(ComponentName.Echo)
            {
                description = "You know what this component does."
            });

            RegisterContentMod(new PackModification(ItemPackName.UpgradePack)
            {
                extraPackArtifacts = [ModContentManager.GetModArtifactName<BlitzDrive>()],
                excludedPackArtifacts = [ArtifactName.Blueprint]
            });

            RegisterContentMod(new PackModification(ItemPackName.BeachPack)
            {
                extraPackArtifacts = [ArtifactName.Blueprint]
            });

            RegisterContentMod(new PackModification(ItemPackName.GunnerPack)
            {
                displayName = "Scarlet",
                description = "Offerings from the Scarlet Mask.",
                sprite = GetStandardSprite("ScarletPack.png"),
                isHidden = false
            });

            RegisterContentMod(new CardModification(CardName.Avarice)
            {
                displayName = "Avarice",
                description = "Select a card in your hand.",
                newClass = ClassName.UniquePack,
                newPilot = PilotName.None,
                newRarity = Rarity.Common,
                isShowable = true
            });

            RegisterContentMod(new CardModification(CardName.Discipline)
            {
                displayName = "Discipline",
                description = "The next <b><color=#FF0000>Attack</color></b> you play this turn gets +1 <b><color=#FFBF00>Repeat</color></b> and goes to the top of the draw pile.",
                newClass = ClassName.UniquePack,
                newRarity = Rarity.Rare,
                isShowable = true
            });

            RegisterContentMod(new CardModification(CardName.Expose)
            {
                displayName = "Expose",
                description = "Move 1 to 3 tiles. Strike all <nobr><sprite=\"TextIcons\" name=\"Shield\"> <b><color=#FFBF00>Shielded</color></b></nobr> entities in your row.",
                newClass = ClassName.UniquePack,
                newRarity = Rarity.Rare,
                isShowable = true
            });

            RegisterContentMod(new CardModification(CardName.Shine)
            {
                displayName = "Shine",
                description = "Give your mech a <nobr><sprite=\"TextIcons\" name=\"Shield\"> <b><color=#FFBF00>Shield</color></b></nobr>.\nPurge this card.",
                newClass = ClassName.UniquePack,
                newRarity = Rarity.Common,
                isShowable = true
            });

            RegisterContentMod(new ArtifactModification(ArtifactName.RisingStar)
            {
                displayName = "Rising Star",
                description = "Draw 2 fewer cards each turn.\n" +
                "Whenever you play the rightmost card in your hand, draw 1 card.",
                newClass = ClassName.UniquePack,
                newRarity = Rarity.Legendary,
                canBeDuplicated = true
            });

            RegisterContentMod(new ArtifactModification(ArtifactName.FermiReaction)
            {
                displayName = "Fermi Reaction",
                description = "When an entity is pushed, push any entity it collides with.",
                newClass = ClassName.UniquePack,
                newRarity = Rarity.Common
            });

            RegisterContentMod(new ArtifactModification(ArtifactName.AegisInvaders)
            {
                isEncounterModifier = false,
                isCurseModifier = false
            });

            RegisterContentMod(new ArtifactModification(ArtifactName.Doomed)
            {
                isEncounterModifier = true,
                isCurseModifier = true
            });

            RegisterContentMod(new ItemModification(ItemName.AegisBomb)
            {
                displayName = "BIG Bomb!",
                description = "Due to technical limitations, this is currently only a normal sized bomb. :(",
                newViewData = new ItemViewDataSO()
                {
                    Sprite = GetStandardSprite("AegisBomb.png", 40),
                    Shadow = GetDefaultShadowSprite(),
                    SpawnType = SpawnType.Heavy,
                    Floatiness = 0
                },
                hasRegeneratingShield = true
            });

            CardName followThroughName = ModContentManager.GetModCardName<FollowThrough>();

            RegisterContentMod(new PilotModification(PilotName.Shun)
            {
                startingCards = new List<PlayerCardData>()
                {
                    new PlayerCardData(CardName.Dash),
                    new PlayerCardData(CardName.Dash),
                    new PlayerCardData(CardName.Dash),
                    new PlayerCardData(followThroughName),
                    new PlayerCardData(followThroughName),
                    new PlayerCardData(CardName.Jab),
                    new PlayerCardData(CardName.Jab),
                    new PlayerCardData(CardName.Jab),
                    new PlayerCardData(CardName.Jab),
                    new PlayerCardData(CardName.HeaveHo), //Zap
                }.ToILCPP()
            });

            RegisterContentMod(new CardModification(ModContentManager.GetModCardName<CannonFire>())
            {
                displayName = "Cannon Fire! (Global Default)",
                localizedNames = new Dictionary<string, string>()
                {
                    ["en"] = "Cannon Fire! (en)",
                    ["es"] = "Cannon Fire! (es)",
                    ["fr"] = "Cannon Fire! (fr)"
                }
            });

            LoggerInstance.Msg("Done!");

            base.LateRegisterMod();
        }
    }
}