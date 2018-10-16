using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

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
                string path = HttpUtility.UrlEncode(assets[i].Value);

                if (i == 0)
                {
                    output.Attributes.Add("src", path);
                    continue;
                }

                output.PostContent.AppendHtml($"<script src='{path}'></script>");
            }

            return Task.CompletedTask;
        }
    }
}
