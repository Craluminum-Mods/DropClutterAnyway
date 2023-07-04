using System.Reflection;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

[assembly: ModInfo("Drop Clutter Anyway")]

namespace DropClutterAnyway;

public class Core : ModSystem
{
    public const string HarmonyID = "craluminum2413.dropclutteranyway";

    public override void StartServerSide(ICoreServerAPI api)
    {
        base.StartServerSide(api);
        new Harmony(HarmonyID).PatchAll(Assembly.GetExecutingAssembly());
        api.World.Logger.Event("started 'Drop Clutter Anyway' mod");
    }

    public override void Dispose()
    {
        new Harmony(HarmonyID).UnpatchAll();
        base.Dispose();
    }

    [HarmonyPatch(typeof(BlockClutter), nameof(BlockClutter.GetDrops))]
    public static class BlockClutterDropPatch
    {
        public static bool Prefix(BlockClutter __instance, BlockPos pos)
        {
            var bec = __instance.GetBEBehavior<BEBehaviorShapeFromAttributes>(pos);
            bec.Collected = true;
            return true;
        }
    }
}