import { createGetRequest, createPostRequest } from './appApiBase'

export default {
  getConfigs: createGetRequest('/configs'),
  googleAuthenticate: createPostRequest('/auth/google')
}
