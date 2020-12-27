using Kingmaker;
using Kingmaker.PubSubSystem;
using ModMaker;
using static InventoryTweaks.Utilities.SettingsWrapper;

namespace InventoryTweaks.Tweaks
{
    public class ItemStacking : IModEventHandler, IAreaLoadingStagesHandler
    {
        public int Priority => 100;

        public void OnAreaLoadingComplete()
        {
            Game.Instance.Player.Inventory.ForceStackable = ForceStacking;
        }

        public void OnAreaScenesLoaded()
        {

        }

        public static void ToggleForceStacking()
        {
            ForceStacking = !ForceStacking;

            Game.Instance.Player.Inventory.ForceStackable = ForceStacking;
        }

        public void HandleModEnable()
        {
            EventBus.Subscribe(this);
        }

        public void HandleModDisable()
        {
            EventBus.Unsubscribe(this);
        }
    }
}
