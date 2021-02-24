import React from 'react';

import './FormTextInput.sass';

const FormTextInput = ({ labelText, type, htmlFor, onChangeHandler }) => {
  return (
    <label className="textInput__label" htmlFor={htmlFor}>
      {labelText}
      <input className="textInput__input" type={type} id={htmlFor} name={htmlFor} onChange={onChangeHandler} />
    </label>
  );
}

export default FormTextInput;