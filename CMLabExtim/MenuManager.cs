using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using CMLabExtim.Menuitems;

namespace CMLabExtim
{
    public class MenuManager
    {
        public static void LoadMenu(Menu menu, List<BnMenuItem> bnMenuItems)
        {
            if (menu.Visible != true) return;
            menu.Items.Clear();
            foreach (var bnMenuItem in bnMenuItems)
            {
                if (bnMenuItem.Position.IndexOf('.') >= 0) continue;
                var mi = new MenuItem(bnMenuItem.Text, bnMenuItem.Value)
                {
                    Selectable = bnMenuItem.Selectable,
                    Enabled = bnMenuItem.Enabled,
                    ToolTip = bnMenuItem.ToolTip
                };
                if (bnMenuItem.Visible != true) continue;
                MenuAddChildItems(bnMenuItems, bnMenuItem.Position, mi);
                menu.Items.Add(mi);
            }
        }

        private static void MenuAddChildItems(List<BnMenuItem> bnMenuItems, string position, MenuItem parentItem)
        {
            var parentPathSteps = position.Split('.');
            var parentLevel = parentPathSteps.Length;
            var parentPath = position;

            foreach (var bnMenuItem in bnMenuItems)
            {
                var childPathSteps = bnMenuItem.Position.Split('.');
                var childLevel = childPathSteps.Length;

                if (childLevel != parentLevel + 1) continue;
                var childParentPath = bnMenuItem.Position.Substring(0, bnMenuItem.Position.Length - 4);

                if (childParentPath != parentPath) continue;
                var mi = new MenuItem(bnMenuItem.Text, bnMenuItem.Value)
                {
                    Selectable = bnMenuItem.Selectable,
                    Enabled = bnMenuItem.Enabled,
                    ToolTip = bnMenuItem.ToolTip
                };
                if (bnMenuItem.Visible != true) continue;
                var jump = parentItem.ChildItems.Count > 0
                           && parentItem.ChildItems[parentItem.ChildItems.Count - 1].Text == "------"
                           && mi.Text == "------";
                if (jump) continue;
                parentItem.ChildItems.Add(mi);
                MenuAddChildItems(bnMenuItems, bnMenuItem.Position, mi);
            }
        }

        public static MenuItem GetMenuItemByValue(Menu menu, string value)
        {
            var foundItem = new MenuItem();
            foreach (MenuItem mi in menu.Items)
            {
                if (String.CompareOrdinal(mi.Value, value) == 0)
                {
                    foundItem = mi;
                    break;
                }
                foundItem = GetMenuSubItemByValue(mi, value);
                if (foundItem.Text != String.Empty)
                {
                    break;
                }
            }
            return foundItem;
        }

        private static MenuItem GetMenuSubItemByValue(MenuItem menuItem, string value)
        {
            var foundItem = new MenuItem();
            foreach (MenuItem mi in menuItem.ChildItems)
            {
                if (String.CompareOrdinal(mi.Value, value) == 0)
                {
                    foundItem = mi;
                    break;
                }
                foundItem = GetMenuSubItemByValue(mi, value);
                if (foundItem.Text != String.Empty)
                {
                    break;
                }
            }
            return foundItem;
        }

        public static void LoadTree(TreeView treeView, List<BnMenuItem> bnMenuItems)
        {
            if (treeView.Visible != true) return;
            treeView.Nodes.Clear();
            foreach (var bnMenuItem in bnMenuItems)
            {
                if (bnMenuItem.Position.IndexOf('.') >= 0) continue;
                var mi = new TreeNode(bnMenuItem.Text, bnMenuItem.Value)
                {
                    ShowCheckBox = true,
                    ToolTip = bnMenuItem.ToolTip,
                    Checked = bnMenuItem.Visible,
                    SelectAction = TreeNodeSelectAction.None
                };
                TreeAddChildItems(bnMenuItems, bnMenuItem.Position, mi);
                treeView.Nodes.Add(mi);
            }
        }

        private static void TreeAddChildItems(IEnumerable<BnMenuItem> bnMenuItems, string position, TreeNode parentItem)
        {
            var parentPathSteps = position.Split('.');
            var parentLevel = parentPathSteps.Length;
            var parentPath = position;

            var menuItems = bnMenuItems as BnMenuItem[] ?? bnMenuItems.ToArray();
            foreach (var bnMenuItem in menuItems)
            {
                var childPathSteps = bnMenuItem.Position.Split('.');
                var childLevel = childPathSteps.Length;

                if (childLevel != parentLevel + 1) continue;
                var childParentPath = bnMenuItem.Position.Substring(0, bnMenuItem.Position.Length - 3);

                if (childParentPath != parentPath) continue;
                var mi = new TreeNode(bnMenuItem.Text, bnMenuItem.Value)
                {
                    ShowCheckBox = true,
                    ToolTip = bnMenuItem.ToolTip,
                    Checked = bnMenuItem.Visible,
                    SelectAction = TreeNodeSelectAction.None
                };
                parentItem.ChildNodes.Add(mi);
                TreeAddChildItems(menuItems, bnMenuItem.Position, mi);
            }
        }
    }
}