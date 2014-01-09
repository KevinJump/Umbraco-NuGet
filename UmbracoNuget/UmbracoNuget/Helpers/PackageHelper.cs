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


    }
}
