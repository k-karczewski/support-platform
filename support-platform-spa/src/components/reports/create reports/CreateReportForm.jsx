import React, { useState } from 'react';

import FormHeader from '../../forms/form header/FormHeader';
import FormTextInput from '../../forms/form text input/FormTextInput'
import FormTextAreaInput from '../../forms/form textarea input/FormTextAreaInput';
import FormSubmitButton from '../../forms/form submit button/FormSubmitButton';

import './CreateReportForm.sass';

const CreateReportForm = () => {
  const [heading, setHeading] = useState('');
  const [message, setMessage] = useState('');
  const [fileInBytes, setFileInBytes] = useState([]);


  const headerText = {
    heading: 'Stwórz nowe zgłoszenie',
    description: 'Wypełnij formularz by wysłać zgłoszenie'
  }

  const handleFormSubmit = event => {
    event.preventDefault();

    const reportToCreate = {
      heading,
      message,
      fileInBytes
    }

    console.log(reportToCreate)
  }

  const handleTextInputs = event => {
    const { name, value } = event.target;
    switch (name) {
      case 'heading':
        setHeading(value);
        break;
      case 'message':
        setMessage(value);
        break;
      default:
        break;
    }
  }

  const handleFileInput = event => {
    const { files } = event.target

    var reader = new FileReader();
    var fileByteArray = [];
    reader.readAsArrayBuffer(files[0]);
    reader.onloadend = event => {
      if (event.target.readyState === FileReader.DONE) {
        var arrayBuffer = event.target.result,
          array = new Uint8Array(arrayBuffer);
        for (var i = 0; i < array.length; i++) {
          fileByteArray.push(array[i]);
        }
      }
    }
    setFileInBytes(fileByteArray);
  }

  return (
    <main className="report__create">
      <div className="container">
        <FormHeader heading={headerText.heading} description={headerText.description} />
        <form method="post" className="form__create" onSubmit={handleFormSubmit}>
          <FormTextInput type="text" htmlFor="heading" labelText="Tytuł zgłoszenia" onChangeHandler={handleTextInputs} />
          <FormTextAreaInput htmlFor="message" labelText="Opis zgłoszenia" onChangeHandler={handleTextInputs} />
          <input type="file" name="fileInput" id="fileInput" accept=".png, .jpg, .pdf" multiple={false} onChange={handleFileInput} />
          <FormSubmitButton text="Wyślij zgłoszenie" />
          {/* <FormTextInput type="text" htmlFor="message" labelText="Opis problemu" /> */}


        </form>




        {/* <form className="login__form" method="post" onSubmit={handleSubmit}>
          <label htmlFor="username">
            Nazwa użytkownika
          <input type="text" id="username" onChange={handleInputsValueChange} />
          </label>
          <label htmlFor="password">
            Hasło
          <input type="password" id="password" onChange={handleInputsValueChange} />
          </label>
          <button type="submit" className="form__submit">Zaloguj się</button>
          <Link to="/register" className="form__registerLink">Nie masz jeszcze konta? Zarejestruj się!</Link>
        </form>
      </div>

      {formErrors.length > 0 && <LoginFormErrors errors={formErrors} />} */}
      </div>
    </main>
  );
}

export default CreateReportForm;