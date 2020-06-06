const core = require('@actions/core');
const github = require('@actions/github');
const fs = require('fs');
const xml2js = require('xml2js');

String.isNullOrEmpty = function(value) {
    return !(typeof value === "string" && value.length > 0);
}

fs.readFile('Directory.Build.Props', function (err, data) {
    if (!err) {
        var json = JSON.parse(xml2js.toJson(data, {reversible: false}));

        var prefix = json["Project"]["PropertyGroup"]["VersionPrefix"].trim();
        var suffix = json["Project"]["PropertyGroup"]["VersionSuffix"].trim();

        let version = prefix + (suffix != '' ? '-' + suffixes[0].innerText : '') + github.sha.substring(0, 8)

        core.setOutput("tag", version);

        console.log(`Version tag is: ${version}`);
    }
});