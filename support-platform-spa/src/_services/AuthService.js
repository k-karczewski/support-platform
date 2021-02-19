import { clearUserDataAction, saveUserDataAction } from '../_redux/actions/AuthActions';
import store from '../_redux/stores/ApplicationStore';
import HttpService from './HttpService';
import { isExpired, decodeToken } from 'react-jwt';
import { apiUrl } from '../_environments/environment';

export default class AuthService {
  constructor() {
    this.token = localStorage.getItem('token');
    this.http = new HttpService();
  }

  login = async (username, password) => {
    const userToLoginDto = {
      username,
      password
    }

    const result = {}

    await this.http.sendRequest(`${apiUrl}/auth/login`, 'POST', userToLoginDto)
      .then(response => {
        if(response.ok) {
          result.succeeded = true;
        } else {
          result.succeeded = false;
        }
        return response.json();
      }).then(data => {
        if(result.succeeded) {
          localStorage.setItem('token', data.token);
          localStorage.setItem('username', data.username);
          const decodedToken = decodeToken(data.token);
          store.dispatch(saveUserDataAction(data.username, decodedToken));
        } else {
          result.errors = data;
        }
      });
      return result;
  }

  register = async (username, email, password, confirmPassword ) => {
    const userToRegisterDto = {
      username,
      email,
      password,
      confirmPassword
    };

    const result = {}

    await this.http.sendRequest(`${apiUrl}/auth/register`, 'POST', userToRegisterDto)
      .then(response => {
        if (response.ok) {
          result.succeeded = true;
        } else {
          return response.json();
        }})
      .then(data => {
        if(data) {
          result.succeeded = false;
          result.errors = data;       
        }
      });

      return result;
    }

    isUserLoggedIn = () => {
      return this.token && !isExpired(this.token) ? true : false;
    }

    initializeStore = () => {
      const token = localStorage.getItem('token');
      const username = localStorage.getItem('username');

      if(username && token) {
        store.dispatch(saveUserDataAction(username, decodeToken(token)));
        return true;
      } else {
        localStorage.removeItem('username');
        localStorage.removeItem('token');
        return false;
      }
    }

    logout = () => {
      store.dispatch(clearUserDataAction());
      localStorage.removeItem('username');
      localStorage.removeItem('token');
    }

    getDecodedToken = () => {
      return store.getState().authStates.decodedToken;
    }
  }