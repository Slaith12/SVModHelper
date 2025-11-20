using UnityEngine;

namespace SVModHelper.ModContent
{
    public class ComponentModification
    {
        internal SVMod m_Source;
        //public SVMod sourceMod => m_Source;
        public ComponentName targetComponent;
        public int priority;

        //I'm not adding all of the component properties to this class because I think if you start
        //changing some of the functionality of the component you might as well just make a new component instead.
        //That said, if you think there's anything that should be here, feel free to message me or post about it in #modding

        public string displayName;
        public string description;
        public Dictionary<string, string> localizedNames = new();
        public Dictionary<string, string> localizedDescriptions = new();
        public Sprite sprite;

        public ClassName? newClass;
        public Rarity? newRarity;

        public HashSet<ComponentTrait> extraComponentTraits = new();
        public HashSet<CardTrait> extraAddedTraits = new();
        public HashSet<CardTrait> extraAddedHiddenTraits = new();

        public ComponentModification(ComponentName target, int priority = 0)
        {
            this.targetComponent = target;
            this.priority = priority;
        }

        public void CopyTo(ComponentModification other)
        {
            if (displayName != null)
                other.displayName = displayName;
            if (description != null)
                other.description = description;
            foreach (var locName in localizedNames)
            {
                other.localizedNames[locName.Key] = locName.Value;
            }
            foreach (var locDesc in localizedDescriptions)
            {
                other.localizedDescriptions[locDesc.Key] = locDesc.Value;
            }
            if (sprite != null)
                other.sprite = sprite;
            if (newClass != null)
                other.newClass = newClass;
            if (newRarity != null)
                other.newRarity = newRarity;
            if (extraComponentTraits != null)
                other.extraComponentTraits.UnionWith(extraComponentTraits);
            if (extraAddedTraits != null)
                other.extraAddedTraits.UnionWith(extraAddedTraits);
            if (extraAddedHiddenTraits != null)
                other.extraAddedHiddenTraits.UnionWith(extraAddedHiddenTraits);
        }

        internal void ApplyTo(AComponent component)
        {
            if (newClass != null)
                component.Class = newClass.Value;
            if (newRarity != null)
                component.Rarity = newRarity.Value;

            component.ComponentTraits.UnionWith(extraComponentTraits.ToILCPPEnumerable());
            component.AddedTraits.UnionWith(extraAddedTraits.ToILCPPEnumerable());
            component.AddedHiddenTraits.UnionWith(extraAddedHiddenTraits.ToILCPPEnumerable());
        }
    }
}
