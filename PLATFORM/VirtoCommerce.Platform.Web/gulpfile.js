/// <binding />

/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require("gulp"),
    mainBowerFiles = require('main-bower-files'),
    concat = require("gulp-concat"),
    //cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify");

// Concatenate JS Files
gulp.task('packJavaScript', function () {
    return gulp.src(mainBowerFiles({
        // Only the JavaScript files
        filter: /.*\.js$/i
    }))
      .pipe(concat('allPackages.js'))
      .pipe(uglify())
      .pipe(gulp.dest('Scripts'));
});

gulp.task('packCss', function () {
    return gulp.src(mainBowerFiles({
        // Only the CSS files
        filter: /.*\.css$/i
    }))
      .pipe(concat('allStyles.css'))
      // .pipe(cssmin())
      .pipe(gulp.dest('Content'));
});

// copy fonts for simple packages like angular-ui-grid.
gulp.task('copyMainFonts', function () {
    return gulp.src(mainBowerFiles({
        // Only return the font files
        filter: /.*\.(eot|svg|ttf|woff)$/i
    }))
      .pipe(gulp.dest('Content'));
});

// font-awesome package
gulp.task('fontawesomeCss', function () {
    return gulp.src('client_packages/font-awesome/css/font-awesome.css')
     .pipe(gulp.dest('Content/themes/main/css'));
});
gulp.task('fontawesomeFonts', function () {
    return gulp.src('client_packages/font-awesome/fonts/*.*')
     .pipe(gulp.dest('Content/themes/main/fonts'));
});
gulp.task('fontawesomePackage', ['fontawesomeCss', 'fontawesomeFonts']);


gulp.task('packAll', ['packJavaScript', 'packCss', 'copyMainFonts', 'fontawesomePackage']);
