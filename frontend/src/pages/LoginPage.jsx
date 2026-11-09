import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import useAuth from '../hooks/useAuth'

function LoginPage() {
  const navigate = useNavigate()
  const { login } = useAuth()
  const [userName, setUserName] = useState('')
  const [password, setPassword] = useState('')
  const [errorMessage, setErrorMessage] = useState('')
  const [isSubmitting, setIsSubmitting] = useState(false)

  async function handleSubmit(event) {
    event.preventDefault()
    setErrorMessage('')
    setIsSubmitting(true)

    try {
      await login({ userName, password })
      navigate('/admin', { replace: true })
    } catch (error) {
      const apiMessage = error.response?.data?.message ?? error.response?.data?.Message

      setErrorMessage(apiMessage ?? 'Não foi possível entrar. Verifique suas credenciais e tente novamente.')
    } finally {
      setIsSubmitting(false)
    }
  }

  return (
    <main className="container py-5">
      <div className="row justify-content-center">
        <div className="col-12 col-sm-10 col-md-7 col-lg-5">
          <section className="card shadow-sm" aria-labelledby="login-title">
            <div className="card-body p-4 p-md-5">
              <h1 className="h3 mb-4" id="login-title">Entrar</h1>

              {errorMessage && (
                <div className="alert alert-danger" role="alert">
                  {errorMessage}
                </div>
              )}

              <form onSubmit={handleSubmit}>
                <div className="mb-3">
                  <label className="form-label" htmlFor="userName">Usuário</label>
                  <input
                    autoComplete="username"
                    className="form-control"
                    disabled={isSubmitting}
                    id="userName"
                    onChange={(event) => setUserName(event.target.value)}
                    required
                    type="text"
                    value={userName}
                  />
                </div>

                <div className="mb-4">
                  <label className="form-label" htmlFor="password">Senha</label>
                  <input
                    autoComplete="current-password"
                    className="form-control"
                    disabled={isSubmitting}
                    id="password"
                    onChange={(event) => setPassword(event.target.value)}
                    required
                    type="password"
                    value={password}
                  />
                </div>

                <button className="btn btn-primary w-100" disabled={isSubmitting} type="submit">
                  {isSubmitting ? 'Entrando...' : 'Entrar'}
                </button>
              </form>
            </div>
          </section>
        </div>
      </div>
    </main>
  )
}

export default LoginPage
