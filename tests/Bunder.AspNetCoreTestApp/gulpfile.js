"use strict";

var settings = {
    paths: {
        scriptsDestDirectory: "./wwwroot/content/js/",
        stylesDestDirectory: "./wwwroot/content/css/"
    },
    basePath: "./wwwroot/" // appended to filepaths defined in bundle.Files
}

var gulp = require("gulp"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify"),
    newer = require("gulp-newer"),
    clean = require('gulp-clean');

var baseBath = "./wwwroot/";

// get bundle definitions from json config file
var bundles = require("./bundles.json"),
    appSettings = require("./appsettings.json"),
    bunderSettings = appSettings.Bunder;


// build out script bundles based on explicitly defined "type" or check file extension of first file in list
var scriptBundles = bundles.filter(function (item) {
    return (item.type !== undefined && (item.type === "js" || item.type === "JS"))
        || (item.type === undefined && item.files !== undefined && item.files.length > 0 && item.files.some(function (file) { return file.endsWith("js"); }));
});

// build out style bundles based on explicitly defined "type" or check file extension of first file in list
var styleBundles = bundles.filter(function (item) {
    return (item.type !== undefined && (item.type === "css" || item.type === "CSS"))
        || (item.type === undefined && item.files !== undefined && item.files.length > 0 && item.files.some(function (file) { return file.endsWith("css"); }));
});

// recursively build out list of files in a bundle
function BuildFiles(bundle, bundlesList) {
    var bundleFiles = [];

    if (bundle && bundle.files && bundle.files.length) {
        for (var j = 0; j < bundle.files.length; j++) {
            //if "file" is found as a bundle name, recursively get that bundle's files
            var existingBundle = bundlesList.filter(function (b) {
                return b.name === bundle.files[j];
            });

            if (existingBundle && existingBundle.length) {
                bundleFiles = bundleFiles.concat(BuildFiles(existingBundle[0], bundlesList));
            } else {
                var file = bundle.files[j];
                if (settings.basePath && settings.basePath.length)
                    file = settings.basePath + file;
                bundleFiles.push(file);
            }
        }
    }

    return bundleFiles;
}

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

function Argument(args, key, defaultValue) {
    var self = this;

    function _getArgumentValue(args, key) {
        var option,
            index = args.indexOf(key);

        if (index > -1 && args.length > (index + 1)) {
            return args[index + 1];
        }

        return undefined;
    }

    self.Value = defaultValue;
    self.Argument = _getArgumentValue(args, key);

    if (self.Argument !== undefined)
        self.Value = self.Argument;
}

function BoolArgument(args, key, defaultValue) {
    var self = this;

    function _getArgumentValue(args, key) {
        var option,
            index = args.indexOf(key);

        if (index > -1 && args.length > (index + 1)) {
            return args[index + 1];
        }

        return undefined;
    }

    self.Value = defaultValue === undefined ? false : defaultValue;
    self.Argument = _getArgumentValue(args, key);

    if (self.Argument !== undefined)
        self.Value = ToBool(self.Argument);
}

gulp.task("clean-bunder-output", function () {

    var _gulp = gulp;

    if (bunderSettings && bunderSettings.OutputDirectories) {
        for (var i = 0; i < bunderSettings.OutputDirectories.length; i++) {
            _gulp = _gulp.src(bunderSettings.OutputDirectories[i][1], { read: false })
                .on("end", function () {
                    console.log("* Cleaning destintation '" + bunderSettings.OutputDirectories[i][1] + "'... *");
                })
                .pipe(clean())
                .on("end", function () {
                    console.log("* Cleaning complete. *");
                });
        }
    }

    return _gulp;
});

function BundleJS(newerOnly) {
    if (scriptBundles && scriptBundles.length) {
        console.log("*** Starting JS bundling. Newer Only: " + newerOnly + " ***");

        var completedCount = 0,
            totalBundleCount = scriptBundles.length;

        for (var i = 0; i < totalBundleCount; i++) {
            var scriptBundle = scriptBundles[i];

            var dest = settings.paths.scriptsDestDirectory;
            dest += scriptBundle.subpath || "";

            if (dest.slice(-1) != "/") { dest += "/"; }

            dest += scriptBundle.filename || (scriptBundle.name + ".min.js");

            if (ToBool(scriptBundle.referenceOnly)) {
                console.log("Bundle for " + scriptBundle.name + " is set to only be referenced. No bundling for this bundle.");

                if (scriptBundle.staticOutputPath) {
                    (function (scriptBundle, dest) {
                        var gulpTask = gulp.src(settings.basePath + scriptBundle.staticOutputPath, { base: "." })
                            .on("end", function () {
                                console.log("Bundle " + scriptBundle.name + " marked as have a *static output* of '" + scriptBundle.staticOutputPath + "'. It will have it's static output copied to destination.");
                            })
                            .pipe(concat(dest))
                            .pipe(gulp.dest("."))
                            .on("end", function () {
                                completedCount++;
                                console.log("Static Output '" + scriptBundle.staticOutputPath + "' copied to '" + dest + "'.")
                            });
                    })(scriptBundle, dest);

                } else {
                    completedCount++;
                }

                continue;
            }

            var files = BuildFiles(scriptBundle, scriptBundles); //get list of files for this bundle
            var gulpTask = gulp.src(files, { base: "." });

            if (newerOnly) {
                gulpTask = gulpTask.pipe(newer(dest));
            }

            (function (name, files) {
                gulpTask
                    .on("end", function () {
                        console.log("Bundling " + name + " ... ");
                        for (var i = 0; i < files.length; i++) {
                            console.log(" - Includes file " + files[i] + ".");
                        }
                    })
                    .pipe(concat(dest))
                    .pipe(uglify())
                    .pipe(gulp.dest("."))
                    .on("end", function () {
                        completedCount++

                        if (completedCount == totalBundleCount) {
                            console.log("*** Bundling JS process complete. ***");
                        }
                    });
            })(scriptBundle.name, files);
        }
    } else {
        console.log("No JS bundles found.");
    }
}

function Bundle() {

}

gulp.task("bundle-js", function () {

    var bundlingArguments = {
        // flag to only updated bundle files if the script file is newer
        // defaulted to "true" for quicker local builds
        // process.argv.newerOnly is an optional parameter to set this value - intened as part of CI/build process
        NewerOnly: new BoolArgument(process.argv, "--newerOnly", true)
    };

    BundleJS(bundlingArguments.NewerOnly.Value);
});

gulp.task("bundle-js-full", ["clean-js"], function () {
    BundleJS(false);
});



gulp.task("bundle-css", function () {

    if (styleBundles && styleBundles.length) {
        for (var i = 0; i < styleBundles.length; i++) {
            var styleBundle = styleBundles[i];
            if (ToBool(styleBundle.referenceOnly)) {
                console.log("Bundle for " + styleBundle.name + " is set to only be referenced. No bundling for this bundle.")
                continue;
            }

            console.log("Bundling " + styleBundle.name + " ... ");

            var files = BuildFiles(styleBundle, styleBundles); //get list of files for this bundle

            var dest = settings.paths.stylesDestDirectory;
            dest += styleBundle.subpath || "";

            if (dest.slice(-1) != "/") { dest += "/"; }

            dest += styleBundle.filename || (styleBundle.name + ".min.css");

            gulp.src(files, { base: "." })
                .pipe(concat(dest))
                .pipe(cssmin())
                .pipe(gulp.dest("."));
        }
    }
    else {
        console.log("No CSS bundles found.");
    }

    console.log("Bundling CSS process complete.");
});