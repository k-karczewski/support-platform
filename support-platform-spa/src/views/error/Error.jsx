import React from 'react';

import './Error.sass';

const Error = () => {
  return (
    <section className="error__view">
      <h2>Ups... Coś poszło nie tak :(</h2>
      <p>Podana strona nie istnieje lub nie masz do niej dostępu.</p>
    </section>
  );
}

export default Error;