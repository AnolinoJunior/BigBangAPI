import { useMemo, useState } from 'react'
import AuthContext from './auth-context'
import { login as loginRequest } from '../services/authService'

const storageKeys = {
  accessToken: 'bigbangapi.accessToken',
  refreshToken: 'bigbangapi.refreshToken',
  expiration: 'bigbangapi.expiration',
  user: 'bigbangapi.user',
}

const nameClaimTypes = [
  'unique_name',
  'name',
  'userName',
  'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name',
]

const roleClaimTypes = [
  'role',
  'roles',
  'http://schemas.microsoft.com/ws/2008/06/identity/claims/role',
]

function decodeJwtPayload(token) {
  const payload = token.split('.')[1]

  if (!payload) {
    throw new Error('Token JWT inválido.')
  }

  const base64 = payload.replace(/-/g, '+').replace(/_/g, '/')
  const json = decodeURIComponent(
    atob(base64)
      .split('')
      .map((character) => `%${`00${character.charCodeAt(0).toString(16)}`.slice(-2)}`)
      .join(''),
  )

  return JSON.parse(json)
}

function getClaim(payload, claimTypes) {
  return claimTypes.find((claimType) => payload[claimType])
}

function getRoles(payload) {
  const roleClaimType = getClaim(payload, roleClaimTypes)
  const roles = roleClaimType ? payload[roleClaimType] : []

  return Array.isArray(roles) ? roles : roles ? [roles] : []
}

function createUserFromToken(accessToken) {
  const payload = decodeJwtPayload(accessToken)
  const nameClaimType = getClaim(payload, nameClaimTypes)

  return {
    userName: nameClaimType ? payload[nameClaimType] : '',
    roles: getRoles(payload),
  }
}

function isTokenValid(accessToken, expiration) {
  const expirationTime = new Date(expiration).getTime()

  if (!Number.isFinite(expirationTime) || expirationTime <= Date.now()) {
    return false
  }

  try {
    const { exp } = decodeJwtPayload(accessToken)
    return typeof exp === 'number' && exp * 1000 > Date.now()
  } catch {
    return false
  }
}

function clearStoredSession() {
  Object.values(storageKeys).forEach((key) => sessionStorage.removeItem(key))
}

function getStoredSession() {
  const accessToken = sessionStorage.getItem(storageKeys.accessToken)
  const refreshToken = sessionStorage.getItem(storageKeys.refreshToken)
  const expiration = sessionStorage.getItem(storageKeys.expiration)
  const storedUser = sessionStorage.getItem(storageKeys.user)

  if (!accessToken || !refreshToken || !expiration || !isTokenValid(accessToken, expiration)) {
    clearStoredSession()
    return null
  }

  try {
    return {
      accessToken,
      refreshToken,
      expiration,
      user: storedUser ? JSON.parse(storedUser) : createUserFromToken(accessToken),
    }
  } catch {
    clearStoredSession()
    return null
  }
}

function storeSession(session) {
  sessionStorage.setItem(storageKeys.accessToken, session.accessToken)
  sessionStorage.setItem(storageKeys.refreshToken, session.refreshToken)
  sessionStorage.setItem(storageKeys.expiration, session.expiration)
  sessionStorage.setItem(storageKeys.user, JSON.stringify(session.user))
}

export function AuthProvider({ children }) {
  const [session, setSession] = useState(getStoredSession)

  async function login(credentials) {
    const tokenData = await loginRequest(credentials)
    const user = createUserFromToken(tokenData.accessToken)
    const newSession = {
      accessToken: tokenData.accessToken,
      refreshToken: tokenData.refreshToken,
      expiration: tokenData.expiration,
      user,
    }

    if (!isTokenValid(newSession.accessToken, newSession.expiration)) {
      throw new Error('O token de acesso retornado pela API já expirou.')
    }

    storeSession(newSession)
    setSession(newSession)
  }

  function logout() {
    clearStoredSession()
    setSession(null)
  }

  const value = useMemo(() => {
    const roles = session?.user.roles ?? []

    return {
      login,
      logout,
      isAuthenticated: Boolean(session),
      isAdmin: roles.some((role) => role.toLowerCase() === 'admin'),
      user: session?.user ?? null,
    }
  }, [session])

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>
}
