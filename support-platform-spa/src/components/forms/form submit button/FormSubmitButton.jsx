import React from 'react';

import './FormSubmitButton.sass'

const FormSubmitButton = ({ text }) => {
  return (
    <button type="submit" className="submit__button">{text}</button>
  );
}

export default FormSubmitButton;