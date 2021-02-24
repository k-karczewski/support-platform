import React from 'react';

import './ChangeHistoryEntry.sass';

const ChangeHistoryEntry = ({ date, message }) => {
  return (
    <section className="history__entry">
      <p className="history__date">{date}</p>
      <p className="history__message">{message}</p>
    </section>
  );
}

export default ChangeHistoryEntry;