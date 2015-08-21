/// <binding Clean='clean' />

/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require("gulp"),
    mainBowerFiles = require('main-bower-files'),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify");
//project = require("./project.json");

//var paths = {
//    webroot: "./" + project.webroot + "/"
//};

//paths.js = paths.webroot + "js/**/*.js";
//paths.minJs = paths.webroot + "js/**/*.min.js";
//paths.css = paths.webroot + "css/**/*.css";
//paths.minCss = paths.webroot + "css/**/*.min.css";
//paths.concatJsDest = paths.webroot + "js/site.min.js";
//paths.concatCssDest = paths.webroot + "css/site.min.css";

//gulp.task("clean:js", function (cb) {
//    rimraf(paths.concatJsDest, cb);
//});

//gulp.task("clean:css", function (cb) {
//    rimraf(paths.concatCssDest, cb);
//});

//gulp.task("clean", ["clean:js", "clean:css"]);

//gulp.task("min:js", function () {
//    gulp.src([paths.js, "!" + paths.minJs], { base: "." })
//        .pipe(concat(paths.concatJsDest))
//        .pipe(uglify())
//        .pipe(gulp.dest("."));
//});

//gulp.task("min:css", function () {
//    gulp.src([paths.css, "!" + paths.minCss])
//        .pipe(concat(paths.concatCssDest))
//        .pipe(cssmin())
//        .pipe(gulp.dest("."));
//});

//gulp.task("min", ["min:js", "min:css"]);

var srcDir = 'client_packages/';

// Concatenate JS Files
//gulp.task('packScriptsManual', function () {
//    // return gulp.src('client_packages/**/*.js')
//    return gulp.src([srcDir + 'angular-google-chart/ng-google-chart.js',
//        srcDir + 'angular-gridster/dist/angular-gridster.min.js',
//        //srcDir + 'CodeMirror/**/*.js',
//        srcDir + 'ng-context-menu/dist/ng-context-menu.min.js',
//        srcDir + 'ng-focus-on/ng-focus-on.min.js',
//        srcDir + 'ng-tags-input/ng-tags-input.js',
//        srcDir + 'ngstorage/ngStorage.min.js',
//        srcDir + 'textAngular/dist/textAngular-rangy.min.js',
//        srcDir + 'textAngular/dist/textAngular-sanitize.min.js',
//        srcDir + 'textAngular/dist/textAngular.min.js'
//    ])
//      .pipe(concat('allPackagesManual.js'))
//      .pipe(uglify())
//      .pipe(gulp.dest('Scripts'));
//});

gulp.task('packScriptsAuto', function () {
    return gulp.src(mainBowerFiles({
        // Set the base path for your bower components
        // base: './bower_components',

        // Only return the JavaScript files
        filter: /.*\.js$/i
    }))
      .pipe(concat('allPackages.js'))
      .pipe(uglify())
      .pipe(gulp.dest('Scripts'));
});

gulp.task('watch', function () {
    // All files in bower_components
    gulp.watch('client_packages/*.js', ['packScriptsAuto']);
});

// Default Task
gulp.task('default', ['packScriptsAuto']);