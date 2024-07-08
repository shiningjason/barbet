import { useNavigate } from 'react-router-dom'
import useSelector from '@app/stores/helpers/useSelector'
import memberStore from '@app/stores/memberStore'

export default function IndexPage() {
  const navigate = useNavigate()
  const loggedIn = useSelector(() => memberStore.loggedIn)
  return (
    <div>
      {loggedIn ? (
        <button onClick={() => alert('Hello, world!')}>登出</button>
      ) : (
        <button onClick={() => navigate('/login')}>登入</button>
      )}
    </div>
  )
}
