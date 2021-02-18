import React, { useState } from 'react';
import { Link, useHistory } from 'react-router-dom';
import AuthService from '../../../_services/AuthService';
import LoginFormErrors from './login form errors/LoginFormErrors';

import './LoginForm.sass';

const LoginForm = () => {
  const history = useHistory();
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [formErrors, setFormErrors] = useState([]);

  const handleInputsValueChange = (event) => {
    switch (event.target.id) {
      case 'username':
        setUsername(event.target.value);
        break;
      case 'password':
        setPassword(event.target.value);
        break;
      default:
        break;
    }
  }

  const handleSubmit = async (event) => {
    event.preventDefault();

    const authService = new AuthService();

    if (username && password) {
      const result = await authService.login(username, password);
      if (result.succeeded) {
        history.push('/'); // push to view for logged in user
      } else {
        // show error message
        debugger
        setFormErrors(result.errors);
      }
    }
  }

  return (
    <main className="login">
      <div className="container">
        <header className="form__header">
          <h2 className="form__heading">Zaloguj się</h2>
          <p className="form__text">Wpisz swoje dane aby się zalogować!</p>
        </header>
        <form className="login__form" method="post" onSubmit={handleSubmit}>
          <label htmlFor="username">
            Nazwa użytkownika
          <input type="text" id="username" onChange={handleInputsValueChange} />
          </label>
          <label htmlFor="password">
            Hasło
          <input type="password" id="password" onChange={handleInputsValueChange} />
          </label>
          <button type="submit" className="form__submit">Zaloguj się</button>
          <Link to="/register" className="form__registerLink">Nie masz jeszcze konta? Zarejestruj się!</Link>
        </form>
      </div>

      {formErrors.length > 0 && <LoginFormErrors errors={formErrors} />}
    </main>
  );
}

export default LoginForm;