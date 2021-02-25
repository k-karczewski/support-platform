import React, { useState } from 'react';
import { useHistory, Prompt } from 'react-router-dom';

import ReportService from '../../../_services/ReportService';

import FormHeader from '../../shared/forms/header/FormHeader';
import FormTextInput from '../../shared/forms/text input/FormTextInput'
import FormTextAreaInput from '../../shared/forms/textarea input/FormTextAreaInput';
import FormSubmitButton from '../../shared/forms/submit button/FormSubmitButton';

import FormErrorsPanel from '../../shared/forms/errors panel/FormErrorsPanel';
import './CreateReportForm.sass';

const CreateReportForm = () => {
  const [heading, setHeading] = useState('');
  const [message, setMessage] = useState('');
  const [file, setFile] = useState(null);
  const [formErrors, setFormErrors] = useState([]);
  const history = useHistory();


  const headerText = {
    heading: 'Stwórz nowe zgłoszenie',
    description: 'Wypełnij formularz by wysłać zgłoszenie'
  }

  const formIsValid = () => {
    const errorList = [];

    if (heading.length === 0) {
      errorList.push("Proszę wypełnić tytuł zgłoszenia");
    }
    if (message.length === 0) {
      errorList.push("Proszę wypełnić wiadomość zgłoszenia");
    }

    setFormErrors(errorList);
    return errorList.length === 0 ? true : false;
  }

  const handleFormSubmit = event => {
    event.preventDefault();
    if (formIsValid()) {

      const reportService = new ReportService();
      reportService.createReport(heading, message, file)
        .then(data => {
          history.push({
            pathname: `/reports/details/${data.id}`,
            state: {
              data
            }
          })
        })
        .catch(errors => {
          setFormErrors(errors)
        });

      setHeading('');
      setMessage('');
      setFile(null);
    }
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
    const file = {
      filename: files[0].name,
      fileInBytes: fileByteArray
    }

    setFile(file);
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
        </form>
      </div>
      {formErrors.length > 0 ? <FormErrorsPanel errors={formErrors} /> : null}
      <Prompt
        when={heading.length > 0 || message.length > 0 || file !== null}
        message="Masz niezapisane zmiany. 
                Czy na pewno chcesz opuścić tę stronę?"
      />
    </main>

  );
}

export default CreateReportForm;