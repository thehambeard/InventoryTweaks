using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModMaker;
using ModMaker.Utility;
using UnityModManagerNet;
using UnityEngine;
using GL = UnityEngine.GUILayout;
using Kingmaker;
using Kingmaker.Blueprints.Items.Equipment;

namespace InventoryTweaks.Menus
{
    class MainMenu : IMenuTopPage
    {
        public string Name => "Main Menu";

        public int Priority => 100;

        string path = "ServiceWindow/Encyclopedia/HierarchyView/TweakContainer";

        public void OnGUI(UnityModManager.ModEntry modEntry)
        {
            path = GL.TextField(path);
            if (GL.Button("Clear", GL.ExpandWidth(false)))
            {
                Transform tweakContainer;
                while (tweakContainer = Game.Instance.UI.Common.transform.Find(path))
                {
                    tweakContainer.SafeDestroy();
                }
            }
            if (GL.Button("Items", GL.ExpandWidth(false)))
            {
                foreach (var item in Game.Instance.Player.Inventory.Where(c => (c.Blueprint as BlueprintItemEquipmentUsable != null) && (c.Blueprint as BlueprintItemEquipmentUsable).Type == UsableItemType.Scroll))
                {
                    if (item != null)
                        Main.Mod.Debug(item);
                }
            }
                    
        }
    }
}
