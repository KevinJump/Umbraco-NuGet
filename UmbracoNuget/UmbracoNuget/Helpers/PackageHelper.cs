using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
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


        public static void CopyPackageFiles(this IPackage package)
        {
            //For Content Files Copy the files to the correct places
            foreach (var file in package.GetContentFiles())
            {
                //Copy File from package to correct location on disk
                var fileLocation = file.EffectivePath;

                //Map Path from location
                var mappedFileLocation = HostingEnvironment.MapPath("~/" + fileLocation);

                //Ensure directory exists
                if (!Directory.Exists(Path.GetDirectoryName(mappedFileLocation)))
                {
                    //Directory does NOT exist
                    //Create it
                    Directory.CreateDirectory(Path.GetDirectoryName(mappedFileLocation));
                }
                
                //Get File Contents
                var fileContents = file.GetStream();

                //Save file to disk
                //http://stackoverflow.com/questions/411592/how-do-i-save-a-stream-to-a-file
                using (var fileStream = File.Create(mappedFileLocation))
                {
                    fileContents.CopyTo(fileStream);
                }

            }

            //For Lib files (aka /bin)
            foreach (var file in package.GetLibFiles())
            {
                //Copy File from package to /bin folder
                var fileLocation = file.EffectivePath;

                //Map Path from location
                var mappedFileLocation = HostingEnvironment.MapPath("~/bin/" + fileLocation);

                //Ensure directory exists (I hope so as it's the /bin folder)
                if (!Directory.Exists(Path.GetDirectoryName(mappedFileLocation)))
                {
                    //Directory does NOT exist
                    //Create it
                    Directory.CreateDirectory(Path.GetDirectoryName(mappedFileLocation));
                }

                //Get File Contents
                var fileContents = file.GetStream();

                //Save file to disk
                //http://stackoverflow.com/questions/411592/how-do-i-save-a-stream-to-a-file
                using (var fileStream = File.Create(mappedFileLocation))
                {
                    fileContents.CopyTo(fileStream);
                }


            }
        }

        public static void RemovePackageFiles(this IPackage package)
        {
            //For Content Files remove the files off disk
            foreach (var file in package.GetContentFiles())
            {
                //Remove File from disk

                //Remove File that its package from disk
                var fileLocation = file.EffectivePath;

                //Map Path from location
                var mappedFileLocation = HostingEnvironment.MapPath("~/" + fileLocation);

                //Ensure file exists on disk
                if (File.Exists(mappedFileLocation))
                {
                    //It exists - so let's delete it
                    File.Delete(mappedFileLocation);
                }
            }

            //Remove the directories
            foreach (var dir in package.GetContentFiles())
            {
                //Remove File that its package from disk
                var fileLocation = dir.EffectivePath;

                //Map Path from location
                var mappedFileLocation = HostingEnvironment.MapPath("~/" + fileLocation);

                //Get the directory to delete
                var directoryPath = Path.GetDirectoryName(mappedFileLocation);

                //Delete the directory
                Directory.Delete(directoryPath);
            }

            //For Lib files (aka /bin)
            foreach (var file in package.GetLibFiles())
            {
                //Remove File from to /bin folder

                //Copy File from package to /bin folder
                var fileLocation = file.EffectivePath;

                //Map Path from location
                var mappedFileLocation = HostingEnvironment.MapPath("~/bin/" + fileLocation);

                //Remove DLL from bin
                File.Delete(mappedFileLocation);
            }
        }




    }
}
