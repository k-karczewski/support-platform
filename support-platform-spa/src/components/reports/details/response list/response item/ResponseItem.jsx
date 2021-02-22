import React from 'react';

import MessageElement from '../../../../shared/message element/MessageElement';

import './ResponseItem.sass';

const ResponseItem = ({ id, message, date, createdBy }) => {


  return (
    <li className="reponse__item">
      <h4>Odpowiedź: #{id}</h4>
      <MessageElement date={date} createdBy={createdBy} message={message} />
    </li>
  );
}

export default ResponseItem;