/**
 * Para executar utilize o seguinte comando
 * yarn gulp js
 * yarn gulp css
 * yarn gulp watch-css
 * obs: "js" é o nome da tarefa configurada em "gulpfile.js"
 */
//Iniciando os modulos
var gulp = require('gulp');
var concat = require('gulp-concat');
var cssmin = require('gulp-cssmin');
var uncss = require('gulp-uncss');
var browserSync = require('browser-sync').create();

gulp.task('browser-sync', function () {
    browserSync.init({
        proxy: 'localhost:5001'
    });
    gulp.watch('./Styles/**/*.css', ['css']);//css é a tarefa 'css'
    gulp.watch('./js/**/*.js', ['js']);//js é a tarefa 'js'
});

//Nova tarefa
gulp.task('js', function () {
    return gulp.src([
        './node_modules/bootstrap/dist/js/bootstrap.min.js',
        './node_modules/jquery/dist/jquery.min.js',
        './node_modules/jquery-validation/dist/jquery.validate.min.js',
        './node_modules/jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.min.js',
        './js/**/*.js',
    ])
        .pipe(gulp.dest('wwwroot/js/'))//Onde será salvo a saida
        .pipe(browserSync.stream());//Atualiza a tela do navegador
});

//Nova tarefa
gulp.task('css', function () {
    return gulp.src([
        './Styles/site.css',
        './node_modules/bootstrap/dist/css/bootstrap.css'
    ])
        .pipe(concat('site.min.css'))//Concatena o codigo
        .pipe(cssmin())//Minifica o codigo
        .pipe(uncss({ html: ['Views/**/*.cshtml'] }))//Remove as partes do css que nao estao usadas
        .pipe(gulp.dest('wwwroot/css/'))//Onde será salvo a saida
        .pipe(browserSync.stream());//Atualiza a tela do navegador
});

//Monitora um determinado path e qualquer alteração no mesmo e executado o gulp
gulp.task('watch-css', function () {
    gulp.watch('./Styles/**/*.css', ['css']);//css é a tarefa 'css'
});