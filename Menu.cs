using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using The_Monkey_Mod_Menu.Mods;

namespace The_Monkey_Mod_Menu
{
    public class Menu : MonoBehaviour
    {
        private GameObject menuRoot;
        private Transform leftHand;

        private bool menuOpen;
        private bool prevLeftSecondary;

        private int pageNumber = 0;
        private const int ButtonsPerPage = 5;

        private readonly List<GameObject> spawnedButtons = new List<GameObject>();

        private enum Page
        {
            Main,
            Movement,
            Visual
        }

        private Page currentPage = Page.Main;

        private float nextY;
        private const float StartY = 0.12f;
        private const float StepY = 0.05f;

        private void Start()
        {
            StartCoroutine(WaitAndBuild());
        }

        private IEnumerator WaitAndBuild()
        {
            while (GorillaTagger.Instance == null || GorillaTagger.Instance.leftHandTransform == null)
                yield return null;

            leftHand = GorillaTagger.Instance.leftHandTransform;

            BuildMenuRoot();
            SetOpen(false);
            RebuildPage();
        }

        private void Update()
        {
            if (!Plugin.InModdedRoom)
            {
                if (menuOpen)
                    SetOpen(false);

                prevLeftSecondary = false;
                return;
            }

            if (ControllerInputPoller.instance == null || menuRoot == null)
                return;

            bool pressed = ControllerInputPoller.instance.leftControllerSecondaryButton;

            if (pressed && !prevLeftSecondary)
            {
                SetOpen(!menuOpen);

                if (menuOpen)
                    RebuildPage();
                else
                    Utils.NotifiLib.SetText("");
            }

            prevLeftSecondary = pressed;

            if (menuOpen)
            {
                menuRoot.transform.localPosition = new Vector3(0.03f, 0.02f, 0.08f);
                menuRoot.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
            }
        }

        private void BuildMenuRoot()
        {
            menuRoot = new GameObject("HandMenuRoot");
            menuRoot.transform.SetParent(leftHand, false);

            GameObject plate = GameObject.CreatePrimitive(PrimitiveType.Cube);
            plate.transform.SetParent(menuRoot.transform, false);
            plate.transform.localScale = new Vector3(0.15f, 0.25f, 0.01f);
            plate.transform.localPosition = new Vector3(0f, 0.05f, 0.01f);

            Renderer renderer = plate.GetComponent<Renderer>();
            renderer.material.shader = Shader.Find("GorillaTag/UberShader");
            renderer.material.color = Color.black;

            Destroy(plate.GetComponent<BoxCollider>());
        }

        private void RebuildPage()
        {
            if (menuRoot == null)
                return;

            ClearButtons();
            BeginLayout();
            CreateTitle3D("THE MONKE MENU ");

            List<Action> buttonList = new List<Action>();

            if (currentPage == Page.Main)
            {
                pageNumber = 0;

                CreateCategoryButton("Movement Mods >", "Browse movement mods", () =>
                {
                    currentPage = Page.Movement;
                    pageNumber = 0;
                    RebuildPage();
                });

                CreateCategoryButton("Visual Mods >", "Browse visual mods", () =>
                {
                    currentPage = Page.Visual;
                    pageNumber = 0;
                    RebuildPage();
                });

                return;
            }

            if (currentPage == Page.Movement)
            {
                buttonList.Add(() => CreateCategoryButton("< BACK", "Return to main menu", () =>
                {
                    currentPage = Page.Main;
                    pageNumber = 0;
                    RebuildPage();
                }));

                buttonList.Add(() => CreateToggleButton($"Fly: {(Movement.flyEnabled ? "ON" : "OFF")}", "Fly while holding right primary", Movement.flyEnabled, () =>
                {
                    Movement.flyEnabled = !Movement.flyEnabled;
                    RebuildPage();
                }));

                buttonList.Add(() => CreateToggleButton($"Speed: {(Movement.speedBoostEnabled ? "ON" : "OFF")}", "Small speed boost", Movement.speedBoostEnabled, () =>
                {
                    Movement.speedBoostEnabled = !Movement.speedBoostEnabled;
                    if (Movement.speedBoostEnabled)
                    {
                        Movement.maxspeedBoostEnabled = false;
                        Movement.SUPERDUPERSPEEDBOOST = false;
                    }
                    RebuildPage();
                }));

                buttonList.Add(() => CreateToggleButton($"Long Arms: {(Movement.LongArmsEnabled ? "ON" : "OFF")}", "Increase player scale", Movement.LongArmsEnabled, () =>
                {
                    Movement.LongArmsEnabled = !Movement.LongArmsEnabled;
                    RebuildPage();
                }));

                buttonList.Add(() => CreateToggleButton("Reset Arms", "Reset your scale to normal", false, () =>
                {
                    Movement.ResetLongarms = true;
                    RebuildPage();
                }));

                buttonList.Add(() => CreateToggleButton($"Super Speed: {(Movement.maxspeedBoostEnabled ? "ON" : "OFF")}", "Big speed boost", Movement.maxspeedBoostEnabled, () =>
                {
                    Movement.maxspeedBoostEnabled = !Movement.maxspeedBoostEnabled;
                    if (Movement.maxspeedBoostEnabled)
                    {
                        Movement.speedBoostEnabled = false;
                        Movement.SUPERDUPERSPEEDBOOST = false;
                    }
                    RebuildPage();
                }));

                buttonList.Add(() => CreateToggleButton($"SuperDUPER Speed: {(Movement.SUPERDUPERSPEEDBOOST ? "ON" : "OFF")}", "Very high speed boost", Movement.SUPERDUPERSPEEDBOOST, () =>
                {
                    Movement.SUPERDUPERSPEEDBOOST = !Movement.SUPERDUPERSPEEDBOOST;
                    if (Movement.SUPERDUPERSPEEDBOOST)
                    {
                        Movement.speedBoostEnabled = false;
                        Movement.maxspeedBoostEnabled = false;
                    }
                    RebuildPage();
                }));

                buttonList.Add(() => CreateToggleButton($"Platforms: {(Movement.platsenabled ? "ON" : "OFF")}", "Hold grip to use platforms", Movement.platsenabled, () =>
                {
                    Movement.platsenabled = !Movement.platsenabled;
                    RebuildPage();
                }));

                buttonList.Add(() => CreateToggleButton($"Copy Rig: {(Movement.rigCopyEnabled ? "ON" : "OFF")}", "Copy another player's rig", Movement.rigCopyEnabled, () =>
                {
                    Movement.rigCopyEnabled = !Movement.rigCopyEnabled;
                    RebuildPage();
                }));

                buttonList.Add(() => CreateToggleButton($"NoClip: {(Movement.NoCilpEnabled ? "ON" : "OFF")}", "Disables your body collider while held", Movement.NoCilpEnabled, () =>
                {
                    Movement.NoCilpEnabled = !Movement.NoCilpEnabled;
                    RebuildPage();
                }));

                buttonList.Add(() => CreateToggleButton($"Checkpoint: {(Movement.checkPointSet ? "ON" : "OFF")}", "Place checkpoint with trigger, teleport with grab", Movement.checkPointSet, () =>
                {
                    Movement.checkPointSet = !Movement.checkPointSet;
                    RebuildPage();
                }));

                buttonList.Add(() => CreateToggleButton($"Tag Random: {(Movement.TagRandomPlayerEnabled ? "ON" : "OFF")}", "Teleport to a random player", Movement.TagRandomPlayerEnabled, () =>
                {
                    Movement.TagRandomPlayerEnabled = !Movement.TagRandomPlayerEnabled;
                    RebuildPage();
                }));

                buttonList.Add(() => CreateToggleButton($"No Finger Movement: {(Movement.NFMEnabled ? "ON" : "OFF")}", "Locks finger input", Movement.NFMEnabled, () =>
                {
                    Movement.NFMEnabled = !Movement.NFMEnabled;
                    RebuildPage();
                }));

                buttonList.Add(() => CreateToggleButton($"WASD Fly: {(Movement.WASDFlyEnabled ? "ON" : "OFF")}", "Use keyboard to fly", Movement.WASDFlyEnabled, () =>
                {
                    Movement.WASDFlyEnabled = !Movement.WASDFlyEnabled;
                    RebuildPage();
                }));

                buttonList.Add(() => CreateToggleButton($"Ghost Monke: {(Movement.GhostMonkeEnabled ? "ON" : "OFF")}", "Hide offline rig while held", Movement.GhostMonkeEnabled, () =>
                {
                    Movement.GhostMonkeEnabled = !Movement.GhostMonkeEnabled;
                    RebuildPage();
                }));
            }
            buttonList.Add(() => CreateToggleButton($"Invisible: {(Movement.invisibleEnabled ? "ON" : "OFF")}", "Become invisible", Movement.invisibleEnabled, () =>
            {
                Movement.invisibleEnabled = !Movement.invisibleEnabled;
                RebuildPage();
            }));


            if (currentPage == Page.Visual)
            {
                buttonList.Add(() => CreateCategoryButton("< BACK", "Return to main menu", () =>
                {
                    currentPage = Page.Main;
                    pageNumber = 0;
                    RebuildPage();
                }));

                buttonList.Add(() => CreateToggleButton($"Tracers: {(Visual.tracersEnabled ? "ON" : "OFF")}", "Draw lines to players", Visual.tracersEnabled, () =>
                {
                    Visual.tracersEnabled = !Visual.tracersEnabled;
                    RebuildPage();
                }));

                buttonList.Add(() => CreateToggleButton($"Info Gun: {(Visual.InfoGun ? "ON" : "OFF")}", "Show target name and distance", Visual.InfoGun, () =>
                {
                    Visual.InfoGun = !Visual.InfoGun;
                    RebuildPage();
                }));

                buttonList.Add(() => CreateToggleButton($"Skeleton ESP: {(Visual.SkeletonEnabled ? "ON" : "OFF")}", "Draw simple skeleton lines", Visual.SkeletonEnabled, () =>
                {
                    Visual.SkeletonEnabled = !Visual.SkeletonEnabled;
                    RebuildPage();
                }));

                buttonList.Add(() => CreateToggleButton($"Chams: {(Visual.ChamsEnabled ? "ON" : "OFF")}", "Highlight players through walls", Visual.ChamsEnabled, () =>
                {
                    Visual.ChamsEnabled = !Visual.ChamsEnabled;
                    RebuildPage();
                }));




            }

            int maxPage = Mathf.Max(0, (buttonList.Count - 1) / ButtonsPerPage);
            pageNumber = Mathf.Clamp(pageNumber, 0, maxPage);

            int startIdx = pageNumber * ButtonsPerPage;
            int endIdx = Mathf.Min(startIdx + ButtonsPerPage, buttonList.Count);

            for (int i = startIdx; i < endIdx; i++)
                buttonList[i].Invoke();

            nextY = -0.20f;

            if (pageNumber > 0)
            {
                Create3DButton("<", Color.cyan, "Previous Page", () =>
                {
                    pageNumber--;
                    RebuildPage();
                });
            }

            Create3DButton($"Page {pageNumber + 1}/{maxPage + 1}", Color.gray, "Current Page", null);

            if (pageNumber < maxPage)
            {
                Create3DButton(">", Color.cyan, "Next Page", () =>
                {
                    pageNumber++;
                    RebuildPage();
                });
            }
        }

        private void BeginLayout()
        {
            nextY = StartY;
        }

        private void ClearButtons()
        {
            foreach (GameObject obj in spawnedButtons)
            {
                if (obj != null)
                    Destroy(obj);
            }

            spawnedButtons.Clear();
        }

        private void CreateTitle3D(string text)
        {
            GameObject titleObj = new GameObject("Title");
            titleObj.transform.SetParent(menuRoot.transform, false);
            titleObj.transform.localPosition = new Vector3(0f, 0.165f, 0f);

            TextMesh tm = titleObj.AddComponent<TextMesh>();
            tm.text = text;
            tm.fontSize = 64;
            tm.characterSize = 0.0045f;
            tm.anchor = TextAnchor.MiddleCenter;
            tm.color = Color.white;

            spawnedButtons.Add(titleObj);
        }

        private void CreateCategoryButton(string label, string description, Action onPress)
        {
            Create3DButton(label, new Color(0.15f, 0.15f, 0.15f, 1f), description, onPress);
        }

        private void CreateToggleButton(string label, string description, bool enabled, Action onPress)
        {
            Color color = enabled ? new Color(0f, 0.5f, 0f, 1f) : new Color(0.5f, 0f, 0f, 1f);
            Create3DButton(label, color, description, onPress);
        }

        private void Create3DButton(string label, Color color, string description, Action onPress)
        {
            GameObject btnObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            btnObj.transform.SetParent(menuRoot.transform, false);

            btnObj.transform.localScale = new Vector3(0.13f, 0.035f, 0.015f);
            btnObj.transform.localPosition = new Vector3(0f, nextY, 0f);

            Renderer renderer = btnObj.GetComponent<Renderer>();
            renderer.material.shader = Shader.Find("GorillaTag/UberShader");
            renderer.material.color = color == new Color(0.15f, 0.15f, 0.15f, 1f) ? Color.cyan : color;

            Destroy(btnObj.GetComponent<BoxCollider>());

            GameObject textObj = new GameObject("Label");
            textObj.transform.SetParent(btnObj.transform, false);
            textObj.transform.localPosition = new Vector3(0f, 0f, -0.55f);

            TextMesh tm = textObj.AddComponent<TextMesh>();
            tm.text = label.ToUpper();
            tm.fontSize = 50;
            tm.characterSize = 0.02f;
            tm.anchor = TextAnchor.MiddleCenter;
            tm.color = Color.black;

            WorldButton wb = btnObj.AddComponent<WorldButton>();
            wb.OnPress = onPress;
            wb.tooltipDescription = description;

            spawnedButtons.Add(btnObj);
            nextY -= StepY;
        }

        private void SetOpen(bool open)
        {
            menuOpen = open;

            if (menuRoot != null)
                menuRoot.SetActive(open);

            if (!open)
                Utils.NotifiLib.SetText("");
        }

        private class WorldButton : MonoBehaviour
        {
            public Action OnPress;
            public string tooltipDescription = "";

            private static float globalCooldown;
            private float nextCheckTime;

            private void Update()
            {
                if (Time.time < nextCheckTime)
                    return;

                nextCheckTime = Time.time + 0.03f;

                if (!Plugin.InModdedRoom || GorillaLocomotion.GTPlayer.Instance == null)
                    return;

                Transform rHand = GorillaLocomotion.GTPlayer.Instance.GetControllerTransform(false);
                if (rHand == null)
                    return;

                float dist = Vector3.Distance(rHand.position, transform.position);

                if (dist < 0.12f)
                {
                    Utils.NotifiLib.SetText(tooltipDescription);
                }
                else if (dist > 0.12f && dist < 0.15f)
                {
                    Utils.NotifiLib.SetText("");
                }

                if (dist < 0.07f && Time.time > globalCooldown)
                {
                    globalCooldown = Time.time + 0.35f;
                    OnPress?.Invoke();
                }
            }
        }
    }
}