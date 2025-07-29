using MelonLoader;

namespace SVModHelper
{
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
                Melon<Core>.Logger.Error("No run in progress.");
                return;
            }
            DataManager.PlayerData.AddArtifact(artifact);
        }

        public static void AddCardToDeck(int card, int component = 0, int affinity = 0)
        {
            AddCardToDeck((CardName)card, (ComponentName)component, (AffinityName)affinity);
        }

        public static void AddCardToDeck(CardName card, ComponentName component = ComponentName.None, AffinityName affinity = AffinityName.None)
        {
            if (!DataManager.PlayerHasRunInProgress())
            {
                Melon<Core>.Logger.Error("No run in progress.");
                return;
            }
            if (!DataManager.PlayerHasRunInProgress())
            {
                Melon<Core>.Logger.Error("No run in progress.");
                return;
            }
            DataManager.PlayerData.AddCardToDeck(new PlayerCardData(card, component, affinity));
        }
    }
}
