import path from 'node:path'
import { PROJECT_DIR } from '../constants.js'

export const SRC_CURRENCY_SVG_GLOB = path.join(
  PROJECT_DIR,
  'src/icon/currency/*.svg'
)
export const SRC_SPORT_SVG_GLOB = path.join(PROJECT_DIR, 'src/icon/sport/*.svg')
export const SRC_SPORT_PNG_GLOB = path.join(PROJECT_DIR, 'src/icon/sport/*.png')
export const DIST_DIR = path.join(PROJECT_DIR, 'dist/sprite')
