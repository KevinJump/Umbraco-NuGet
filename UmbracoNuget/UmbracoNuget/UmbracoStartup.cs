using umbraco.cms.businesslogic.packager;
using Umbraco.Core;
using UmbracoNuget.Services;

namespace UmbracoNuget
{
    public class UmbracoStartup : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            //Check to see if section needs to be added
            Install.AddSection(applicationContext);

            //Check to see if language keys for section needs to be added
            Install.AddSectionLanguageKeys();

            //Add OLD Style Package Event
            InstalledPackage.BeforeDelete += InstalledPackage_BeforeDelete;

            //Get the package manager instance & wire up events
            var packageManager = PackageManagerService.Instance.PackageManager;
            packageManager.PackageInstalled += packageManager_PackageInstalled;
            packageManager.PackageUninstalled += packageManager_PackageUninstalled;
        }

        void packageManager_PackageUninstalled(object sender, NuGet.PackageOperationEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        void packageManager_PackageInstalled(object sender, NuGet.PackageOperationEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        void InstalledPackage_BeforeDelete(InstalledPackage sender, System.EventArgs e)
        {
            //Check which package is being uninstalled
            if (sender.Data.Name == "NuGet")
            {
                //Start Uninstall - clean up process...
                //Uninstall.RemoveSection();
                //Uninstall.RemoveSectionLanguageKeys();
            }
        }
    }
}
