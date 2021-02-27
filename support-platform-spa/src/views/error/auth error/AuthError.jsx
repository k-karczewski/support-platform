import React from 'react';

import './AuthError.sass';

const AuthError = () => {
  return (
    <section className="error__auth">
      <h2>Ups... Coś poszło nie tak :(</h2>
      <p>Podana strona nie istnieje lub nie masz do niej dostępu.</p>
    </section>
  );
}

export default AuthError;