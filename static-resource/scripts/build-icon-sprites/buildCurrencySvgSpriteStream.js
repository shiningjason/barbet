import buildSvgSpriteStream from './buildSvgSpriteStream.js'
import { DIST_DIR, SRC_CURRENCY_SVG_GLOB } from './path.js'

export default () =>
  buildSvgSpriteStream({
    srcGlob: SRC_CURRENCY_SVG_GLOB,
    destDir: DIST_DIR,
    destFilename: 'currency.svg'
  })
