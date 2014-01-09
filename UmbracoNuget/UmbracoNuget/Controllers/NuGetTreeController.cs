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
        public const string mainRoute = "/NuGet/Packages";

        protected override MenuItemCollection GetMenuForNode(string id, FormDataCollection queryStrings)
        {
            return null;
        }

        /// <summary>
        /// Main Overide for items in our tree for our NuGet section/app
        /// </summary>
        /// <param name="id"></param>
        /// <param name="queryStrings"></param>
        /// <returns></returns>
        protected override TreeNodeCollection GetTreeNodes(string id, FormDataCollection queryStrings)
        {
            //Nodes that we will return
            var nodes = new TreeNodeCollection();

            //Add nodes
            var treeNodes = new List<SectionTreeNode>();

            //check if we're rendering the root node's children
            if (id == Constants.System.Root.ToInvariantString())
            {
                //Call GetMainTreeItems()
                treeNodes = GetMainTreeItems();
            }


            //Check if we are rendering the installed tree
            if (id == "installed")
            {
                treeNodes = GetInstalledPackagesTreeItems();
            }

            //Check if we are rendering the updates tree
            if (id == "updates")
            {
                treeNodes = GetPackageUpdatesTreeItems();
            }


            //Create tree nodes
            if (treeNodes.Any())
            {
                //Write out the tree nodes...
                nodes = CreateTreeNodes(treeNodes, queryStrings);

                //Return the nodes
                return nodes;
            }


            //this tree doesn't suport rendering more than 1 level
            throw new NotSupportedException();
        }


        /// <summary>
        /// Main Tree when section loads
        /// </summary>
        /// <returns></returns>
        public List<SectionTreeNode> GetMainTreeItems()
        {
            //Add nodes
            var treeNodes = new List<SectionTreeNode>();

            treeNodes.Add(new SectionTreeNode() { Id = "browse", Title = "Browse Packages", Icon = "icon-box-alt", Route = string.Format("{0}/view/{1}", mainRoute, "browse"), HasChildren = false });
            treeNodes.Add(new SectionTreeNode() { Id = "search", Title = "Search for Packages", Icon = "icon-search", Route = string.Format("{0}/view/{1}", mainRoute, "search"), HasChildren = false });

            //Check we have any installed packages
            if (PackageHelper.HasInstalledPackages())
            {
                //Add installed packages item to tree
                treeNodes.Add(new SectionTreeNode() { Id = "installed", Title = "Installed Packages", Icon = "icon-box", Route = string.Format("{0}/view/{1}", mainRoute, "installed"), HasChildren = true });

                //Add package updates item to tree if we have updates available
                if (PackageHelper.HasUpdates())
                {
                    treeNodes.Add(new SectionTreeNode() { Id = "updates", Title = "Package Updates", Icon = "icon-cloud-upload", Route = string.Format("{0}/view/{1}", mainRoute, "updates"), HasChildren = true });
                }
            }

            //treeNodes.Add(new SectionTreeNode() { Id = "local", Title = "Install Local Package", Icon = "icon-cloud-upload", Route = string.Format("{0}/view/{1}", mainRoute, "local"), HasChildren = false });
            //treeNodes.Add(new SectionTreeNode() { Id = "created", Title = "Created Packages", Icon = "icon-brick", Route = string.Format("{0}/view/{1}", mainRoute, "created"), HasChildren = false });

            return treeNodes;
        }


        /// <summary>
        /// Used when Installed packages node is expanded (Fetches installed packages)
        /// </summary>
        /// <returns></returns>
        public List<SectionTreeNode> GetInstalledPackagesTreeItems()
        {
            //Add nodes
            var treeNodes = new List<SectionTreeNode>();

            //Get installed packages
            var installedPackages = PackageHelper.ListInstalledPackages();

            foreach (var package in installedPackages)
            {
                treeNodes.Add(new SectionTreeNode() { Id = package.Id, Title = package.Title, Icon = "icon-box-open", Route = string.Format("{0}/view/{1}", mainRoute, "installed-package"), HasChildren = false });
            }

            return treeNodes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<SectionTreeNode> GetPackageUpdatesTreeItems()
        {
            //Add nodes
            var treeNodes = new List<SectionTreeNode>();

            //Get package updates
            var packageUpdates = PackageHelper.ListPackageUpdates();

            foreach (var package in packageUpdates)
            {
                treeNodes.Add(new SectionTreeNode() { Id = package.Id, Title = package.Title, Icon = "icon-box-open", Route = string.Format("{0}/view/{1}", mainRoute, "update"), HasChildren = false });
            }

            return treeNodes;
        }

        /// <summary>
        /// Creates the tree nodes from our list we pass in
        /// </summary>
        /// <param name="treeNodes"></param>
        /// <param name="queryStrings"></param>
        /// <returns></returns>
        public TreeNodeCollection CreateTreeNodes(List<SectionTreeNode> treeNodes, FormDataCollection queryStrings)
        {
            //Nodes that we will return
            var nodes = new TreeNodeCollection();

            foreach (var item in treeNodes)
            {
                //When clicked - /App_Plugins/NuGet/backoffice/Packages/edit.html
                //URL in address bar - /developer/NuGet/General/someID
                var nodeToAdd = CreateTreeNode(item.Id, null, queryStrings, item.Title, item.Icon, item.HasChildren, item.Route);

                //Add it to the collection
                nodes.Add(nodeToAdd);
            }

            return nodes;
        }
    }

}
