using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bunder.TagHelpers
{
    [HtmlTargetElement("script", Attributes = AttributeNameAsset)]
    public class ScriptTagHelper : StaticAssetTagHelper
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
                    output.Attributes.Add("src", assets[i]);
                    continue;
                }

                output.PostContent.AppendHtml($"<script src='{assets[i]}'></script>");
            }

            return Task.CompletedTask;
        }
    }
}
