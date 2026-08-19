using Il2CppStarVaders;
using MelonLoader;
using SVModHelper.ModContent;

namespace SVModHelper
{
    /// <summary>
    /// <para>This class is intended to be used with the UnityExplorer mod (https://github.com/yukieiji/UnityExplorer)
    /// to get various debug options while playing.</para>
    /// </summary>
    public static class GameDebug
    {
        public static void AddArtifact(int artifact)
        {
            AddArtifact((ArtifactName)artifact);
        }

        public static void AddArtifact(ArtifactName artifact)
        {
            if(!DataManager.PlayerHasRunInProgress())
            {
                Melon<Core>.Logger.Error("Cannot use AddArtifact() when there's no run in progress.");
                return;
            }
            Melon<Core>.Logger.Msg($"Adding artifact {artifact} to campaign deck (does not affect current encounter if active)");
            DataManager.PlayerData.AddArtifact(artifact);
        }

        public static void PrintAllModArtifactNames()
        {
            Melon<Core>.Logger.Msg("Calling PrintAllModArtifactNames:");
            foreach(ArtifactName name in ModContentManager.GetAllModArtifactNames())
            {
                IHasArtifactID artifact = ModContentManager.GetModArtifactInstance(name);
                Melon<Core>.Logger.Msg($"{name}: {artifact.DisplayName} ({artifact.ID})");
            }
        }

        public static void AddCardToDeck(int card, int component = 0, int affinity = 0)
        {
            AddCardToDeck((CardName)card, (ComponentName)component, (AffinityName)affinity);
        }

        public static void AddCardToDeck(CardName card, ComponentName component = ComponentName.None, AffinityName affinity = AffinityName.None)
        {
            if (!DataManager.PlayerHasRunInProgress())
            {
                Melon<Core>.Logger.Error("Cannot use AddCardToDeck() when there's no run in progress.");
                return;
            }
            Melon<Core>.Logger.Msg($"Adding card {card} to campaign deck (does not affect current encounter if active)");
            DataManager.PlayerData.AddCardToDeck(new PlayerCardData(card, component, affinity));
        }

        public static void PrintAllModCardNames()
        {
            Melon<Core>.Logger.Msg("Calling PrintAllModCardNames:");
            foreach (CardName name in ModContentManager.GetAllModCardNames())
            {
                AModCard card = ModContentManager.GetModCardInstance(name);
                Melon<Core>.Logger.Msg($"{name}: {card.DisplayName} ({card.ID})");
            }
        }
    }
}
