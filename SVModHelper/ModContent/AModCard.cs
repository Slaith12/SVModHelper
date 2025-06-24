using Il2CppInterop.Runtime.Injection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SVModHelper.ModContent
{
    public abstract class AModCard : AModContent
    {
        protected CardViewData GetStandardCardViewData(string imageName, float pixelsPerUnit = 100, FilterMode filter = FilterMode.Bilinear)
        {
            Sprite sprite = GetStandardSprite(imageName, pixelsPerUnit, filter);
            if (sprite == null)
                return null;
            return new CardViewData(CardName, sprite, null);
        }

        public CardName CardName => ModContentManager.GetModCardName(GetType());

        public abstract string DisplayName { get; }
        public abstract string Description { get; }
        public virtual CardViewData CardViewData => GetStandardCardViewData(GetType().Name + ".png");

        /// <summary>
        /// The main traits that the card has.
        /// <para>Examples: Move/Attack/Tactic/Junk</para>
        /// </summary>
        public abstract Il2CppCollections.HashSet<CardTrait> Traits { get; }
        /// <summary>
        /// Any extra traits that don't show up on the card trait display, but are still used by other card/artifact effects. 
        /// <para>Examples: Fire!/Push/Flow/Sacrifice</para>
        /// </summary>
        public virtual Il2CppCollections.HashSet<CardTrait> HiddenTraits => new Il2CppCollections.HashSet<CardTrait>();

        /// <summary>
        /// The mech class that's able to get this card. Set to Neutral to allow any class to get it. Set to UniquePack to restrict it to a pack.
        /// </summary>
        public abstract ClassName Class { get; }
        /// <summary>
        /// The pilot this card belongs to. Set to None to allow any pilot to get it.
        /// </summary>
        public virtual PilotName PilotUnique => PilotName.None;

        public abstract Rarity Rarity { get; }
        public virtual int ClassBaseCost => 0;

        /// <summary>
        /// The type of preview shown when the card is highlighted in the shop/reward screens.
        /// </summary>
        public virtual ContextPreviewType ContextPreviewType => ContextPreviewType.None;

        /// <summary>
        /// The definition panels that show up in the more info screen of this card.
        /// </summary>
        public virtual Il2CppCollections.HashSet<MoreInfoWordName> MoreInfoWords => new Il2CppCollections.HashSet<MoreInfoWordName>();
        /// <summary>
        /// The cards that show up in the more info screen of this card.
        /// </summary>
        public virtual Il2CppCollections.HashSet<CardName> MoreInfoCards => new Il2CppCollections.HashSet<CardName>();
        /// <summary>
        /// The items that show up in the more info screen of this card.
        /// </summary>
        public virtual Il2CppCollections.HashSet<ItemName> MoreInfoItems => new Il2CppCollections.HashSet<ItemName>();
        /// <summary>
        /// The enemies that show up in the more info screen of this card.
        /// </summary>
        public virtual Il2CppCollections.HashSet<EnemyName> MoreInfoEnemies => new Il2CppCollections.HashSet<EnemyName>();

        /// <summary>
        /// If true, this card is able to be created by any effect that creates a random card (including the reward screen)
        /// </summary>
        public virtual bool IsShowable => Rarity != Rarity.TempRemoved && Rarity != Rarity.Cosmic && Rarity != Rarity.Trader && Rarity != Rarity.Junk && Rarity != Rarity.Created && !IsToken;
        /// <summary>
        /// If true, this card is only able to created as part of another effect.
        /// </summary>
        public virtual bool IsToken => false;
        /// <summary>
        /// <para>If true, the card can only use energy from the battery for its cost.</para>
        /// <para>Has no effect on non-Stinger mechs.</para>
        /// </summary>
        public virtual bool IsPowerCellOnly => false;
        /// <summary>
        /// <para>If true, the card can only be played when the player has an active mech/summon on the field.</para>
        /// <para>Has no effect on non-Keeper mechs.</para>
        /// </summary>
        public virtual bool RequiresPlayerEntity => false;
        /// <summary>
        /// The base number of repeats the card has.
        /// </summary>
        public virtual int RepeatAmount => 0;
        /// <summary>
        /// If true, the card purges itself after being played.
        /// </summary>
        public virtual bool IsPurged => false;
        /// <summary>
        /// If true, the card is free by default.
        /// </summary>
        public virtual bool IsFree => false;
        /// <summary>
        /// Where the card goes after being played.
        /// </summary>
        public virtual Pile Destination => Pile.Discard;

        //don't use these for now
        public virtual bool HasSpecialCardModel => false;
        public virtual bool HasSpecialCardViewPrefab => false;

        /// <summary>
        /// <para>Only components with at least one of these traits can be applied to this card.</para>
        /// </summary>
        public virtual Il2CppCollections.HashSet<ComponentTrait> AllowedComponentTraits => new Il2CppCollections.HashSet<ComponentTrait>();
        /// <summary>
        /// <para>Any components with at least one of these traits can NOT be applied to this card.</para>
        /// </summary>
        public virtual Il2CppCollections.HashSet<ComponentTrait> BlockedComponentTraits => new Il2CppCollections.HashSet<ComponentTrait>();
        /// <summary>
        /// <para>These components can be applied to this card. This has higher priority than the ComponentTraits lists.</para>
        /// </summary>
        public virtual Il2CppCollections.HashSet<ComponentName> AllowedComponentNames => new Il2CppCollections.HashSet<ComponentName>();
        /// <summary>
        /// <para>These components can NOT be applied to this card. This has higher priority than the ComponentTraits lists.</para>
        /// </summary>
        public virtual Il2CppCollections.HashSet<ComponentName> BlockedComponentNames => new Il2CppCollections.HashSet<ComponentName>();

        /// <summary>
        /// Returns any effects that can trigger outside of when this card is played/created.
        /// </summary>
        public virtual Il2CppCollections.List<TriggerEffect> GetTriggerEffects(OnCreateIDValue cardID)
        {
            return new();
        }

        /// <summary>
        /// Returns any effects that trigger when the card is created during an encounter (including when it's added to the deck at the start of the encounter)
        /// </summary>
        public virtual Il2CppCollections.List<ATask> GetOnCreateTaskList(OnCreateIDValue cardID)
        {
            return new();
        }

        /// <summary>
        /// <para>Returns any effects that trigger when the card is played.</para>
        /// <para>These tasks will be previewed when the player is hovering over the card, even before selections are made.</para>
        /// </summary>
        public virtual Il2CppCollections.List<ATask> GetPreSelectionTaskList(OnCreateIDValue cardID)
        {
            return new();
        }

        /// <summary>
        /// <para>Returns any effects that trigger when the card is played.</para>
        /// <para>These tasks will only be previewed when the player is finishing the selections.</para>
        /// </summary>
        public virtual Il2CppCollections.List<ATask> GetPostSelectionTaskList(OnCreateIDValue cardID)
        {
            return new();
        }

        /// <summary>
        /// Returns the selections the player makes as part of playing this card.
        /// </summary>
        public virtual Il2CppCollections.List<Selection> GetSelections(OnCreateIDValue cardID)
        {
            return new List<Selection>()
            {
                new Selection(new DefaultSelectionCondition())
            }.ToILCPP();
        }

        /// <summary>
        /// <para>Returns the full on-play behavior of this card. If this is overwritten, GetPreSelectionTaskList(), GetPostSelectionTaskList(), and GetSelections() are unused.</para>
        /// <para>Override this if you want to have multiple selections with different effects occuring between each selection.</para>
        /// </summary>
        public virtual Il2CppCollections.List<SelectionTaskGroup> GetSelectionTaskGroups(OnCreateIDValue cardID)
        {
            return new List<SelectionTaskGroup>()
            {
                new SelectionTaskGroup(GetPreSelectionTaskList(cardID), GetPostSelectionTaskList(cardID), GetSelections(cardID))
            }.ToILCPP();
        }

        /// <summary>
        /// Returns the condition that must be satisfied for the card to be played.
        /// </summary>
        public virtual ACondition PlayCondition => new AndCondition(
            new NotCondition(new IsBurntCondition(new TargetValue())),
            new EnoughPowerToPlayCondition(new TargetValue()),
            new EnoughManaToPlayCondition(new TargetValue()),
            new OrCondition(
                new PlayerEntityExistsCondition(),
                new EqualsCondition(RequiresPlayerEntity, false)
            )
        );
    }
}
