import React, { useState } from 'react';

import FormTextAreaInput from '../../../forms/form textarea input/FormTextAreaInput';
import FormSubmitButton from '../../../forms/form submit button/FormSubmitButton';

import './ResponseForm.sass';

const ResponseForm = () => {
  const [response, setResponse] = useState('');

  const handleResponse = event => {
    setResponse(event.target.value);
  }

  const handleSubmit = event => {
    event.preventDefault();

    if (response.length > 0) {
      console.log(response);
    }
  }

  return (
    <section className="report__response">
      <h3 className="form__heading">Odpowiedz na zgłoszenie</h3>
      <form method="post" onSubmit={handleSubmit}>
        <FormTextAreaInput htmlFor="response" onChangeHandler={handleResponse} />
        <FormSubmitButton text="Wyślij wiadomość" />
      </form>
    </section>
  );
}

export default ResponseForm;