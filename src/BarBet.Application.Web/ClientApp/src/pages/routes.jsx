import { createRoutesFromElements, Route } from 'react-router-dom'
import ErrorPage from './common/ErrorPage'
import RouteContainer from './common/RouteContainer'
import RouteErrorBoundary from './common/RouteErrorBoundary'

export default createRoutesFromElements(
  <>
    <Route
      path="/"
      Component={RouteContainer}
      ErrorBoundary={RouteErrorBoundary}
    >
      <Route
        path="/"
        lazy={async () => ({
          Component: (await import('@app/pages/index/IndexPage')).default
        })}
      />
      <Route
        path="/login"
        lazy={async () => ({
          Component: (await import('@app/pages/login/LoginPage')).default
        })}
      />
      <Route
        path="*"
        lazy={async () => ({
          Component: (await import('@app/pages/common/NotFoundPage')).default
        })}
      />
    </Route>
    <Route path="/error" Component={ErrorPage} />
  </>
)
