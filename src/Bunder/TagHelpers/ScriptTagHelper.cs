using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bunder
{
    [HtmlTargetElement("script", Attributes = AttributeNameAsset)]
    public class ScriptTagHelper : StaticAssetTagHelper
    {
        public ScriptTagHelper(BunderSettings settings) 
            : base(settings)
        {

        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            // TODO

            await base.ProcessAsync(context, output);
        }
    }
}
