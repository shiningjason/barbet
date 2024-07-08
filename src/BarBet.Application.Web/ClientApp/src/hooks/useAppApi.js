import { useLatest, useRequest } from 'ahooks'
import { useRef } from 'react'
import appApi from '@app/utils/appApi'
import errorEmitter from '@app/utils/errorEmitter'

export default function useAppApi(name, options) {
  const api = appApi[name]
  if (api == null) throw new Error(`Invalid Web API: ${name}`)

  const onErrorRef = useLatest(options?.onError)
  const handleErrorRef = useRef()
  handleErrorRef ??= error => {
    errorEmitter.emit(error)
    onErrorRef.current?.(error)
  }

  const abortControllerRef = useRef()
  const onCancelRef = useLatest(options?.onCancel)
  const handleCancelRef = useRef()
  handleCancelRef.current ??= () => {
    abortControllerRef.current?.abort()
    onCancelRef.current?.()
  }

  return useRequest(
    params => {
      abortControllerRef.current = new AbortController()
      return api({
        data: params,
        signal: abortControllerRef.current.signal
      })
    },
    {
      manual: true,
      onCancel: handleCancelRef.current,
      ...options
    }
  )
}

export function useConfigsApi() {
  const name = 'getConfigs'
  return useAppApi(name, {
    manual: false,
    cacheKey: name,
    staleTime: Infinity
  })
}
