/// <binding ProjectOpened='watch' />
/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require("gulp"),
    mainBowerFiles = require('main-bower-files'),
    concat = require("gulp-concat"),
    uglify = require("gulp-uglify"),
    autoprefixer = require('gulp-autoprefixer'),
    sass = require('gulp-sass'),
    sourcemaps = require('gulp-sourcemaps');

// minify all js files from bower packages to single file
gulp.task('packJavaScript', function () {
    return gulp.src(mainBowerFiles({
        // Only the JavaScript files
        filter: /.*\.js$/i
    }))
      .pipe(concat('allPackages.js'))
      .pipe(uglify())
      .pipe(gulp.dest('Scripts'));
});

// translate sass to css
gulp.task('translateSass', function () {
    return gulp.src(['Content/themes/main/sass/**/*.sass'])
        // must be executed straigh after source
        .pipe(sourcemaps.init())
        .pipe(sass({
            includePaths: require('node-bourbon').includePaths
        }))
        .pipe(autoprefixer({
            browsers: [
                'Explorer >= 10',
                'Edge >= 12',
                'Firefox >= 19',
                'Chrome >= 20',
                'Safari >= 8',
                'Opera >= 15',
                'iOS >= 8',
                'Android >= 4.4',
                'ExplorerMobile >= 10',
                'last 2 versions'
            ]
        }))
        // must be executed straight before output
        .pipe(sourcemaps.write('.', { includeContent: false, sourceRoot: '../sass' }))
        .pipe(gulp.dest('Content/themes/main/css'));
});

// concatenate all css files from bower packages to single file
gulp.task('packCss', function () {
    return gulp.src(mainBowerFiles({
        // Only the CSS files
        filter: /.*\.css$/i
    }))
      .pipe(concat('allStyles.css'))
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

gulp.task('packAll', ['packJavaScript', 'packCss', 'copyMainFonts']);

// Watch on sass to enable auto-translation
gulp.task('watch', function () {
    gulp.watch('Content/themes/main/sass/**/*.sass', ['translateSass']);
})