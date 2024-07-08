import gulp from 'gulp'
import clone from 'gulp-clone'
import csso from 'gulp-csso'
import imagemin from 'gulp-imagemin'
import webp from 'gulp-webp'
import spritesmith from 'gulp.spritesmith'
import mergeStream from 'merge-stream'
import vinylBuffer from 'vinyl-buffer'
import { DIST_DIR, SRC_SPORT_PNG_GLOB } from './path.js'

export default () => {
  const spriteStream = gulp.src(SRC_SPORT_PNG_GLOB, { encoding: false }).pipe(
    spritesmith({
      imgName: 'sport.png',
      imgPath: '~sport.png',
      cssName: 'sport.css'
    })
  )
  const imageSpriteStream = spriteStream.img.pipe(vinylBuffer())
  return mergeStream(
    imageSpriteStream.pipe(imagemin({ verbose: true })),
    imageSpriteStream.pipe(clone()).pipe(webp()),
    spriteStream.css.pipe(csso())
  ).pipe(gulp.dest(DIST_DIR, { encoding: false }))
}
