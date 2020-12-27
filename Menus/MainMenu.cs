using InventoryTweaks.Tweaks;
using InventoryTweaks.Utilities;
using ModMaker;
using UnityModManagerNet;
using GL = UnityEngine.GUILayout;


namespace InventoryTweaks.Menus
{
    class MainMenu : IMenuTopPage
    {
        public string Name => "Main Menu";

        public int Priority => 100;

        public void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GL.Label("This will toggle items to stack in your inventory, such as multiple suits of armor will take up one slot. Note: Toggling off will remove the number count for items stacked, split the items before turning stacking off.  Turning stack back on will restore the count.  Items not already stacked in your inventory will not auto-stack, they can be manually stacked and stacking will be automatic from the point you turn stacking on.");
            if (GL.Button($"Toggled: {SettingsWrapper.ForceStacking}", GL.ExpandWidth(false)))
            {
                ItemStacking.ToggleForceStacking();
            }
        }
    }
}
