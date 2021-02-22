import React from 'react';

import './MessageElement.sass';

const MessageElement = ({ createdBy, date, message }) => {
  return (
    <section className="message__element">
      <p className="message__heading">Dnia {date} {createdBy} napisał(a) wiadomosć:</p>
      <p className="message__text">{message}</p>
    </section>
  );
}

export default MessageElement;