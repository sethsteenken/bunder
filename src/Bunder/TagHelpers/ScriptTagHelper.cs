using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bunder.TagHelpers
{
    [HtmlTargetElement("script", Attributes = AttributeNameAsset)]
    public sealed class ScriptTagHelper : StaticAssetTagHelper
    {
        public ScriptTagHelper(BunderSettings settings, IAssetResolver assetResolver) 
            : base(settings, assetResolver)
        {
            
        }

        protected override Task ProcessStaticAssetTagAsync(TagHelperContext context, TagHelperOutput output, IEnumerable<Asset> assets)
        {
            int index = 0;
            foreach (var asset in assets)
            {
                if (index == 0)
                {
                    output.Attributes.Add("src", asset.Value);
                    continue;
                }

                output.PostElement.AppendHtml($"<script src='{asset.Value}'></script>");
                index++;
            }

            return Task.CompletedTask;
        }
    }
}
