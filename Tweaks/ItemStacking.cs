using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker;

using UnityEngine;

namespace InventoryTweaks.Tweaks
{
    class ItemStacking
    {
        public void ToggleForceStacking()
        {
            Game.Instance.Player.Inventory.ForceStackable = !Game.Instance.Player.Inventory.ForceStackable;
        }
    }
}
