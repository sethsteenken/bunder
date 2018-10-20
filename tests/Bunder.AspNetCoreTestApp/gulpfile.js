/// <binding AfterBuild='bundle' />
"use strict";

var gulp = require("gulp"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify"),
    newer = require("gulp-newer"),
    clean = require('gulp-clean');

// get Bunder settings and bundle definitions from json config files
var bunderSettings = require("./appsettings.json").Bunder,
    basePath = "./wwwroot/",
    bundleConfigs = require("./" + bunderSettings.BundlesConfigFilePath);

function ToBool(value) {
    if (value === undefined) {
        return false;
    } else if (typeof value === 'boolean') {
        return value;
    } else if (typeof value === 'number') {
        value = value.toString();
    } else if (typeof value !== 'string') {
        return false;
    }

    switch (value.toLowerCase()) {
        case "true":
        case "yes":
        case "1":
            return true;
        default:
            return false;
    }
}

function Bundle(config, bunderSettings, basePath) {
    if (!config) {
        throw new Error("Bundle config paramater null.");
    }

    if (!config.Files || !config.Files.length) {
        throw new Error("Bundle must have at least one file under Files reference.");
    }

    var _ext = /(?:\.([^.]+))?$/.exec(config.OutputFileName || config.Files[0])[1];
    if (_ext) {
        _ext = _ext.toLowerCase();
    }

    this.Extension = _ext;
    this.Name = config.Name;
    this.OutputFileName = config.OutputFileName || this.Name.replace(" ", "_") + ".min." + this.Extension;
    this.SubPath = config.SubPath || "";
    this.Files = config.Files;
    this.OutputDirectory = config.OutputDirectory || bunderSettings.OutputDirectories[this.Extension] || "";

    // build output path
    var _outputPath = (basePath || "") + this.OutputDirectory + this.SubPath;
    if (_outputPath.slice(-1) != "/") {
        _outputPath += "/";
    }
    _outputPath += this.OutputFileName;

    this.OutputPath = _outputPath;

    this.Concat = function () {
        return concat(this.OutputPath);
    };

    this.Minify = function () {
        switch (this.Extension) {
            case "js":
                return uglify();
            case "css":
                return cssmin();
            default:
                throw new Error("No support for file extension '" + this.Extension + "' on Minify action.");
        }
    };

    // any custom properties found on in the json config for this bundle
    for (var prop in config) {
        if (!this[prop]) {
            this[prop] = config[prop];
        }
    }
}

// recursively build out list of files in a bundle
function BuildListOfFiles(bundle, bundlesList) {
    var bundleFiles = [];

    if (bundle && bundle.Files && bundle.Files.length) {
        for (var i = 0; i < bundle.Files.length; i++) {

            // if "file" is found as a bundle name, recursively get that bundle's files
            var existingBundle = bundlesList.filter(function (b) {
                return b.Name === bundle.Files[i];
            });

            if (existingBundle && existingBundle.length) {
                bundleFiles = bundleFiles.concat(BuildListOfFiles(existingBundle[0], bundlesList));
            } else {
                var file = bundle.Files[i];
                if (basePath && basePath.length)
                    file = basePath + file;
                bundleFiles.push(file);
            }
        }
    }

    return bundleFiles;
}

function BundleFiles(bundles, newerOnly) {
    if (bundles && bundles.length) {
        console.log("*** Starting bundling. Newer Only: " + newerOnly + " ***");

        var completedCount = 0,
            totalBundleCount = bundles.length;

        console.log("Bundle count: " + totalBundleCount);

        for (var i = 0; i < totalBundleCount; i++) {
            var bundle = bundles[i];

            if (ToBool(bundle.ReferenceOnly)) {
                console.log("Bundle for " + bundle.Name + " is set to only be referenced. No bundling for this bundle.");

                if (bundle.StaticOutputPath) {
                    (function (bundle) {
                        var gulpTask = gulp.src(basePath + bundle.StaticOutputPath, { base: "." })
                            .on("end", function () {
                                console.log("Bundle " + bundle.Name + " marked as have a *static output* of '" + bundle.StaticOutputPath + "'. It will have it's static output copied to destination.");
                            })
                            .pipe(bundle.Concat())
                            .pipe(gulp.dest("."))
                            .on("end", function () {
                                completedCount++;
                                console.log("Static Output '" + bundle.StaticOutputPath + "' copied to '" + bundle.OutputPath + "'.")
                            });
                    })(bundle);
                } else {
                    completedCount++;
                }

                continue;
            }

            var files = BuildListOfFiles(bundle, bundles),
                gulpTask = gulp.src(files, { base: "." });

            if (newerOnly) {
                gulpTask = gulpTask.pipe(newer(bundle.OutputPath));
            }

            (function (bundle, files) {
                gulpTask
                    .on("end", function () {
                        console.log("Bundling " + bundle.Name + " ... ");
                        for (var i = 0; i < files.length; i++) {
                            console.log(" - Includes file " + files[i] + ".");
                        }
                    })
                    .pipe(bundle.Concat())
                    .pipe(bundle.Minify())
                    .pipe(gulp.dest("."))
                    .on("end", function () {
                        completedCount++

                        if (completedCount == totalBundleCount) {
                            console.log("*** Bundling process complete. ***");
                        }
                    });
            })(bundle, files);
        }
    } else {
        console.log("No bundles found.");
    }
}

gulp.task("clean-output", function () {

    if (bunderSettings && bunderSettings.OutputDirectories) {
        for (var ext in bunderSettings.OutputDirectories) {
            gulp.src(bunderSettings.OutputDirectories[ext], { read: false })
                .on("end", function () {
                    console.log("* Cleaning destintation '" + bunderSettings.OutputDirectories[ext] + "'... *");
                })
                .pipe(clean())
                .on("end", function () {
                    console.log("* Cleaning complete. *");
                });
        }
    }
});

gulp.task("bundle", function () {
    BundleFiles(bundleConfigs.map(function (item) {
        return new Bundle(item, bunderSettings, basePath);
    }), true);
});

gulp.task("bundle-full", ["clean-output"], function () {
    BundleFiles(bundleConfigs.map(function (item) {
        return new Bundle(item, bunderSettings, basePath);
    }), false);
});