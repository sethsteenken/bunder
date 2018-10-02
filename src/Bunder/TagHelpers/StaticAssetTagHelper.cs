using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bunder
{
    public abstract class StaticAssetTagHelper : TagHelper
    {
        public const string AttributeNameAsset = "asset";

        protected StaticAssetTagHelper(BunderSettings settings)
        {
            Settings = settings;
        }

        protected BunderSettings Settings { get; private set; }


        [HtmlAttributeName(AttributeNameAsset)]
        public string Asset { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            await base.ProcessAsync(context, output);
        }
    }
}
