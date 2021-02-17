import React, { useState } from 'react';
import { useHistory } from 'react-router-dom';
import RegisterErrors from './RegisterErrors/RegisterErrors';

import './Register.sass';

const Register = () => {
  const [username, setUsername] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [formErrors, setFormErrors] = useState([]);
  const history = useHistory();

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

  const handleSubmit = (event) => {
    event.preventDefault();

    if (username && email && password && confirmPassword && (password === confirmPassword)) {
      const userToRegisterDto = {
        username,
        email,
        password,
        confirmPassword
      };

      fetch('https://localhost:44339/api/auth/register', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(userToRegisterDto),
      })
        .then(response => {
          debugger
          console.log(response)
          if (!response.ok) {
            console.log(response);
            throw response.json();
          }
          history.push('/accountCreated');
        })
        .catch((error) => {
          error.then(errors => setFormErrors(errors));
        });
    }
  }

  return (
    <main className="register">
      <div className="container">
        <header className="form__header">
          <h2 className="form__heading">Zarejestruj się</h2>
          <p className="form__text">Wpisz swoje dane aby się zarejestrować!</p>
        </header>
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

      {formErrors.length > 0 && <RegisterErrors errors={formErrors} />}
    </main>

  );
}

export default Register;