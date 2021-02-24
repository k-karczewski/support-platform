import React from 'react';

import './FormTextAreaInput.sass';

const FormTextAreaInput = ({ htmlFor, labelText, onChangeHandler, placeholder, value }) => {
  return (
    <label htmlFor={htmlFor} className="textArea__label">
      {labelText}
      <textarea id={htmlFor} name={htmlFor} className="textArea__input" onChange={onChangeHandler} placeholder={placeholder} value={value} />
    </label>
  );
}

export default FormTextAreaInput;