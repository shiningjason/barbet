import { useEffect, useRef, useState } from 'react'

const scriptPromises = {}

export default function useLoadScript({ src, onLoadSuccess, onLoadError }) {
  const callbackRef = useRef({})
  Object.assign(callbackRef.current, { onLoadSuccess, onLoadError })

  const [loadSuccess, setLoadSuccess] = useState(false)

  useEffect(() => {
    if (!scriptPromises[src]) {
      let onResolve, onReject
      scriptPromises[src] = new Promise((resolve, reject) => {
        onResolve = resolve
        onReject = reject
      })

      const $script = document.createElement('script')
      $script.src = src
      $script.async = true
      $script.onload = onResolve
      $script.onerror = onReject

      document.body.appendChild($script)
    }

    let mount = true
    scriptPromises[src].then(
      () => {
        if (!mount) return
        setLoadSuccess(true)
        callbackRef.current.onLoadSuccess?.()
      },
      () => {
        if (!mount) return
        setLoadSuccess(false)
        callbackRef.current.onLoadError?.()
      }
    )
    return () => (mount = false)
  }, [src])

  return loadSuccess
}
