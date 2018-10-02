using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bunder
{
    [HtmlTargetElement("link", Attributes = AttributeNameAsset)]
    public class LinkTagHelper : StaticAssetTagHelper
    {
        public LinkTagHelper(BunderSettings settings) 
            : base(settings)
        {
        }
    }
}
