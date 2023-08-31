using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

[assembly: ModInfo(name: "Drop Clutter Anyway", modID: "dropclutteranyway", Side = "Server")]

namespace DropClutterAnyway;

public class Core : ModSystem
{
    public const string HarmonyID = "craluminum2413.dropclutteranyway";

    public override void Start(ICoreAPI api)
    {
        base.Start(api);
        api.RegisterBlockBehaviorClass("DropClutterAnyway:GuaranteedDrop", typeof(BlockBehaviorGuaranteedDrop));
    }

    public override void StartServerSide(ICoreServerAPI api)
    {
        base.StartServerSide(api);
        new Harmony(HarmonyID).Patch(original: typeof(BlockClutter).GetMethod("GetDrops"), prefix: typeof(BlockClutterDropPatch).GetMethod("Prefix"));
    }

    public override void Dispose()
    {
        new Harmony(HarmonyID).Unpatch(original: typeof(BlockClutter).GetMethod("GetDrops"), HarmonyPatchType.All, HarmonyID);
        base.Dispose();
    }

    [HarmonyPatch(typeof(BlockClutter), nameof(BlockClutter.GetDrops))]
    public static class BlockClutterDropPatch
    {
        public static bool Prefix(BlockClutter __instance, BlockPos pos)
        {
            BEBehaviorShapeFromAttributes bec = __instance.GetBEBehavior<BEBehaviorShapeFromAttributes>(pos);
            bec.Collected = true;
            return true;
        }
    }
}