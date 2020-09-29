using Kingmaker.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryTweaks.Tweaks
{
    class ScrollCase : Container
    {
        public void AddScroll(ItemEntity item)
        {
            base.contents.Add(item);
        }

        public void RemoveScroll(ItemEntity item)
        {
            base.contents.Remove(item);
        }
    }
}
