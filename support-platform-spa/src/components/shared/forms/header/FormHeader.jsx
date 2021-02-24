import React from 'react';

import './FormHeader.sass';

const FormHeader = ({ heading, description }) => {
  return (
    <header className="form__header">
      <h2 className="form__heading">{heading}</h2>
      <p className="form__description">{description}</p>
    </header>
  );
}

export default FormHeader;