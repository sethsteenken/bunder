using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bunder
{
    [HtmlTargetElement("link", Attributes = AttributeNameAsset)]
    public class LinkTagHelper : StaticAssetTagHelper
    {
        public LinkTagHelper(BunderSettings settings, IAssetResolver assetResolver) 
            : base(settings, assetResolver)
        {
        }

        protected override Task ProcessStaticAssetTagAsync(TagHelperContext context, TagHelperOutput output, IEnumerable<Asset> assets)
        {
            throw new NotImplementedException();
        }
    }
}
