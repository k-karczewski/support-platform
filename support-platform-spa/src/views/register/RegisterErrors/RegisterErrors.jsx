import React from 'react';

import './RegisterErrors.sass'

const RegisterErrors = ({ errors }) => {

  const errorElements = errors.map(error => <li><p className="register__error">{error}</p></li>)
  return (
    <aside className="register__errors">
      <h2>Błąd!</h2>
      <ul>
        {errorElements}
      </ul>
    </aside>
  );

}

export default RegisterErrors;