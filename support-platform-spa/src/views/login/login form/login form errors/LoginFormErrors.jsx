import React from 'react';

import './LoginFormErrors.sass';

const LoginFormErrors = ({ errors }) => {
  const errorElements = errors.map(error => <li key={error}><p className="login__error">{error}</p></li>)
  return (
    <aside className="login__errors">
      <h2>Błąd!</h2>
      <ul>
        {errorElements}
      </ul>
    </aside>
  );
}

export default LoginFormErrors;