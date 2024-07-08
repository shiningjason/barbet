import useLoadScript from './useLoadScript'

export default function useGsiScript(props) {
  return useLoadScript({
    ...props,
    src: 'https://accounts.google.com/gsi/client'
  })
}
