using static InventoryTweaks.Main;

namespace InventoryTweaks.Utilities
{
    public static class SettingsWrapper
    {
        public static string LocalizationFileName
        {
            get => Mod.Settings.localizationFileName;
            set => Mod.Settings.localizationFileName = value;
        }

        public static bool ForceStacking
        {
            get => Mod.Settings.forceStatcking;
            set => Mod.Settings.forceStatcking = value;
        }

        public static float Menu_X
        {
            get => Mod.Settings.menu_x;
            set => Mod.Settings.menu_x = value;
        }

        public static float Menu_Y
        {
            get => Mod.Settings.menu_y;
            set => Mod.Settings.menu_y = value;
        }
    }
}