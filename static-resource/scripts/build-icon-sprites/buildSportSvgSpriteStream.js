import buildSvgSpriteStream from './buildSvgSpriteStream.js'
import { DIST_DIR, SRC_SPORT_SVG_GLOB } from './path.js'

export default () =>
  buildSvgSpriteStream({
    srcGlob: SRC_SPORT_SVG_GLOB,
    destDir: DIST_DIR,
    destFilename: 'sport.svg'
  })
