using System.Collections.Generic;
using System.Linq;
using NuGet;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;
using UmbracoNuget.Models;

namespace UmbracoNuget.Controllers
{
    [PluginController("NuGet")]
    public class PackageApiController : UmbracoApiController
    {
        public const int PageSize   = 10;
        public const string RepoUrl = "https://packages.nuget.org/api/v2";

        public PackagesResponse GetPackages(int page = 0)
        {
            //Connect to the official package repository
            IPackageRepository repo = PackageRepositoryFactory.Default.CreateRepository(RepoUrl);

            //Get the number of packages in the repo (latest version)
            //Is there a way - to get this only once as it won't change between pages I hope?!
            var count = repo.GetPackages().Where(x => x.IsLatestVersion).Count();

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
            packageResponse.NoResults       = count;
            packageResponse.PreviousLink    = null;     //TODO
            packageResponse.NextLink        = null;     //TODO

            //Return the package response
            return packageResponse;
        }
    }
}
