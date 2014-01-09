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
    }
}
