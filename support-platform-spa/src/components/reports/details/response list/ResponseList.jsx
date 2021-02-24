import React from 'react';

import ResponseItem from './response item/ResponseItem';

import './ResponseList.sass';

const ResponseList = ({ responses }) => {

  const responseItems = responses.map(response => <ResponseItem key={response.id} id={response.id} message={response.message} date={response.date} createdBy={response.createdBy} />)

  return (
    <section className="report__responses">
      <h2>Lista odpowiedzi:</h2>
      <ul>
        {responseItems}
      </ul>
    </section>
  );
}

export default ResponseList;