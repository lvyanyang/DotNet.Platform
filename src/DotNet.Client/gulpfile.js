'use strict';

var gulp = require('gulp'),
    concat = require('gulp-concat'),
    cleancss = require('gulp-clean-css'),
    uglify = require('gulp-uglify'),
    sass = require('gulp-sass'),
    rename = require('gulp-rename');

    var fx_css_path = [
      'lib/fx/core.css'
    ];

    var fx_js_path = [
      'lib/fx/core.js',
      'lib/fx/action.js',
      'lib/fx/extend.js',
      'lib/fx/form.js',
      'lib/fx/modal.js',
      'lib/fx/table.js',
      'lib/fx/tree.js',
      'lib/fx/treegrid.js',
      'lib/fx/treetable.js',
      'lib/fx/plugin.js'
    ];

    var css_path = [
      'lib/animate/animate.css',
      'lib/font-awesome/font-awesome.css',
      'lib/simple-line-icons/simple-line-icons.css',
      'lib/jquery-easyui/jquery-easy-layout.css',
      'lib/jquery-easyui/jquery-easy-tree.css',
      'lib/jquery-easyui/jquery-easy-grid.css',
      'lib/bootstrap/bootstrap.css',
      'lib/bootstrap-fileinput/css/fileinput.css',
      'lib/bootstrap-switch/bootstrap-switch.css',
      'lib/jquery-minicolors/jquery-minicolors.css',
      'lib/bootstrap-touchspin/bootstrap-touchspin.css',
      'lib/bootstrap-timepicker/bootstrap-timepicker.css',
      'lib/bootstrap-datepicker/bootstrap-datepicker.css',
      'lib/bootstrap-datetimepicker/bootstrap-datetimepicker.css',
      'lib/jquery-sortable/css/kv-sortable.css',
      'lib/jquery-icheck/jquery-icheck.css',
      'lib/jquery-icheck/jquery-icheck-square.css',
      'lib/jquery-select2/jquery-select2.css',
      'lib/jquery-select2/jquery-select2-bootstrap.css',
      'lib/jquery-loadmask/jquery-loadmask.css',
      'lib/layer/layer.css'
    ];

    var js_path = [
      'lib/jquery/jquery.js',
      'lib/jquery-easyui/jquery-easy-parser.js',
      'lib/jquery-easyui/jquery-easy-layout.js',
      'lib/jquery-easyui/jquery-easy-tree.js',
      'lib/jquery-easyui/jquery-easy-grid.js',
      'lib/bootstrap/bootstrap.js',
      'lib/bootstrap-switch/bootstrap-switch.js',
      'lib/jquery-minicolors/jquery-minicolors.js',
      'lib/bootstrap-maxlength/bootstrap-maxlength.js',
      'lib/bootstrap-touchspin/bootstrap-spinner.js',
      'lib/bootstrap-touchspin/bootstrap-touchspin.js',
      'lib/bootstrap-timepicker/bootstrap-timepicker.js',
      'lib/bootstrap-datepicker/bootstrap-datepicker.js',
      'lib/bootstrap-datepicker/bootstrap-datepicker-zh.js',
      'lib/bootstrap-datetimepicker/bootstrap-datetimepicker.js',
      'lib/bootstrap-datetimepicker/bootstrap-datetimepicker-zh.js',
      'lib/bootstrap-fileinput/js/fileinput.js',
      'lib/bootstrap-fileinput/js/fileinput_locale_zh.js',
      'lib/bootstrap-fileinput/themes/fa/theme.js',
      'lib/jquery-sortable/js/html.sortable.js',
      'lib/jquery-icheck/jquery-icheck.js',
      'lib/jquery-select2/jquery-select2.js',
      'lib/jquery-select2/jquery-select2-zh.js',
      'lib/jquery-slimscroll/jquery-slimscroll.js',
      'lib/jquery-validation/jquery-validate.js',
      'lib/jquery-validation/jquery-validate-messages-zh.js',
      'lib/jquery-form/jquery-form.js',
      'lib/jquery-loadmask/jquery-loadmask.js',
      'lib/jquery-storageapi/jquery-storageapi.js',
      'lib/useragent/useragent_base.js',
      'lib/useragent/useragent.js',
      'lib/layer/layer.js'
    ];

gulp.task('css', function () {
    return gulp.src(css_path)
        .pipe(concat('lib.css'))
        .pipe(gulp.dest('./'))
        .pipe(cleancss())
        .pipe(rename('lib.min.css'))
        .pipe(gulp.dest('./'));
});

gulp.task('fx_css', function () {
    return gulp.src(fx_css_path)
        .pipe(concat('fx.css'))
        .pipe(gulp.dest('./'))
        .pipe(cleancss())
        .pipe(rename('fx.min.css'))
        .pipe(gulp.dest('./'));
});

gulp.task('js', function () {
    return gulp.src(js_path)
        .pipe(concat('lib.js'))
        .pipe(gulp.dest('./'))
        .pipe(uglify())
        .pipe(rename('lib.min.js'))
        .pipe(gulp.dest('./'));
});

gulp.task('fx_js', function () {
    return gulp.src(fx_js_path)
        .pipe(concat('fx.js'))
        .pipe(gulp.dest('./'))
        .pipe(uglify())
        .pipe(rename('fx.min.js'))
        .pipe(gulp.dest('./'));
});

gulp.task('min_lib_js', function() {
    gulp.src(['lib/**/*.js','!lib/**/*.min.js'])
        .pipe(uglify())
        .pipe(rename({ suffix: '.min' }))
        .pipe(gulp.dest('./lib'))
})

gulp.task('min_lib_css', function() {
    gulp.src(['lib/**/*.css','!lib/**/*.min.css'])
        .pipe(cleancss())
        .pipe(rename({ suffix: '.min' }))
        .pipe(gulp.dest('./lib'))
})
		

//gulp.task('sass', function () {
//	gulp.src('lib/metronic/sass/global/*.scss')
//    .pipe(sass())
//    .pipe(gulp.dest('lib/metronic'));
//});

gulp.task('default', ['fx_css','css','fx_js','js','min_lib_js','min_lib_css']);
