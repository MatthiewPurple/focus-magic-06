using MelonLoader;
using HarmonyLib;
using Il2Cpp;
using focus_magic_06;

[assembly: MelonInfo(typeof(FocusMagic06), "Focus Magic (ver. 0.6)", "1.0.0", "Matthiew Purple")]
[assembly: MelonGame("アトラス", "smt3hd")]

namespace focus_magic_06;
public class FocusMagic06 : MelonMod
{
    static public bool isHealing = false;

    // After checking if a certain buff/debuff is applied
    [HarmonyPatch(typeof(nbCalc), nameof(nbCalc.nbGetMagicAttack))]
    private class Patch
    {
        public static void Postfix(ref int nskill)
        {
            isHealing = datSkill.tbl[nskill].skillattr == 13;
        }
    }

    // After checking if a certain buff/debuff is applied
    [HarmonyPatch(typeof(nbCalc), nameof(nbCalc.nbGetHojoRitu))]
    private class Patch2
    {
        public static void Postfix(ref int formindex, ref int type, ref float __result)
        {
            // If the game is checking for the presence of magic buff (5) and the demon has focused (15)
            if (type == 5 && nbMainProcess.nbGetPartyFromFormindex(formindex).count[15] == 1 && !isHealing)
            {
                nbMainProcess.nbGetPartyFromFormindex(formindex).count[15] = 0; // Remove Focus
                __result *= 2.5f; // Multiply damage by 2.5
            }
        }
    }

    // After getting the description of a skill
    [HarmonyPatch(typeof(datSkillHelp_msg), nameof(datSkillHelp_msg.Get))]
    private class Patch3
    {
        public static void Postfix(ref int id, ref string __result)
        {
            if (id == 224) __result = "More than doubles \nattack next turn."; // Changes Focus' description
        }
    }
}
