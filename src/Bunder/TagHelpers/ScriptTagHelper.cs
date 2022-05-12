using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            output.TagName = null;
            
            var scriptTagBuilder = new StringBuilder();

            foreach (var asset in assets)
            {
                scriptTagBuilder.Append($"<script src='{asset.Value}'");

                if (context.AllAttributes.Any(a => a.Name == "async"))
                    scriptTagBuilder.Append(" async");

                if (Settings.DefaultToDefer || context.AllAttributes.Any(a => a.Name == "defer"))
                    scriptTagBuilder.Append(" defer");

                scriptTagBuilder.Append("></script>");

                output.Content.AppendHtml(scriptTagBuilder.ToString());

                scriptTagBuilder.Clear();
            }

            return Task.CompletedTask;
        }
    }
}
