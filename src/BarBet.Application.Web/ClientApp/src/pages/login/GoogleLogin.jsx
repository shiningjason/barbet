import useAppApi, { useConfigsApi } from '@app/hooks/useAppApi'
import useGsiScript from '@app/hooks/useGsiScript'

export default function GoogleLogin() {
  useGsiScript()
  const { data: configs } = useConfigsApi()
  const request = useAppApi('googleAuthenticate')

  window.googleAuthenticate = request.runAsync
  window.handleGsiCallback ??= payload => {
    console.log(payload)
    window.googleAuthenticate(payload)
  }

  return (
    <>
      <div
        id="g_id_onload"
        data-client_id={configs.googleAuthClientId}
        data-context="signin"
        data-ux_mode="popup"
        data-callback="handleGsiCallback"
        data-auto_prompt="false"
      />
      <div
        className="g_id_signin"
        data-type="icon"
        data-shape="square"
        data-theme="outline"
        data-text="signin_with"
        data-size="large"
      />
    </>
  )
}
