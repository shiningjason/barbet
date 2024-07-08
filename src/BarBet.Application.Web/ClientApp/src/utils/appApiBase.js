import axios from 'axios'
import AppError from './AppError'

const client = axios.create({
  baseURL: '/api',
  headers: { 'Content-Type': 'application/json' },
  transformRequest: data => (data != null ? JSON.stringify(data) : undefined)
})

const handleResponse = ({ data: response }) => {
  const isObjectResponse =
    typeof response === 'object' && response != null && !Array.isArray(response)
  if (!isObjectResponse) throw new AppError('WAPP0104')
  if (!Object.keys(response).length) throw new AppError('WAPP0103')
  if (!response.code) throw new AppError('WAPP0104')

  const { code, msg, data } = response
  if (code !== 'COMM0000') throw new AppError(code, msg, data)
  return data
}

const handleError = error => {
  if (error instanceof AppError) throw error
  if (error.response)
    throw new AppError('WAPP0102', null, { httpStatus: error.response.status })
  if (error.request) throw new AppError('WAPP0101')
  throw new AppError('WAPP9999', null, { innerError: error })
}

const createRequest = (url, method) => config =>
  client.request({ ...config, url, method }).then(handleResponse, handleError)
const createGetRequest = url => createRequest(url, 'get')
const createPostRequest = url => createRequest(url, 'post')

export { createGetRequest, createPostRequest }
