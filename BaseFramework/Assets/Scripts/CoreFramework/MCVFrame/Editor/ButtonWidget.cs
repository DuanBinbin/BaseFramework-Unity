﻿/// <copyright file="ButtonWidget.cs">Copyright (c) 2015 All Rights Reserved</copyright>
/// <author>Joris van Leeuwen</author>
/// <date>01/27/2014</date>

using UnityEngine;
using System.Collections;
using UnityEditor;

namespace CoreFramework.Editor {

    public class ButtonWidget: Widget {

        public const float Width = 120;
        public const float Height = 30;

        public static string Tooltip;

        public bool IsHovered {
            get {
                return rect.Contains(CoreFrameworkMonitorWindow.MousePosition - CoreFrameworkMonitorWindow.WindowOffset - CoreFrameworkMonitorWindow.WindowRect.position);
            }
        }

        public override Vector2 Position {
            get {
                return new Vector2(rect.x + .5f * Width, rect.y + .5f * Height);
            }
            set {
                rect.x = value.x - .5f * Width;
                rect.y = value.y - .5f * Height;
            }
        }

        private const float TooltipHoverDuration = 1.2f;
        private const float TooltipShowDuration = 2.5f;

        private static GUIStyle buttonRightClickStyle;
        private static GUIStyle buttonStyle;

        private Rect rect;
        private float durationHovered;

        public ButtonWidget() {
            rect = new Rect(0, 0, Width, Height);
            durationHovered = 0.0f;
        }

        public virtual void Render() {
            if (buttonRightClickStyle == null) {
                buttonRightClickStyle = new GUIStyle(GUI.skin.button);
                buttonRightClickStyle.active.textColor = Color.white;
                buttonRightClickStyle.active.background = buttonRightClickStyle.normal.background;

                buttonStyle = new GUIStyle(buttonRightClickStyle);
                buttonStyle.active.textColor = buttonStyle.normal.textColor;
            }

            Rect buttonRect = new Rect(rect.x + CoreFrameworkMonitorWindow.WindowOffset.x, rect.y + CoreFrameworkMonitorWindow.WindowOffset.y, rect.width, rect.height);

            if (buttonRect.Contains(CoreFrameworkMonitorWindow.MousePosition - CoreFrameworkMonitorWindow.WindowRect.position)) {
                durationHovered += CoreFrameworkMonitorWindow.DeltaTime;
                if (durationHovered >= TooltipHoverDuration && durationHovered <= TooltipHoverDuration + TooltipShowDuration) {
                    Tooltip = "Right click...";
                }
            } else {
                durationHovered = 0.0f;
            }

            GUIStyle style = Event.current.button == 1 ? buttonRightClickStyle : buttonStyle;
            if (GUI.Button(buttonRect, GetText(), style) && Event.current.button == 1) {
                Tooltip = "";
                ShowContextMenu();
            }
        }

        public virtual void RenderMiniMap() {
            Vector2 position = CoreFrameworkMonitorWindow.MiniMapWindowRect.position + (rect.position + CoreFrameworkMonitorWindow.WindowPadding) * CoreFrameworkMonitorWindow.MiniMapScaleFactor;
            Vector2 size = rect.size * CoreFrameworkMonitorWindow.MiniMapScaleFactor;
            RenderingHelper.RenderRect(new Rect(position.x, position.y, size.x, size.y), new Color(1.0f, 1.0f, 1.0f, .5f * (CoreFrameworkMonitorWindow.IsHoveringMiniMap ? 1.0f : CoreFrameworkEditorStyles.MiniMapMouseOutAlpha)), CoreFrameworkMonitorWindow.MiniMapWindowRect);
        }

        protected virtual void ShowContextMenu() {

        }

        protected virtual string GetText() {
            return "";
        }
    }

}