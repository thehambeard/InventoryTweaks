using System;
using UnityModManagerNet;

namespace InventoryTweaks.Utilities
{
    public class ModCheck
    {
        private UnityModManager.ModEntry modEntry;
        public ModCheck(string id)
        {
            modEntry = UnityModManager.FindMod(id);
        }
        public bool IsActive()
        {
            return modEntry != null && modEntry.Assembly != null && modEntry.Active;
        }

        public bool IsInstalled()
        {
            return modEntry != null;
        }

        public Version Version()
        {
            return new Version((modEntry != null) ? modEntry.Info.Version : "0");
        }
    }
}
