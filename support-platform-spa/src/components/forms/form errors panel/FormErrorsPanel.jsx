import React from 'react';

import './FormErrorsPanel.sass';

const FormErrorsPanel = ({ errors }) => {
  const errorElements = errors.map(error => <li key={error}><p className="error__item">{error}</p></li>)
  return (
    <aside className="error__list">
      <h2>Błąd!</h2>
      <ul>
        {errorElements}
      </ul>
    </aside>
  );
}

export default FormErrorsPanel;