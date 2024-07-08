const EVENT_NAME = '$error'
const listeners = new Map()

function on(callback) {
  let listener = listeners.get(callback)

  if (listener == null) {
    listener = e => callback(e.detail)
    listener.off = () => off(callback)
    window.addEventListener(EVENT_NAME, listener)
    listeners.set(callback, listener)
  }

  return listener.off
}

function off(callback) {
  const listener = listeners.get(callback)
  if (listener == null) return

  window.removeEventListener(EVENT_NAME, listener)
  listeners.delete(callback)
}

function emit(error) {
  window.dispatchEvent(new CustomEvent(EVENT_NAME, { detail: error }))
}

export default { emit, off, on }
