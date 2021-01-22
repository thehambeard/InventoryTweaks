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
        public int Priority => 200;
        public ContainersUIController UI { get; internal set; }

        private Version resetAfter = new Version(2, 0, 0);

        public void ResetSettings()
        {
            Mod.Debug(MethodBase.GetCurrentMethod());
            Mod.Settings.lastModVersion = Mod.Version.ToString();
            LocalizationFileName = Local.FileName;
            ForceStacking = true;
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
            if (!Version.TryParse(Mod.Settings.lastModVersion, out Version version) || version > resetAfter)
                ResetSettings();
            else
            {
                Mod.Settings.lastModVersion = Mod.Version.ToString();
            }



            EventBus.Subscribe(this);
        }

        public void HandleModDisable()
        {
            Mod.Debug(MethodBase.GetCurrentMethod());
            EventBus.Unsubscribe(this);
        }
    }
}