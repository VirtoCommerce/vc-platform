var GulpSelenium = require('gulp-selenium');
var gulpSelenium = GulpSelenium(),
    outputDir = process.env.CIRCLE_TEST_REPORTS || 'test-result';
var config = {
  capabilities: {
    browserName: 'chrome'
  },
  onPrepare: function() {
    require('jasmine-reporters');
    jasmine.getEnv().addReporter(
      new jasmine.JUnitXmlReporter(outputDir, true, true)
    );
  },
  specs: ['test/ui/**/*.spec.js']
};

if (process.env.CI) {
  config.seleniumAddress = 'http://localhost:4444/wd/hub';
} else {
  config.seleniumServerJar = gulpSelenium.path;
  config.chromeDriver = gulpSelenium.chromeDriverPath;
}

exports.config = config;