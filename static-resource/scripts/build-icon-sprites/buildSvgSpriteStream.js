import gulp from 'gulp'
import svgSprite from 'gulp-svg-sprite'

export default ({ srcGlob, destDir, destFilename }) =>
  gulp
    .src(srcGlob)
    .pipe(
      svgSprite({
        log: 'info',
        mode: { symbol: { dest: '', sprite: destFilename } }
      })
    )
    .pipe(gulp.dest(destDir))
