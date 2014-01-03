
using System;
using System.Collections.Generic;
using System.Linq;
using NuGet;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;
using UmbracoNuget.Models;

namespace UmbracoNuget.Controllers
{
    [PluginController("NuGet")]
    public class PackageApiController : UmbracoAuthorizedApiController
    {
        /// <summary>
        /// http://localhost:64700/umbraco/NuGet/PackageApi/GetAllPackages
        /// </summary>
        /// <returns></returns>
        public List<Package> GetAllPackages()
        {
            //Connect to the official package repository
            IPackageRepository repo = PackageRepositoryFactory.Default.CreateRepository("https://packages.nuget.org/api/v2");

            //Get the list of all NuGet packages      
            List<IPackage> packages = repo.GetPackages().OrderBy(x => x.DownloadCount).Take(10).ToList();

            var packageItems = new List<Package>();
            
            foreach (var package in packages)
            {
                var packageToAdd = new Package();

                packageToAdd.AssemblyReferences         = package.AssemblyReferences;
                packageToAdd.Authors                    = package.Authors;
                packageToAdd.DependencySets             = package.DependencySets;
                packageToAdd.Description                = package.Description;
                packageToAdd.DownloadCount              = package.DownloadCount;
                packageToAdd.IconUrl                    = package.IconUrl;
                packageToAdd.Id                         = package.Id;
                packageToAdd.IsLatestVersion            = package.IsLatestVersion;
                packageToAdd.Owners                     = package.Owners;
                packageToAdd.PackageAssemblyReferences  = package.PackageAssemblyReferences;
                packageToAdd.ProjectUrl                 = package.ProjectUrl;
                packageToAdd.Published                  = package.Published;
                packageToAdd.Summary                    = package.Summary;
                packageToAdd.Tags                       = package.Tags;
                packageToAdd.Title                      = package.Title;
                packageToAdd.Version                    = package.Version;
                
                //Add the package
                packageItems.Add(packageToAdd);
            }

            //Return the packages
            return packageItems;
        }
    }
}
