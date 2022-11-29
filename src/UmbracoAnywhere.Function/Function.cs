using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Services;

namespace UmbracoAnywhere.Function
{
    public class Function
    {
        private readonly IContentService _contentService;

        public Function(IContentService contentService)
        {
            _contentService = contentService;
        }

        [FunctionName("GetItems")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "GET", "POST", Route = null)] HttpRequest req, ILogger log)
        {
            var items = _contentService.GetRootContent()
                .Select(x => x.Name);

            return new OkObjectResult(items);
        }
    }
}