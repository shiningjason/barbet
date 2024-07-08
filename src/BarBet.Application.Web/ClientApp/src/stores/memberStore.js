import { action, computed, observable } from 'mobx'
import store from '@app/stores/helpers/store'

@store
class MemberStore {
  @observable.ref
  accessor member = undefined

  @computed
  get loggedIn() {
    return this.member != null
  }

  @action
  login(member) {
    this.member = member
  }

  @action
  logout() {
    this.member = undefined
  }
}

export default new MemberStore()
