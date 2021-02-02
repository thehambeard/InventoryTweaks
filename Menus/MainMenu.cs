using InventoryTweaks.Tweaks;
using InventoryTweaks.Utilities;
using ModMaker;
using UnityEngine;
using UnityModManagerNet;
using GL = UnityEngine.GUILayout;


namespace InventoryTweaks.Menus
{
    class MainMenu : IMenuTopPage
    {
        public string Name => "Main Menu";

        public int Priority => 100;

        public static bool MoveToggle = false;

        GUIStyle toggleStyle = new GUIStyle(GUI.skin.toggle) { wordWrap = true };
        GUIStyle labelStyle = new GUIStyle(GUI.skin.label) { wordWrap = true };
        public void OnGUI(UnityModManager.ModEntry modEntry)
        {
            using (new GL.VerticalScope("box"))
            {
                GL.Label("This will toggle items to stack in your inventory, such as multiple suits of armor will take up one slot. Note: Toggling off will remove the number count for items stacked, split the items before turning stacking off.  Turning stack back on will restore the count. \n Items not already stacked in your inventory will not auto-stack, they can be manually stacked and stacking will be automatic from the point you turn stacking on.", labelStyle, GL.ExpandWidth(false));
                if (GL.Button($"Toggled: {SettingsWrapper.ForceStacking}", GL.ExpandWidth(false)))
                {
                    ItemStacking.ToggleForceStacking();
                }
            }
            using (new GL.VerticalScope("box"))
            { 
                GL.Label("Move mode:", GL.ExpandWidth(false));
                MoveToggle = GL.Toggle(MoveToggle, "Toggle move mode to move the location of the container window. Select the entire party so quick actions aren't triggered and use the 4, 8, 6, 2 keys to move the window left, up, right, down. Pressing 7 will up the amount the menu moves per direction key pressed, 1 will decrease it. Turn move mode off after finished moving the window where you want it.", toggleStyle, GL.ExpandWidth(false));
            }
        }
    }
}
