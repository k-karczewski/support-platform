import React, { useState } from 'react';
import { useHistory, Prompt } from 'react-router-dom';

import AuthService from '../../../_services/AuthService';

import FormErrorsPanel from '../../../components/shared/forms/errors panel/FormErrorsPanel';
import FormHeader from '../../../components/shared/forms/header/FormHeader';
import FormTextInput from '../../../components/shared/forms/text input/FormTextInput';
import FormSubmitButton from '../../../components/shared/forms/submit button/FormSubmitButton';

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
        setUsername('');
        setEmail('');
        setPassword('');
        setConfirmPassword('');
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
          <FormTextInput labelText="Nazwa użytkownika" htmlFor="username" type="text" onChangeHandler={handleInputsValueChange} />
          <FormTextInput labelText="Email" htmlFor="email" type="text" onChangeHandler={handleInputsValueChange} />
          <FormTextInput labelText="Hasło" htmlFor="password" type="password" onChangeHandler={handleInputsValueChange} />
          <FormTextInput labelText="Potwierdź hasło" htmlFor="confirmPassword" type="password" onChangeHandler={handleInputsValueChange} />
          <FormSubmitButton text="Zarejestruj się" />
        </form>
        <Prompt
          when={username.length > 0 || password.length > 0 || email.length > 0 || confirmPassword.length > 0}
          message="Masz niezapisane zmiany. 
                Czy na pewno chcesz opuścić tę stronę?"
        />
      </div>

      {formErrors.length > 0 && <FormErrorsPanel errors={formErrors} />}
    </main>
  );
}

export default Register;