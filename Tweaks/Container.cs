using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker;
using Kingmaker.Items;
using UnityEngine.UI;

namespace InventoryTweaks.Tweaks
{
    class Container
    {
        protected ItemsCollection contents;

        public Container()
        {
            contents.ForceStackable = true;
        }
    }
}
