using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UnityPackage.Editor
{
    public class ExpansionPackageListTree : TreeView
    {
        private ExpansionWindow window;
        private Rect labelRect;
        private Rect versionRect;
        private Rect iconRect;
        private GUIStyle versionStyle = null;
        public ExpansionPackageListTree(TreeViewState state, ExpansionWindow window) : base(state)
        {
            this.window = window;
            showBorder = true;
            rowHeight = 25;
        }

        // Fix编码
        protected override TreeViewItem BuildRoot()
        {
            return CreateView();
        }

        public TreeViewItem CreateView()
        {
            TreeViewItem root = new TreeViewItem(0, -1);

            for (int i = 0; i < window.Packages.packages.Count; i++)
            {
                ExpansionPackageInfo info = window.Packages.packages[i];

                if (!info.show_in_list) continue;

                TreeViewItem child = new TreeViewItem(info.name.GetHashCode(), 0, info.displayName);
                 
                root.AddChild(child);
            }

             
             
            return root;
        }
         
        protected override void RowGUI(RowGUIArgs args)
        {
            ExpansionPackageInfo info = window.Packages.Get(args.item.id);

            bool installed = window.IsInstalled(info.name);

            labelRect.Set(args.rowRect.x + 25,args.rowRect.y,args.rowRect.width - 25 - 90,args.rowRect.height);
            EditorGUI.LabelField(labelRect, args.label);
  
            versionRect.Set(args.rowRect.x + args.rowRect.width - 120, args.rowRect.y + 2, 120, args.rowRect.height);

            string version = null;

            if(!installed)
                version = info.version;
            else
                version = window.GetPackage(info.name)?.version;

            if (info.preview)
                version = string.Format("<color=#FFA44E>pre</color>-{0}", version);

            if (versionStyle == null)
            {
                versionStyle = new GUIStyle("CN EntryInfo"); 
                versionStyle.richText = true;
            }

            EditorGUI.LabelField(versionRect, version, versionStyle);

            if (installed) 
            {
                iconRect.Set(args.rowRect.x + args.rowRect.width - 30, args.rowRect.y, 30, args.rowRect.height);
                EditorGUI.LabelField( iconRect, EditorGUIUtility.IconContent("TestPassed"));
            }


            GUI.Box(args.rowRect, string.Empty, "CN Box"); 
        }

        protected override void SelectionChanged(IList<int> selectedIds)
        {
            base.SelectionChanged(selectedIds); 
            window.ShowInfo(window.Packages.Get(selectedIds[0]));
        }
         

        protected override bool CanMultiSelect(TreeViewItem item)
        {
            return false;
        }

    }
}

