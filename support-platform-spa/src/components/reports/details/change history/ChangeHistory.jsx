import React from 'react';
import ChangeHistoryEntry from './change history entry/ChangeHistoryEntry';

import './ChangeHistory.sass';

const ChangeHistory = ({ history }) => {

  const historyEntries = history.map(entry => <li key={entry.id}><ChangeHistoryEntry date={entry.date} message={entry.message} /></li>)
  return (
    <section className="history__list">
      <h4 className="">Historia zmian:</h4>
      <ul>
        {historyEntries}
      </ul>
    </section>
  );
}

export default ChangeHistory;