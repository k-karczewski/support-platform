import AuthService from "./AuthService"

export default class HttpService {
  sendRequest = async (url, method, data) => {
    const authService = new AuthService();
    const authorizationToken = authService.isUserLoggedIn() ? `Bearer ${authService.token}` : '';

    return fetch(url, {
      method: method,
      headers: {
        'Authorization': `${authorizationToken}`,
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(data),
    })
  }
}