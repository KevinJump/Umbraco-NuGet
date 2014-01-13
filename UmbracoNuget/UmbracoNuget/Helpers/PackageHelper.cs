using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NuGet;
using UmbracoNuget.Services;

namespace UmbracoNuget.Helpers
{
    public static class PackageHelper
    {
        public static bool HasInstalledPackages()
        {
            //Get Package Manager
            var packageManager = PackageManagerService.Instance.PackageManager;

            //Count number of installed packages
            var installedPackages = packageManager.LocalRepository.GetPackages().Count();

            return installedPackages > 0;
        }

        public static List<IPackage> ListInstalledPackages()
        {
            //Get Package Manager
            var packageManager = PackageManagerService.Instance.PackageManager;

            //Return the list of packages
            return packageManager.LocalRepository.GetPackages().ToList();
        }

        public static bool HasUpdates()
        {
            //Get Package Manager
            var packageManager = PackageManagerService.Instance.PackageManager;

            //Get the current installed packages
            var installedPackages = packageManager.LocalRepository.GetPackages();

            //Get Updates for the set of installed packages
            var packageUpdates = packageManager.SourceRepository.GetUpdates(installedPackages, false, false);

            //Return bool if we have any updates
            return packageUpdates.Any();
        }

        public static List<IPackage> ListPackageUpdates()
        {
            //Get Package Manager
            var packageManager = PackageManagerService.Instance.PackageManager;

            //Get the current installed packages
            var installedPackages = packageManager.LocalRepository.GetPackages();

            //Get Updates for the set of installed packages
            var packageUpdates = packageManager.SourceRepository.GetUpdates(installedPackages, false, false);

            return packageUpdates.ToList();
        }

        public static bool PackageHasUpdates(string packageID, string version)
        {
            var packageManager = PackageManagerService.Instance.PackageManager;

            //Convert version to SemanticVersion
            var semVersion = SemanticVersion.Parse(version);

            //Get the package
            var getPackage = packageManager.LocalRepository.FindPackagesById(packageID).Where(x => x.Version == semVersion);

            //Get Updates for the set of installed packages
            var packageUpdates = packageManager.SourceRepository.GetUpdates(getPackage, false, false);

            //Return bool if we have any updates
            return packageUpdates.Any();
        }

        public static List<IPackage> ListUpdatesForPackage(string packageID, string version)
        {
            var packageManager = PackageManagerService.Instance.PackageManager;

            //Convert version to SemanticVersion
            var semVersion = SemanticVersion.Parse(version);

            //Get the package
            var getPackage = packageManager.LocalRepository.FindPackagesById(packageID).Where(x => x.Version == semVersion);

            //Get Updates for the set of installed packages
            var packageUpdates = packageManager.SourceRepository.GetUpdates(getPackage, false, false);
            
            return packageUpdates.ToList();
        }

        public static string GetTotalDownloads(this IPackage packge)
        {
            var packageManager = PackageManagerService.Instance.PackageManager;

            //Go & find the package by the ID
            var findPackages = packageManager.SourceRepository.FindPackagesById(packge.Id);

            if (findPackages == null)
            {
                return string.Empty;
            }

            //For each package we find add the download count so we have a total download count
            var totalDownloads = 0;

            //Loop over all versions of the package
            foreach (var package in findPackages)
            {
                totalDownloads += package.DownloadCount;
            }

            return totalDownloads.ToString("##,###,###");
        }


    }
}
