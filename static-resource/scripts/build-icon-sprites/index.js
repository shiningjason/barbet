import { finished } from 'node:stream/promises'
import { deleteAsync } from 'del'
import buildCurrencySvgSpriteStream from './buildCurrencySvgSpriteStream.js'
import buildSportPngSpriteStream from './buildSportPngSpriteStream.js'
import buildSportSvgSpriteStream from './buildSportSvgSpriteStream.js'
import { DIST_DIR } from './path.js'

await deleteAsync(DIST_DIR, { force: true })
await Promise.all([
  finished(buildCurrencySvgSpriteStream()),
  finished(buildSportPngSpriteStream()),
  finished(buildSportSvgSpriteStream())
])
