import { autorun } from 'mobx'
import { useEffect, useState } from 'react'

export default function useSelector(selector, ...args) {
  const [state, setState] = useState(() => selector(...args))
  useEffect(() => autorun(() => setState(selector(...args))), args)
  return state
}
