﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DrawerTools
{
    public class DTTogglePanel : DTBase
    {
        public event Action<int> OnSelectionChange;

        public DTToggleButtonGroup Group { get; protected set; }
        public float Height { get; protected set; } = 18;
        public List<DTToggleButton> Buttons { get; protected set; }
        public bool AllowNotSelected { get => Group.AllowNotSelected; set => Group.AllowNotSelected = value; }

        public DTTogglePanel(params string[] names) => Prepare(18, names.Select(x => new GUIContent(x)).ToArray(), null);

        public DTTogglePanel(IEnumerable<string> names, int selectedPanel) => Prepare(18, names.Select(x => new GUIContent(x)).ToArray(), null, selectedPanel);
        public DTTogglePanel(IEnumerable<string> names, Action<int> callback) => Prepare(18, names.Select(x => new GUIContent(x)).ToArray(), callback);
        public DTTogglePanel(IEnumerable<string> names, Action<int> callback, float height) => Prepare(height, names.Select(x => new GUIContent(x)).ToArray(), callback);
        public DTTogglePanel(IEnumerable<string> names, Action<int> callback, float height, int selectedPanel) => Prepare(height, names.Select(x => new GUIContent(x)).ToArray(), callback, selectedPanel);

        public DTTogglePanel(IEnumerable<Texture> textures, Action<int> callback, float height) => Prepare(height, textures.Select(x => new GUIContent(x)).ToArray(), callback);
        public DTTogglePanel(IEnumerable<Texture> textures, Action<int> callback, float height, int selectedPanel) => Prepare(height, textures.Select(x => new GUIContent(x)).ToArray(), callback, selectedPanel);
        public DTTogglePanel(IEnumerable<Texture> textures, Action<int> callback, float height, int selectedPanel, IEnumerable<string> tooltips) => Prepare(height, textures.Select(x => new GUIContent(x, tooltips.ToArray()[textures.ToList().IndexOf(x)])).ToArray(), callback, selectedPanel);

        public DTTogglePanel(IEnumerable<IconType> icons, Action<int> callback, float height) => Prepare(height, icons.Select(x => new GUIContent(DTIcons.GetIcon(x))).ToArray(), callback);
        public DTTogglePanel(IEnumerable<IconType> icons, Action<int> callback, float height, int selectedPanel) => Prepare(height, icons.Select(x => new GUIContent(DTIcons.GetIcon(x))).ToArray(), callback, selectedPanel);
        public DTTogglePanel(IEnumerable<IconType> icons, Action<int> callback, float height, int selectedPanel, IEnumerable<string> tooltips) => Prepare(height, icons.Select(x => new GUIContent(DTIcons.GetIcon(x), tooltips.ToArray()[icons.ToList().IndexOf(x)])).ToArray(), callback, selectedPanel);

        public void AddButton(GUIContent content)
        {
            int id = Buttons.Count;
            var toggle = content.image == null 
                ? new DTToggleButton(content.text, (val) => { ListenButtonClicked(id); }, content.tooltip) 
                : new DTToggleButton(content.image, (val) => { ListenButtonClicked(id); }, content.tooltip);

            toggle.Height = Height;
            if (content.image != null)
            {
                toggle.Width = Height;
            }
            Buttons.Add(toggle);
            Group.Add(toggle);
        }

        public int SelectedID()
        {
            for (int i = 0; i < Buttons.Count; i++)
            {
                if (Buttons[i].Pressed)
                {
                    return i;
                }
            }
            return 0;
        }

        protected virtual void ListenButtonClicked(int id)
        {
            var btn = Buttons[id];
            if (!btn.Pressed)
            {
                return;
            }
            OnSelectionChange?.Invoke(id);
        }

        protected override void AtDraw()
        {
            using (DTScope.HorizontalBox)
            {
                Buttons.ForEach(x => x.Draw());
            }
        }

        private void Prepare(float height, GUIContent[] content, Action<int> callback, int selectedPanel = 0)
        {
            Group = new DTToggleButtonGroup();
            Height = height;
            Buttons = new List<DTToggleButton>();
            if (callback != null)
            {
                OnSelectionChange += callback;
            }
            foreach (var button in content)
            {
                AddButton(button);
            }
            Buttons[selectedPanel].SetPressed(true, false);
        }

    }
}