using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Routing;
using NuGet;
using Umbraco.Core;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;
using UmbracoNuget.Models;
using UmbracoNuget.Services;
using PackageHelper = UmbracoNuget.Helpers.PackageHelper;
using umb = Umbraco.Web;

namespace UmbracoNuget.Controllers
{
    [PluginController("NuGet")]
    public class PackageApiController : UmbracoApiController
    {
        public const int PageSize           = 9;
        public const string NuGetRepoUrl    = "https://packages.nuget.org/api/v2";
        public const string MyGetRepoUrl    = "https://www.myget.org/F/umbraco-community/";

        public PackagesResponse GetPackages(string sortBy, int page = 1)
        {
            var zeroPageIndex = page - 1;

            //Connect to the official package repository
            IPackageRepository repo = PackageRepositoryFactory.Default.CreateRepository(NuGetRepoUrl);

            //Get the number of packages in the repo (latest version)
            //Is there a way - to get this only once as it won't change between pages I hope?!
            var totalcount = repo.GetPackages().Where(x => x.IsLatestVersion).Count();
            var totalPages = (int)Math.Ceiling((double)totalcount / PageSize);

            //Paging from here
            //http://bitoftech.net/2013/11/25/implement-resources-pagination-asp-net-web-api/

            var prevLink = zeroPageIndex > 0 ? (page - 1).ToString() : string.Empty;
            var nextLink = zeroPageIndex < totalPages - 1 ? (page + 1).ToString() : string.Empty;

            List<IPackage> packages = new List<IPackage>();


            switch (sortBy)
            {
                case "downloads":
                    packages = repo.GetPackages()
                        .Where(x => x.IsLatestVersion)
                        .OrderByDescending(x => x.DownloadCount)
                        .Skip(zeroPageIndex * PageSize)
                        .Take(PageSize).ToList();

                    break;

                case "recent":
                    packages = repo.GetPackages()
                        .Where(x => x.IsLatestVersion)
                        .OrderByDescending(x => x.Published)
                        .Skip(zeroPageIndex * PageSize)
                        .Take(PageSize).ToList();
                    break;

                case "a-z":
                    packages = repo.GetPackages()
                        .Where(x => x.IsLatestVersion)
                        .OrderBy(x => x.Id)
                        .Skip(zeroPageIndex * PageSize)
                        .Take(PageSize).ToList();
                    break;

                default:
                    packages = repo.GetPackages()
                        .Where(x => x.IsLatestVersion)
                        .OrderByDescending(x => x.DownloadCount)
                        .Skip(zeroPageIndex * PageSize)
                        .Take(PageSize).ToList();
                    break;
            }


            //The rows we will return
            var rows = new List<Row>();

            foreach (IEnumerable<IPackage> row in packages.InGroupsOf(3))
            {
                var packagesInRow = new List<Package>();

                foreach (IPackage package in row)
                {
                    var packageToAdd            = new Package();
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

                    //Add the package to the row object
                    packagesInRow.Add(packageToAdd);
                }

                //Add the row to to the list of rows
                var packageRow      = new Row();
                packageRow.Packages = packagesInRow;

                rows.Add(packageRow);
            }
            
            //Build up object to return
            var packageResponse             = new PackagesResponse();
            packageResponse.Rows            = rows;
            packageResponse.TotalItems      = totalcount;
            packageResponse.TotalPages      = totalPages;
            packageResponse.CurrentPage     = page;
            packageResponse.PreviousLink    = prevLink;
            packageResponse.NextLink        = nextLink;

            //Return the package response
            return packageResponse;
        }

        [HttpGet]
        public PackagesResponse SearchPackages(string searchTerm, int page = 1)
        {
            var zeroPageIndex = page - 1;

            //Connect to the official package repository
            IPackageRepository repo = PackageRepositoryFactory.Default.CreateRepository(NuGetRepoUrl);


            var searchPackages = repo.Search(searchTerm, false);

            //Search for packages with search term
            var packages = searchPackages
                .Where(x => x.IsLatestVersion)
                .OrderByDescending(x => x.DownloadCount)
                .Skip(zeroPageIndex * PageSize)
                .Take(PageSize).ToList();

            //Get the number of packages in the repo (latest version)

            var totalcount = searchPackages.Where(x => x.IsLatestVersion).Count();
            var totalPages = (int)Math.Ceiling((double)totalcount / PageSize);

            //Paging from here
            //http://bitoftech.net/2013/11/25/implement-resources-pagination-asp-net-web-api/

            var prevLink = zeroPageIndex > 0 ? (page - 1).ToString() : string.Empty;
            var nextLink = zeroPageIndex < totalPages - 1 ? (page + 1).ToString() : string.Empty;


            //The rows we will return
            var rows = new List<Row>();

            foreach (IEnumerable<IPackage> row in packages.InGroupsOf(3))
            {
                var packagesInRow = new List<Package>();

                foreach (IPackage package in row)
                {
                    var packageToAdd            = new Package();
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

                    //Add the package to the row object
                    packagesInRow.Add(packageToAdd);
                }

                //Add the row to to the list of rows
                var packageRow      = new Row();
                packageRow.Packages = packagesInRow;

                rows.Add(packageRow);
            }

            //Build up object to return
            var packageResponse             = new PackagesResponse();
            packageResponse.Rows            = rows;
            packageResponse.TotalItems      = totalcount;
            packageResponse.TotalPages      = totalPages;
            packageResponse.CurrentPage     = page;
            packageResponse.PreviousLink    = prevLink;
            packageResponse.NextLink        = nextLink;

            //Return the package response
            return packageResponse;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<IPackage> GetInstalledPackages()
        {
            var installedPackages = PackageHelper.ListInstalledPackages();

            return installedPackages;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool GetHasInstalledPackages()
        {
            //Get Package Manager
            var packageManager = PackageManagerService.Instance.PackageManager;

            //Count number of installed packages
            var installedPackages = packageManager.LocalRepository.GetPackages().Count();

            return installedPackages > 0;

        }

        /// <summary>
        /// 
        /// <returns></returns>
        public bool GetPackageInstall()
        {
            //Get Package Manager
            var packageManager = PackageManagerService.Instance.PackageManager;

            bool isInstalled = false;

            //Install package
            try
            {
                var packageVersion = SemanticVersion.Parse("1.0.0");

                //Install the package...
                packageManager.InstallPackage("Analytics", packageVersion, false, false);

                var package = packageManager.LocalRepository.FindPackage("Analytics", packageVersion);
                if (package != null)
                {
                    foreach (var packageFile in package.GetContentFiles())
                    {
                        var y = packageFile.EffectivePath;
                    }
                }

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="packageID"></param>
        /// <returns></returns>
        public IPackage GetPackageDetail(string packageID)
        {
            //Get Package Manager
            var packageManager = PackageManagerService.Instance.PackageManager;

            //Find the package in the MyGet Repo based on the packageID
            var findPackage = packageManager.SourceRepository.FindPackage(packageID);

            //Return the found package from the repo
            return findPackage;
        }

    }
}
