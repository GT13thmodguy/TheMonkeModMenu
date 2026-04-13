using System;
using The_Monkey_Mod_Menu.Mods;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

namespace The_Monkey_Mod_Menu
{
    public class ScreenGUI : MonoBehaviour
    {
        public static bool showGUI = true;

        private Rect windowRect = new Rect(50, 50, 300, 350);

        private enum Page
        {
            Main,
            Movement,
            Visual
        }

        private Page currentPage = Page.Main;

        private void Update()
        {
            if (Keyboard.current.insertKey.isPressed)
            {
                showGUI = !showGUI;
            }
        }

        private void OnGUI()
        {
            if (!showGUI || !Plugin.InModdedRoom) return;

            // COLORS
            Color glassGreen = new Color(0.3f, 1f, 0.3f, 0.25f); // transparent green
            Color solidGreen = new Color(0.2f, 0.8f, 0.2f, 1f);   // solid header

            // WINDOW BACKGROUND (glass)
            GUI.color = glassGreen;
            GUI.Box(windowRect, "");

            // HEADER (solid)
            GUI.color = solidGreen;
            GUI.Box(new Rect(windowRect.x, windowRect.y, windowRect.width, 30), "THE MONKE MENU V1.0");

            GUI.color = Color.white;

            GUILayout.BeginArea(new Rect(windowRect.x + 10, windowRect.y + 40, windowRect.width - 20, windowRect.height - 50));

            // ================= MAIN PAGE =================
            if (currentPage == Page.Main)
            {
                GUILayout.Label("Main Menu");

                if (GUILayout.Button("Movement Mods >"))
                {
                    currentPage = Page.Movement;
                }

                if (GUILayout.Button("Visual Mods >"))
                {
                    currentPage = Page.Visual;
                }
            }

            // ================= MOVEMENT =================
            if (currentPage == Page.Movement)
            {
                GUILayout.Label("Movement");

                if (GUILayout.Button("< Back"))
                {
                    currentPage = Page.Main;
                }

                Movement.flyEnabled = GUILayout.Toggle(Movement.flyEnabled, "Fly");
                Movement.speedBoostEnabled = GUILayout.Toggle(Movement.speedBoostEnabled, "Speed");
                Movement.LongArmsEnabled = GUILayout.Toggle(Movement.LongArmsEnabled, "Long Arms");
                Movement.platsenabled = GUILayout.Toggle(Movement.platsenabled, "Platforms");
                Movement.NoCilpEnabled = GUILayout.Toggle(Movement.NoCilpEnabled, "NoClip");
                Movement.rigCopyEnabled = GUILayout.Toggle(Movement.rigCopyEnabled, "Rig Copy");
                Movement.checkPointSet = GUILayout.Toggle(Movement.checkPointSet, "Checkpoint");
                Movement.TagRandomPlayerEnabled = GUILayout.Toggle(Movement.TagRandomPlayerEnabled, "Tag Random Player");
                Movement.NFMEnabled = GUILayout.Toggle(Movement.NFMEnabled, "No Finger Movement");
                Movement.WASDFlyEnabled = GUILayout.Toggle(Movement.WASDFlyEnabled, "WASD Fly");
                Movement.GhostMonkeEnabled = GUILayout.Toggle(Movement.GhostMonkeEnabled, "Ghost Monke");
            }

            // ================= VISUAL =================
            if (currentPage == Page.Visual)
            {
                GUILayout.Label("Visual");

                if (GUILayout.Button("< Back"))
                {
                    currentPage = Page.Main;
                }

                Visual.tracersEnabled = GUILayout.Toggle(Visual.tracersEnabled, "Tracers");
                Visual.InfoGun = GUILayout.Toggle(Visual.InfoGun, "Info Gun");
                Visual.SkeletonEnabled = GUILayout.Toggle(Visual.SkeletonEnabled, "Skeleton");
                Visual.ChamsEnabled = GUILayout.Toggle(Visual.ChamsEnabled, "Chams");
            }

            GUILayout.EndArea();

            // DRAG WINDOW
            GUI.DragWindow(new Rect(windowRect.x, windowRect.y, windowRect.width, 30));
        }
    }
}