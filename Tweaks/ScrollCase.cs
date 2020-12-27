using Kingmaker.Items;

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
