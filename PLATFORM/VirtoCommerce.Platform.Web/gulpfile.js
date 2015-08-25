/// <binding BeforeBuild='packCss' ProjectOpened='watch' />

/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require("gulp"),
    mainBowerFiles = require('main-bower-files'),
    // rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify");

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


// Concatenate JS Files
gulp.task('packScripts', function () {
    // Only return the JavaScript files
    return gulp.src(mainBowerFiles({
        // Only return the JavaScript files
        filter: /.*\.js$/i
    }))
      .pipe(concat('allPackages.js'))
      .pipe(uglify())
      .pipe(gulp.dest('Scripts'));
});

gulp.task('packCss', function () {
    // Only return the JavaScript files
    return gulp.src(mainBowerFiles({
        // Only return the JavaScript files
        filter: /.*\.css$/i
    }))
      .pipe(concat('allStyles.css'))
      .pipe(cssmin())
      .pipe(gulp.dest('Scripts'));
});

gulp.task('packAll', ['packScripts', 'packCss']);

gulp.task('watch', function () {
    // All files in client_packages
    gulp.watch('client_packages/**/*.js', ['packScripts']);
    gulp.watch('client_packages/**/*.css', ['packCss']);
});

// Default Task
gulp.task('default', ['packScripts']);