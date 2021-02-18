import { isExpired, decodeToken } from 'react-jwt';

export default class AuthService {
  constructor() {
    this.token = localStorage.getItem('token');
  }

  login = async (username, password) => {
    const userToLoginDto = {
      username,
      password
    }

    const result = {}
    await fetch('https://localhost:44339/api/auth/login', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(userToLoginDto),
    })
      .then(response => {
        if(response.ok) {
          result.succeeded = true;
        } else {
          result.succeeded = false;
        }
        return response.json();
      }).then(data => {
        debugger
        if(result.succeeded) {
          this.token = data.token;
          localStorage.setItem('token', data.token);
          localStorage.setItem('username', data.username);
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

    await fetch('https://localhost:44339/api/auth/register', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(userToRegisterDto),
    })
      .then(response => {
        debugger
        if (response.ok) {
          result.succeeded = true;
        } else {
          return response.json();
        }})
      .then(data => {
        result.succeeded = false;
        result.errors = data;       
      });

      return result;
    }

    isUserLoggedIn = () => {
      return this.token && !isExpired(this.token) ? true : false;
    }

    getDecodedToken = () => {
      return this.token ? decodeToken(this.token) : null;
    }
  }