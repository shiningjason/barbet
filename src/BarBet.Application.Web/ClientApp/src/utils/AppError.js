import AppStatus from './AppStatus'

export default class AppError extends Error {
  constructor(code, msg, data) {
    if (!/^[A-Z]{4}[0-9]{4}$/.test(code)) {
      const originalCode = code
      code = 'WAPP9999'
      msg = `${AppStatus[code]} (w/ unexpected code: ${originalCode}, message: ${msg})`
    }

    msg ||= AppStatus[code] ?? code
    msg = msg.replace(new RegExp(`^${code}\\s*:\\s*`), '').trim()

    super(msg)
    this.code = code
    this.msg = msg
    this.data = data
  }
}
