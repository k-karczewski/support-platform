import React, { useState } from 'react';
import { Link, useHistory } from 'react-router-dom';
import AuthService from '../../../_services/AuthService';

import FormHeader from '../../../components/shared/forms/header/FormHeader';
import FormTextInput from '../../../components/shared/forms/text input/FormTextInput';
import FormSubmitButton from '../../../components/shared/forms/submit button/FormSubmitButton';

import './LoginForm.sass';
import FormErrorsPanel from '../../../components/shared/forms/errors panel/FormErrorsPanel';

const LoginForm = () => {
  const history = useHistory();
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [formErrors, setFormErrors] = useState([]);

  const headerText = {
    heading: 'Zaloguj się',
    description: 'Wpisz swoje dane aby się zalogować!'
  }

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
        // push to view for logged in user
        history.push('/');
      } else {
        // show error message
        setFormErrors(result.errors);
      }
    }
  }

  return (
    <main className="login">
      <div className="container">
        <FormHeader heading={headerText.heading} description={headerText.description} />
        <form className="login__form" method="post" onSubmit={handleSubmit}>
          <FormTextInput labelText="Nazwa użytkownika" htmlFor="username" type="text" onChangeHandler={handleInputsValueChange} />
          <FormTextInput labelText="Hasło" htmlFor="password" type="password" onChangeHandler={handleInputsValueChange} />
          <FormSubmitButton text="Zaloguj się" />
          <Link to="/register" className="form__registerLink">Nie masz jeszcze konta? Zarejestruj się!</Link>
        </form>
      </div>

      {formErrors.length > 0 && <FormErrorsPanel errors={formErrors} />}
    </main>
  );
}

export default LoginForm;