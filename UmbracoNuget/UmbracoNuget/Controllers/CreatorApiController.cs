using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;
using UmbracoNuget.Models;

namespace UmbracoNuget.Controllers
{
    [PluginController("NuGet")]
    public class CreatorApiController : UmbracoAuthorizedApiController
    {
        /// <summary>
        /// Used to get infro about the Umbraco install. List Macros, templates, doctypes 
        /// etc so user can pick & choose what to include package
        /// </summary>
        /// <returns></returns>
        public UmbracoInstallInfo GetUmbracoInfo()
        {
            var response = new UmbracoInstallInfo();

            //Content Types aka DocTypes
            response.ContentTypes = UmbracoContext.Application.Services.ContentTypeService.GetAllContentTypes();

            //Media Types (TODO: Can't serialise)
            //response.MediaTypes = UmbracoContext.Application.Services.ContentTypeService.GetAllMediaTypes();

            //Templates
            response.Templates = UmbracoContext.Application.Services.FileService.GetTemplates(); 

            //CSS - Only valid CSS files (as .txt file was getting picked up)
            response.Stylesheets = UmbracoContext.Application.Services.FileService.GetStylesheets().Where(x => x.IsFileValidCss());

            //Macros
            response.Macros = UmbracoContext.Application.Services.MacroService.GetAll();

            //Languages
            response.Languages = UmbracoContext.Application.Services.LocalizationService.GetAllLanguages();

            //Dictionary Items (TODO: Can't serialise)
            response.DictionaryItems = UmbracoContext.Application.Services.LocalizationService.GetRootDictionaryItems();

            //Data Types 
            response.DataTypes = UmbracoContext.Application.Services.DataTypeService.GetAllDataTypeDefinitions();

            //Return the object
            return response;
        }
    }
}
