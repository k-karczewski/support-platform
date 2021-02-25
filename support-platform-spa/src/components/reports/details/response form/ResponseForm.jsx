import React, { useState } from 'react';
import { Prompt } from 'react-router-dom';

import FormSubmitButton from '../../../shared/forms/submit button/FormSubmitButton';
import FormTextAreaInput from '../../../shared/forms/textarea input/FormTextAreaInput';
import FormErrorsPanel from '../../../shared/forms/errors panel/FormErrorsPanel';

import './ResponseForm.sass';

const ResponseForm = ({ sendResponseHandler }) => {
  const [response, setResponse] = useState('');
  const [formErrors, setFormErrors] = useState([]);

  const handleResponse = event => {
    setResponse(event.target.value);
  }

  const formIsValid = () => {
    const formErrors = [];
    if (response.length === 0) {
      formErrors.push("Proszę wpisać wiadomość")
    }

    setFormErrors(formErrors);
    return formErrors.length === 0 ? true : false;
  }

  const handleSubmit = event => {
    event.preventDefault();

    if (formIsValid()) {
      sendResponseHandler(response);
      setResponse('');
      setFormErrors([]);
    }
  }

  return (
    <section className="report__response">
      <h3 className="form__heading">Odpowiedz na zgłoszenie</h3>
      <form method="post" onSubmit={handleSubmit}>
        <FormTextAreaInput htmlFor="response" onChangeHandler={handleResponse} placeholder="Wpisz wiadomość tutaj..." value={response} />
        <FormSubmitButton text="Wyślij wiadomość" />
      </form>
      { formErrors.length > 0 ? <FormErrorsPanel errors={formErrors} /> : null}

      <Prompt
        when={response.length > 0}
        message="Masz niezapisane zmiany. 
        Czy na pewno chcesz opuścić tę stronę?"
      />
    </section>
  );
}

export default ResponseForm;