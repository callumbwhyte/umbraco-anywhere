using System;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Hosting;
using Umbraco.Extensions;

namespace UmbracoAnywhere
{
    public class DefaultHostingEnvironment : IHostingEnvironment
    {
        public string SiteName => "";

        public string ApplicationId => "";

        public string ApplicationPhysicalPath => "";

        public string LocalTempPath => "";

        public string ApplicationVirtualPath => "";

        public bool IsDebugMode => false;

        public bool IsHosted => true;

        public Uri ApplicationMainUrl => new Uri("/");

        public string MapPathContentRoot(string path)
        {
            return Environment.CurrentDirectory + path.TrimStart(Constants.CharArrays.TildeForwardSlash);
        }

        public string MapPathWebRoot(string path)
        {
            return Environment.CurrentDirectory + path.TrimStart(Constants.CharArrays.TildeForwardSlash);
        }

        public string ToAbsolute(string virtualPath)
        {
            if (!virtualPath.StartsWith("~/") && !virtualPath.StartsWith("/"))
            {
                throw new InvalidOperationException($"The value {virtualPath} for parameter {nameof(virtualPath)} must start with ~/ or /");
            }

            if (Uri.IsWellFormedUriString(virtualPath, UriKind.Absolute) == true)
            {
                return virtualPath;
            }

            return ApplicationVirtualPath.EnsureEndsWith("/") + virtualPath.TrimStart(Constants.CharArrays.TildeForwardSlash);
        }

        public void EnsureApplicationMainUrl(Uri currentApplicationUrl)
        {
            return;
        }
    }
}