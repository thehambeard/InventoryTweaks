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
    }
}