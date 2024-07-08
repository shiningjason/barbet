import { makeObservable } from 'mobx'

function makeBinding(self) {
  const keys = new Set()

  let proto = Reflect.getPrototypeOf(self)
  while (proto !== Object.prototype) {
    for (const key of Reflect.ownKeys(proto)) keys.add(key)
    proto = Reflect.getPrototypeOf(proto)
  }

  for (const key of keys) {
    if (key !== 'constructor' && typeof self[key] === 'function')
      self[key] = self[key].bind(self)
  }
}

export default function store(Class) {
  return class extends Class {
    constructor() {
      super()
      makeObservable(this)
      makeBinding(this)
    }
  }
}
