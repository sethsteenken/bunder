# Bunder
> AspNetCore bundle handler and renderer. 

Alternative to AspNetCore's [bundling process](https://docs.microsoft.com/en-us/aspnet/core/client-side/bundling-and-minification?view=aspnetcore-2.1&tabs=visual-studio) by utliziing shared json configuration for bundles and tag helpers for rendering assets similar to ASP.NET MVC's bundling process with Script.Render("~/bundle/path"). Relies on outside task runner like Gulp or Grunt to perform the minification / bundling with the intention for said tasks to utilize the same json configuration used by Bunder.

[![Build status](https://sethsteenken.visualstudio.com/Bunder%20DevOps/_apis/build/status/Bunder%20DevOps%20CI)](https://sethsteenken.visualstudio.com/Bunder%20DevOps/_build/latest?definitionId=1)

## Installing and Setup

Bunder is available as a [Nuget Package](https://www.nuget.org/packages/Bunder). Add to AspNetCore-based project via Nuget Package Manager in Visual Studio or via Package Manager Console.

```shell
PM> Install-Package Bunder
```

### Service Collection Configuration
Inside Startup.cs in an AspNetCore project:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    //...
    services.AddMvc();
    //...
    services.AddBunder();
}
```

### Bundle Configurations
By default, bundle configurations are searlized from a JSON file at bundles.json.

Bundles.json:
```json
[
  {
    "Name": "style-bundle",
    "Files": [
      "source_files/styles.css"
    ]
  },
  {
    "Name": "my-bundle-one",
    "Files": [
      "source_files/test_one.js",
      "source_files/test_two.js"
    ]
  },
  {
    "Name": "my-bundle-two",
    "Files": [
      "source_files/test_one.js",
      "source_files/test_two.js",
      "source_files/test_three.js"
    ]
   }
]
```

### Bunder Settings
By default, Bunder will pull configuration settings from "Bunder" section of appsettings.json.
```json
{
  "Bunder": {
    "UseBundledOutput": true,
    "UseVersioning": true,
    "BundlesConfigFilePath": "bundles.json",
    "OutputDirectories": {
      "js": "/output/js",
      "css": "/output/css"
    }
  }
}
```

### View Settings
Update _ViewImports.cshtml file to include Bunder and its tag helpers.

```cshtml
@using Bunder;

@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
@addTagHelper *, Bunder
```

## Use
Bunder provides tag helpers for rendering static assets like scripts and stylesheets in Views. Similar to ASP.NET MVC's Scripts.Render, Bunder's tag helpers accept a bundle's name as an "asset". Configuration settings will then determine if the bundled output or source files should be rendered.

```cshtml
<link asset="style-bundle" />
<script asset="my-bundle-one"></script>
```

## Contributing

If you'd like to contribute, please fork the repository and use a feature
branch. Pull requests are more than welcome!

## Links

- Project / Repository: https://github.com/sethsteenken/Bunder
- Issue tracker: https://github.com/sethsteenken/Bunder/issues
- Related projects:
  - Integreat: https://github.com/sethsteenken/Integreat
  - NetAssist: https://github.com/sethsteenken/net-assist


## Licensing

The code in this project is licensed under MIT license. [View the license here](LICENSE.md)
