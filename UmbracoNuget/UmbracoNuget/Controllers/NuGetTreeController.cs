using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using Umbraco.Core;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;
using Umbraco.Web.Trees;
using UmbracoNuget.Helpers;
using UmbracoNuget.Models;

namespace UmbracoNuget.Controllers
{
    [Tree("nuget", "Packages", "NuGet Packages")]
    [PluginController("NuGet")]
    public class NuGetTreeController : TreeController
    {
        protected override MenuItemCollection GetMenuForNode(string id, FormDataCollection queryStrings)
        {
            return null;
        }

        //TODO: Neaten this up...
        protected override TreeNodeCollection GetTreeNodes(string id, FormDataCollection queryStrings)
        {
            //Main Route
            var mainRoute = "/NuGet/Packages";

            //Nodes that we will return
            var nodes = new TreeNodeCollection();

            //Add nodes
            var treeNodes = new List<SectionTreeNode>();

            //check if we're rendering the root node's children
            if (id == Constants.System.Root.ToInvariantString())
            {
                treeNodes.Add(new SectionTreeNode() { Id = "browse", Title = "Browse Packages", Icon = "icon-box-alt", Route = string.Format("{0}/view/{1}", mainRoute, "browse"), HasChildren = false });
                treeNodes.Add(new SectionTreeNode() { Id = "search", Title = "Search for Packages", Icon = "icon-activity", Route = string.Format("{0}/view/{1}", mainRoute, "search"), HasChildren = false });

                //Check we have any installed packages
                if (PackageHelper.HasInstalledPackages())
                {
                    //Add installed packages item to tree
                    treeNodes.Add(new SectionTreeNode() { Id = "installed", Title = "Installed Packages", Icon = "icon-activity", Route = string.Format("{0}/view/{1}", mainRoute, "installed"), HasChildren = true });
                }

                treeNodes.Add(new SectionTreeNode() { Id = "local", Title = "Install Local Package", Icon = "icon-activity", Route = string.Format("{0}/view/{1}", mainRoute, "local"), HasChildren = false });
                treeNodes.Add(new SectionTreeNode() { Id = "created", Title = "Created Packages", Icon = "icon-activity", Route = string.Format("{0}/view/{1}", mainRoute, "created"), HasChildren = false });
                treeNodes.Add(new SectionTreeNode() { Id = "settings", Title = "Settings", Icon = "icon-settings", Route = string.Format("{0}/edit/{1}", mainRoute, "settings"), HasChildren = false });
                
            }


            //Check if we are rendering the installed tree
            if (id == "installed")
            {
                //Get installed packages
                var installedPackages = PackageHelper.ListInstalledPackages();

                foreach (var package in installedPackages)
                {
                    treeNodes.Add(new SectionTreeNode() { Id = package.Id, Title = package.Title, Icon = "icon-activity", Route = string.Format("{0}/view/{1}", mainRoute, "installed-package"), HasChildren = false });
                }
            }


            //Write out the tree nodes...
            if (treeNodes.Any())
            {
                foreach (var item in treeNodes)
                {
                    //When clicked - /App_Plugins/NuGet/backoffice/Packages/edit.html
                    //URL in address bar - /developer/NuGet/General/someID
                    var nodeToAdd = CreateTreeNode(item.Id, null, queryStrings, item.Title, item.Icon, item.HasChildren, item.Route);

                    //Add it to the collection
                    nodes.Add(nodeToAdd);
                }

                //Return the nodes
                return nodes;
            }


            //this tree doesn't suport rendering more than 1 level
            throw new NotSupportedException();
        }
    }
}
