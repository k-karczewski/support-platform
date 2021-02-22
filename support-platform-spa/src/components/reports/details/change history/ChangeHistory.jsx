import React from 'react';

import './ChangeHistory.sass';

const ChangeHistory = ({ history }) => {

  const historyEntries = history.map(entry => <li key={entry.id}><p className="history__entry">{entry.message} dnia {entry.date}</p></li>)
  return (
    <section className="history">
      <h4 className="">Historia zmian:</h4>
      <ul>
        {historyEntries}
      </ul>
    </section>
  );
}

export default ChangeHistory;