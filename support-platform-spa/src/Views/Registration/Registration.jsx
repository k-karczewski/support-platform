import React, { useState } from 'react';

const Registration = () => {
  const [username, setUsername] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');

  const handleUsernameInput = (event) => {
    setUsername(event.target.value);
  }

  const handleEmailInput = (event) => {
    setEmail(event.target.value);
  }

  const handlePasswordInput = (event) => {
    setPassword(event.target.value);
  }

  const handleConfirmPasswordInput = (event) => {
    setConfirmPassword(event.target.value);
  }

  const submitForm = (event) => {
    event.preventDefault();
    if (username && email && password && confirmPassword && (password === confirmPassword)) {
      const userToRegister = {
        username,
        email,
        password,
        confirmPassword
      }

      const response = fetch('https://localhost:44339/api/auth/register', {
        method: 'POST', // *GET, POST, PUT, DELETE, etc.
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(userToRegister)
      });

      console.log(response)
    }
  }

  return (
    <main>
      <section className="container">
        <form method="post" onSubmit={submitForm}>
          <label htmlFor="username">
            Nazwa użytkownika
            <input id="username" type="text" onChange={handleUsernameInput} />
          </label>
          <label htmlFor="email">
            Email
            <input id="email" type="email" onChange={handleEmailInput} />
          </label>
          <label htmlFor="password">
            Hasło
            <input id="password" type="password" onChange={handlePasswordInput} />
          </label>
          <label htmlFor="confirmPassword">
            Potwierdź hasło
            <input id="confirmPassword" type="password" onChange={handleConfirmPasswordInput} />
          </label>
          <button type="submit">Zarejestruj się</button>
        </form>
      </section>

    </main>
  );
}

export default Registration;