using InventoryTweaks.UI;
using InventoryTweaks.Utilities;
using Kingmaker.PubSubSystem;
using ModMaker;
using System;
using System.Reflection;
using static InventoryTweaks.Common;
using static InventoryTweaks.Main;
using static InventoryTweaks.Utilities.SettingsWrapper;

namespace InventoryTweaks
{
    class Core :
        IModEventHandler
    {
        public int Priority => 100;
        public ContainersUIController UI { get; internal set; }

        private Version NeedsReset = new Version(0, 1, 5);
        public void ResetSettings()
        {
            Mod.Debug(MethodBase.GetCurrentMethod());
            Mod.Settings.lastReset = Mod.Version.ToString();
            LocalizationFileName = Local.FileName;
            ForceStacking = false;
            Menu_X = 0.27f;
            Menu_Y = 0.61f;
        }

        public void HandleModEnable()
        {
            Mod.Debug(MethodBase.GetCurrentMethod());
            ModCheck bvcheck = new ModCheck("BetterVendors");
            BVModEnabled = bvcheck.IsInstalled() && bvcheck.IsActive() && ((bvcheck.Version() >= BVMinVersion) || bvcheck.Version() <= BVMaxVersion);
            if (!string.IsNullOrEmpty(LocalizationFileName))
            {
                Local.Import(LocalizationFileName, e => Mod.Error(e));
                LocalizationFileName = Local.FileName;
            }
            if (!Version.TryParse(Mod.Settings.lastReset, out Version version) || (version < NeedsReset))
                ResetSettings();
            EventBus.Subscribe(this);
        }

        public void HandleModDisable()
        {
            Mod.Debug(MethodBase.GetCurrentMethod());
            EventBus.Unsubscribe(this);
        }
    }
}