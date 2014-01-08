using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Routing;
using NuGet;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;
using UmbracoNuget.Models;

namespace UmbracoNuget.Controllers
{
    [PluginController("NuGet")]
    public class PackageApiController : UmbracoApiController
    {
        public const int PageSize   = 9;
        public const string RepoUrl = "https://packages.nuget.org/api/v2";

        public PackagesResponse GetPackages(int page = 0)
        {
            //Connect to the official package repository
            IPackageRepository repo = PackageRepositoryFactory.Default.CreateRepository(RepoUrl);

            //Get the number of packages in the repo (latest version)
            //Is there a way - to get this only once as it won't change between pages I hope?!
            var totalcount = repo.GetPackages().Where(x => x.IsLatestVersion).Count();
            var totalPages = (int)Math.Ceiling((double)totalcount / PageSize);

            //Paging from here
            //http://bitoftech.net/2013/11/25/implement-resources-pagination-asp-net-web-api/
            var urlHelper   = new UrlHelper(Request);
            //var prevLink    = page > 0 ? urlHelper.Link("PackageApi", new { page = page - 1 }) : string.Empty;
            //var nextLink    = page < totalPages - 1 ? urlHelper.Link("PackageApi", new { page = page + 1 }) : string.Empty;

            //Get the list of all NuGet packages (latest version)
            List<IPackage> packages = repo.GetPackages().Where(x => x.IsLatestVersion).OrderByDescending(x => x.DownloadCount).Skip(page * PageSize).Take(PageSize).ToList();

            var packageData = new List<Package>();

            foreach (var package in packages)
            {
                var packageToAdd = new Package();

                packageToAdd.Authors        = package.Authors;
                packageToAdd.Description    = package.Description;
                packageToAdd.DownloadCount  = package.DownloadCount;
                packageToAdd.IconUrl        = package.IconUrl;
                packageToAdd.Id             = package.Id;
                packageToAdd.ProjectUrl     = package.ProjectUrl;
                packageToAdd.Published      = package.Published;
                packageToAdd.Summary        = package.Summary;
                packageToAdd.Tags           = package.Tags;
                packageToAdd.Title          = package.Title;
                packageToAdd.Version        = package.Version;

                //Add it to the list
                packageData.Add(packageToAdd);

            }

            //Build up object to return
            var packageResponse             = new PackagesResponse();
            packageResponse.Packages        = packageData;
            packageResponse.NoResults       = totalcount;
            packageResponse.PreviousLink    = null;
            packageResponse.NextLink        = null;

            //Return the package response
            return packageResponse;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="packageID"></param>
        /// <returns></returns>
        public string GetPackageDetail(string packageID)
        {
            return null;
        }

        /// <summary>
        /// 
        /// <returns></returns>
        public bool GetPackageInstall()
        {
            var repo        = PackageRepositoryFactory.Default.CreateRepository("https://packages.nuget.org/api/v2");
            var path        = new DefaultPackagePathResolver("https://packages.nuget.org/api/v2");
            var fileSystem  = new PhysicalFileSystem(HostingEnvironment.MapPath("~/"));

            //Create a NuGet Package Manager
            var packageManager = new PackageManager(repo, path, fileSystem);

            bool isInstalled = false;

            //Install package
            try
            {
                var packageVersion = SemanticVersion.Parse("2.0.3");

                //Install the package...
                packageManager.InstallPackage("jQuery", packageVersion, false, false);

                //Set flag to true
                isInstalled = true;

            }
            catch (Exception)
            {
                //Some error - set flag to false
                isInstalled = false;
                //throw;
            }

            //Returned bool if it's installed or not
            return isInstalled;        
        }

    }
}
