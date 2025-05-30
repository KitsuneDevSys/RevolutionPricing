using BepInEx;
using HarmonyLib;
using HutongGames.PlayMaker.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace RevolutionPricing
{
    [BepInDependency(ETGModMainBehaviour.GUID)]
    [BepInPlugin(GUID, NAME, VERSION)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string GUID = "kitsune.etg.revolutionpricing";
        public const string NAME = "Free Gunslinger And Paradox";
        public const string VERSION = "1.0.0";
        public const string TEXT_COLOR = "#00FFFF";

        public void Start()
        {
            var harmony = new Harmony(GUID);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            ETGModMainBehaviour.WaitForGameManagerStart(GMStart);
        }

        public void GMStart(GameManager g)
        {
            Log($"{NAME} v{VERSION} started successfully.", TEXT_COLOR);
        }

        #region Foyer Price Change

        [HarmonyPatch(typeof(FoyerCharacterSelectFlag), nameof(FoyerCharacterSelectFlag.Start))]
        [HarmonyPostfix]
        public static void RemoveGunslingerPriceFromFoyer(FoyerCharacterSelectFlag __instance)
        {
            //If the player picked the Gunslinger
            bool isInstanceValid = __instance.OverheadElement != null && __instance.OverheadElement.GetComponent<FoyerInfoPanelController>() != null;
            bool isGunslinger = isInstanceValid && __instance.OverheadElement.GetComponent<FoyerInfoPanelController>().characterIdentity == PlayableCharacters.Gunslinger;

            if (isGunslinger)
            {
                __instance.IsGunslinger = false;
                __instance.OverheadElement.GetComponent<FoyerInfoPanelController>().characterIdentity = PlayableCharacters.Pilot;
            }

        }

        [HarmonyPatch(typeof(FoyerCharacterSelectFlag), nameof(FoyerCharacterSelectFlag.Start))]
        [HarmonyPostfix]
        public static void RemoveParadoxPriceFromFoyer(FoyerCharacterSelectFlag __instance)
        {
            //If the player picked the Paradox
            bool isInstanceValid = __instance.OverheadElement != null && __instance.OverheadElement.GetComponent<FoyerInfoPanelController>() != null;
            bool isParadox = isInstanceValid && __instance.OverheadElement.GetComponent<FoyerInfoPanelController>().characterIdentity == PlayableCharacters.Eevee;

            if (isParadox)
            {
                __instance.IsEevee = false;
                __instance.OverheadElement.GetComponent<FoyerInfoPanelController>().characterIdentity = PlayableCharacters.Pilot;
            }

        }

        #endregion

        #region QuickRestart Price Change

        [HarmonyPatch(typeof(AmmonomiconDeathPageController), nameof(AmmonomiconDeathPageController.GetNumMetasToQuickRestart))]
        [HarmonyPostfix]
        public static void FreeQuickRestart(AmmonomiconDeathPageController __instance, ref QuickRestartOptions __result)
        {
            __result.NumMetas = 0;
        }

        #endregion

        public static void Log(string text, string color = "FFFFFF")
        {
            ETGModConsole.Log($"<color={color}>{text}</color>");
        }
    }
}
