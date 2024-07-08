import { createBrowserRouter, RouterProvider } from 'react-router-dom'
import { useConfigsApi } from '@app/hooks/useAppApi'
import routes from './pages/routes'

const router = createBrowserRouter(routes)

export default function App() {
  const { data: configs } = useConfigsApi()
  if (!configs) return null

  return <RouterProvider router={router} />
}
