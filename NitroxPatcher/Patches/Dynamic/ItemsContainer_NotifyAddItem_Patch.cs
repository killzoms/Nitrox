using System.Reflection;
using NitroxClient.GameLogic;
using NitroxModel.Helper;

namespace NitroxPatcher.Patches.Dynamic;

public sealed partial class ItemsContainer_NotifyAddItem_Patch : NitroxPatch, IDynamicPatch
{
    public override MethodInfo targetMethod { get; } = Reflect.Method((ItemsContainer t) => t.NotifyAddItem(default));

    public static void Postfix(ItemsContainer __instance, InventoryItem item)
    {
        if (item != null)
        {
            Resolve<ItemContainers>().BroadcastItemAdd(item.item, __instance.tr);
        }
    }
}
