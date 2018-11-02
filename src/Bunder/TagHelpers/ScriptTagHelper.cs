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

        protected override Task ProcessStaticAssetTagAsync(TagHelperContext context, TagHelperOutput output, IReadOnlyList<Asset> assets)
        {
            for (int i = 0; i < assets.Count; i++)
            {
                if (i == 0)
                {
                    output.Attributes.Add("src", assets[i].Value);
                    continue;
                }

                output.PostElement.AppendHtml($"<script src='{assets[i].Value}'></script>");
            }

            return Task.CompletedTask;
        }
    }
}
