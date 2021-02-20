import React, { useState } from 'react';
import { useHistory } from 'react-router-dom';
import AuthService from '../../../_services/AuthService';

import FormHeader from '../../../components/forms/form header/FormHeader';
import RegisterFormErrors from './register form errors/RegisterFormErrors';

import './RegisterForm.sass';

const Register = () => {
  const [username, setUsername] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [formErrors, setFormErrors] = useState([]);
  const history = useHistory();

  const headerText = {
    heading: 'Zarejestruj się',
    description: 'Wpisz swoje dane aby się zarejestrować!'
  }

  const handleInputsValueChange = (event) => {
    switch (event.target.id) {
      case 'username':
        setUsername(event.target.value);
        break;
      case 'email':
        setEmail(event.target.value);
        break;
      case 'password':
        setPassword(event.target.value);
        break;
      case 'confirmPassword':
        setConfirmPassword(event.target.value);
        break;
      default:
        break;
    }
  }

  const handleSubmit = async (event) => {
    event.preventDefault();
    if (username && email && password && confirmPassword && (password === confirmPassword)) {
      const authService = new AuthService();
      const result = await authService.register(username, email, password, confirmPassword);

      if (result.succeeded) {
        history.push('/accountCreated');
      } else {
        setFormErrors(result.errors);
      }
    }
  }

  return (
    <main className="register">
      <div className="container">
        <FormHeader heading={headerText.heading} description={headerText.description} />
        <form className="register__form" method="post" onSubmit={handleSubmit}>
          <label htmlFor="username">
            Nazwa użytkownika
            <input type="text" id="username" onChange={handleInputsValueChange} />
          </label>
          <label htmlFor="email">
            Email
            <input type="text" id="email" onChange={handleInputsValueChange} />
          </label>
          <label htmlFor="password">
            Hasło
            <input type="password" id="password" onChange={handleInputsValueChange} />
          </label>
          <label htmlFor="confirmPassword">
            Potwierdź hasło
            <input type="password" id="confirmPassword" onChange={handleInputsValueChange} />
          </label>
          <button type="submit" className="form__submit">Zarejestruj się</button>
        </form>
      </div>

      {formErrors.length > 0 && <RegisterFormErrors errors={formErrors} />}
    </main>
  );
}

export default Register;