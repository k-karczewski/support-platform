import React from 'react';

import './ServerError.sass';

const ServerError = () => {
  return (
    <section className="error__server">
      <h2>Ups... Coś poszło nie tak :(</h2>
      <p>Błąd poczas komunikacji z serwerem.</p>
    </section>
  );
}

export default ServerError;