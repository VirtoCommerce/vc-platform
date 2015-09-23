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
gulp.task('packScripts', function () {
    return gulp.src(mainBowerFiles({
        // Only return the JavaScript files
        filter: /.*\.js$/i
    }))
      .pipe(concat('allPackages.js'))
      .pipe(uglify())
      .pipe(gulp.dest('Scripts'));
});

gulp.task('packCss', function () {
    return gulp.src(mainBowerFiles({
        // Only return the CSS files
        filter: /.*\.css$/i
    }))
      .pipe(concat('allStyles.css'))
      // .pipe(cssmin())
      .pipe(gulp.dest('Content'));
});

gulp.task('packFonts', function () {
    return gulp.src(mainBowerFiles({
        // Only return the font files
        filter: /.*\.(eot|svg|ttf|woff)$/i
    }))
      .pipe(gulp.dest('Content'));
});

gulp.task('packAll', ['packScripts', 'packCss', 'packFonts']);
