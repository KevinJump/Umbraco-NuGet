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

            //Package Manager events
            var packageManger                   = PackageManagerService.Instance.PackageManager;
            packageManger.PackageInstalling     += packageManger_PackageInstalling;
            packageManger.PackageInstalled      += packageManger_PackageInstalled;
            packageManger.PackageUninstalling   += packageManger_PackageUninstalling;
            packageManger.PackageUninstalled    += packageManger_PackageUninstalled;

        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void packageManger_PackageInstalling(object sender, NuGet.PackageOperationEventArgs e)
        {
            //Whilst package is installing
            //Try & find package.xml at root of package in Content Files collection

            //If so using that package.xml use PackageService
            var packagingService = ApplicationContext.Current.Services.PackagingService;

            /*
            packagingService.ImportDataTypeDefinitions();
            packagingService.ImportLanguages();
            packagingService.ImportDictionaryItems();
            packagingService.ImportTemplates();
            packagingService.ImportContentTypes();
            packagingService.ImportContent();
            */
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void packageManger_PackageInstalled(object sender, NuGet.PackageOperationEventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void packageManger_PackageUninstalling(object sender, NuGet.PackageOperationEventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void packageManger_PackageUninstalled(object sender, NuGet.PackageOperationEventArgs e)
        {

        }


        /// <summary>
        /// Before Delete of this NuGet Extension package
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
