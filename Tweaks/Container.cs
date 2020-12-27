using Kingmaker.Items;

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
