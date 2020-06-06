const core = require('@actions/core');
const github = require('@actions/github');
const fs = require('fs');

String.isNullOrEmpty = function(value) {
    return !(typeof value === "string" && value.length > 0);
}

fs.readFile('Directory.Build.Props', function (err, doc) {
    if (!err) {
        let prefixes = doc.SelectNodes('Project/PropertyGroup/VersionPrefix');
        if (prefixes.length === 0) {
        core.setFailed("Version prefix cannot be read");
        return;
        }

        let suffixes = doc.SelectNodes('Project/PropertyGroup/VersionSuffix');

        let version = prefixes[0].innerText + 
            (suffixes.length > 0 && suffixes[0].innerText.trim() != '' ? '-' + suffixes[0].innerText.trim() : '') + 
            github.sha.substring(0, 8)

        core.setOutput("tag", version);

        console.log(`Version tag is: ${version}`);
    }
});