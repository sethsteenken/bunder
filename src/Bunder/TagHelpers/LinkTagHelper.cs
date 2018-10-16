using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace Bunder.TagHelpers
{
    [HtmlTargetElement("link", Attributes = AttributeNameAsset)]
    public sealed class LinkTagHelper : StaticAssetTagHelper
    {
        public LinkTagHelper(BunderSettings settings, IAssetResolver assetResolver) 
            : base(settings, assetResolver)
        {
        }

        protected override Task ProcessStaticAssetTagAsync(TagHelperContext context, TagHelperOutput output, IReadOnlyList<Asset> assets)
        {
            for (int i = 0; i < assets.Count; i++)
            {
                string path = HttpUtility.UrlEncode(assets[i].Value);


                if (i == 0)
                {
                    output.Attributes.Add("href", path);
                    output.Attributes.Add("rel", "stylesheet");
                    continue;
                }

                output.PostContent.AppendHtml($"<link href='{path}' rel='stylesheet' />");
            }

            return Task.CompletedTask;
        }
    }
}
