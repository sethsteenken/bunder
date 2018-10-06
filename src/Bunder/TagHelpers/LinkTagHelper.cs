using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bunder.TagHelpers
{
    [HtmlTargetElement("link", Attributes = AttributeNameAsset)]
    public class LinkTagHelper : StaticAssetTagHelper
    {
        public LinkTagHelper(BunderSettings settings, IAssetResolver assetResolver) 
            : base(settings, assetResolver)
        {
        }

        protected override Task ProcessStaticAssetTagAsync(TagHelperContext context, TagHelperOutput output, IReadOnlyList<Asset> assets)
        {
            for (int i = 0; i < assets.Count; i++)
            {
                if (i == 0)
                {
                    output.Attributes.Add("href", assets[i]);
                    output.Attributes.Add("rel", "stylesheet");
                    continue;
                }

                output.PostContent.AppendHtml($"<link href='{assets[i]}' rel='stylesheet' />");
            }

            return Task.CompletedTask;
        }
    }
}
