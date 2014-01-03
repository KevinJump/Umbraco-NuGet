using System;
using System.Collections.Generic;
using System.Net.Http.Formatting;
using Umbraco.Core;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;
using Umbraco.Web.Trees;
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

        protected override TreeNodeCollection GetTreeNodes(string id, FormDataCollection queryStrings)
        {
            //check if we're rendering the root node's children
            if (id == Constants.System.Root.ToInvariantString())
            {
                //Nodes that we will return
                var nodes = new TreeNodeCollection();

                //Main Route
                var mainRoute = "/NuGet/Packages";

                //Add nodes
                var treeNodes = new List<SectionTreeNode>();

                treeNodes.Add(new SectionTreeNode() { Id = "browse", Title = "Browse Packages", Icon = "icon-box-alt", Route = string.Format("{0}/view/{1}", mainRoute, "browse") });
                treeNodes.Add(new SectionTreeNode() { Id = "installed", Title = "Installed Packages", Icon = "icon-activity", Route = string.Format("{0}/view/{1}", mainRoute, "installed") });
                treeNodes.Add(new SectionTreeNode() { Id = "local", Title = "Install Local Package", Icon = "icon-activity", Route = string.Format("{0}/view/{1}", mainRoute, "local") });
                treeNodes.Add(new SectionTreeNode() { Id = "created", Title = "Created Packages", Icon = "icon-activity", Route = string.Format("{0}/view/{1}", mainRoute, "created") });
                treeNodes.Add(new SectionTreeNode() { Id = "settings", Title = "Settings", Icon = "icon-settings", Route = string.Format("{0}/edit/{1}", mainRoute, "settings") });


                foreach (var item in treeNodes)
                {
                    //When clicked - /App_Plugins/NuGet/backoffice/Packages/edit.html
                    //URL in address bar - /developer/NuGet/General/someID
                    var nodeToAdd = CreateTreeNode(item.Id, null, queryStrings, item.Title, item.Icon, false, item.Route);

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
