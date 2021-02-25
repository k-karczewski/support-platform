import React, { useState } from 'react';
import { Prompt } from 'react-router-dom';

import FormSubmitButton from '../../../shared/forms/submit button/FormSubmitButton';
import FormTextAreaInput from '../../../shared/forms/textarea input/FormTextAreaInput';

import './ResponseForm.sass';

const ResponseForm = ({ sendResponseHandler }) => {
  const [response, setResponse] = useState('');

  const handleResponse = event => {
    setResponse(event.target.value);
  }

  const handleSubmit = event => {
    event.preventDefault();
    sendResponseHandler(response);
    setResponse('');
  }

  return (
    <section className="report__response">
      <h3 className="form__heading">Odpowiedz na zgłoszenie</h3>
      <form method="post" onSubmit={handleSubmit}>
        <FormTextAreaInput htmlFor="response" onChangeHandler={handleResponse} placeholder="Wpisz wiadomość tutaj..." value={response} />
        <FormSubmitButton text="Wyślij wiadomość" />
      </form>
      <Prompt
        when={response.length > 0}
        message="Masz niezapisane zmiany. 
                Czy na pewno chcesz opuścić tę stronę?"
      />
    </section>
  );
}

export default ResponseForm;