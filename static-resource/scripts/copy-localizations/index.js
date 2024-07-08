import 'dotenv/config'
import os from 'node:os'
import path from 'node:path'
import select from '@inquirer/select'
import chalk from 'chalk'
import { glob } from 'glob'
import gulp from 'gulp'
import unzip from 'gulp-unzip'

const L10N_DOWNLOAD_DIR = process.env.localization_download_dir || '~/Downloads'
const L10N_SRC_DIR = L10N_DOWNLOAD_DIR.replace(/^~/, os.homedir()).replace(
  /\/$/,
  ''
)
const L10N_SRC_GLOB = path.join(L10N_SRC_DIR, 'localization_*.zip')

const l10nSources = await glob(L10N_SRC_GLOB)
if (!l10nSources.length) {
  const message = 'No localization zip files were found in download directory.'
  console.log(chalk.bold.red(message))
  process.exit(1)
}

const selectedL10nSource = await select({
  message: '選擇要套用的翻譯包',
  choices: l10nSources.map(value => ({ value }))
})

gulp
  .src(selectedL10nSource.replace(new RegExp(`^${L10N_SRC_DIR}/`), ''), {
    cwd: L10N_SRC_DIR,
    encoding: false
  })
  .pipe(unzip())
  .pipe(gulp.dest('tmp', { encoding: false }))
