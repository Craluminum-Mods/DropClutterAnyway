using System.Reflection;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

[assembly: ModInfo(name: "Drop Clutter Anyway", modID: "dropclutteranyway", Side = "Server")]

namespace DropClutterAnyway;

public class Core : ModSystem
{
    private Harmony harmony;

    public override void StartServerSide(ICoreServerAPI api)
    {
        harmony = new Harmony(Mod.Info.ModID);

        harmony.Patch(original: typeof(BlockClutter).GetMethod("GetDrops"), prefix: typeof(BlockClutterDropPatch).GetMethod("Prefix"));
    }

    public override void Dispose()
    {
        harmony?.UnpatchAll(Mod.Info.ModID);
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
