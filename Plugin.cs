using BepInEx;
using UnityEngine;
using Utilla;
using Utilla.Attributes;

namespace The_Monkey_Mod_Menu
{
    [BepInPlugin("com.GT13.gtag.monkemodmenu", "The Monkey Mod Menu", "1.0.0")]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [ModdedGamemode]
    public class Plugin : BaseUnityPlugin
    {
        internal static bool InModdedRoom { get; private set; }

        private void Awake()
        {
            Logger.LogInfo("[The Monkey Mod Menu v1.0] Loaded");

            GameObject manager = new GameObject("ModdedOnlyMenu_Manager");
            DontDestroyOnLoad(manager);

            manager.AddComponent<Menu>();
            manager.AddComponent<Mods.Movement>();
            manager.AddComponent<Mods.Visual>();
            manager.AddComponent<ScreenGUI>();
        }

        [ModdedGamemodeJoin]
        private void OnJoin(string gamemode)
        {
            InModdedRoom = true;
            Logger.LogInfo($"[The Monkey Mod Menu] Joined modded room: {gamemode}");
        }

        [ModdedGamemodeLeave]
        private void OnLeave(string gamemode)
        {
            InModdedRoom = false;
            Logger.LogInfo("[The Monkey Mod Menu] Left modded room");
        }
    }
}