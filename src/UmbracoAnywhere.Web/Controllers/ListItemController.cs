using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.Common.Controllers;

namespace UmbracoAnywhere.Web
{
    public class ListItemController : UmbracoApiController
    {
        private readonly IContentService _contentService;

        public ListItemController(IContentService contentService)
        {
            _contentService = contentService;
        }

        [HttpGet]
        public IEnumerable<string> GetItems()
        {
            var items = _contentService.GetRootContent()
                .Select(x => x.Name);

            return items;
        }
    }
}