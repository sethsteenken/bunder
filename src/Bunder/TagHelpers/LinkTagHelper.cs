using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bunder.TagHelpers
{
    [HtmlTargetElement("link", Attributes = AttributeNameAsset)]
    public sealed class LinkTagHelper : StaticAssetTagHelper
    {
        public LinkTagHelper(BunderSettings settings, IAssetResolver assetResolver) 
            : base(settings, assetResolver)
        {
        }

        protected override Task ProcessStaticAssetTagAsync(TagHelperContext context, TagHelperOutput output, IEnumerable<Asset> assets)
        {
            int index = 0;
            foreach (var asset in assets)
            {
                index++;

                if (index == 1)
                {
                    output.Attributes.Add("href", asset.Value);
                    output.Attributes.Add("rel", "stylesheet");
                    continue;
                }

                output.PostElement.AppendHtml($"<link href='{asset.Value}' rel='stylesheet' />");
            }

            return Task.CompletedTask;
        }
    }
}
