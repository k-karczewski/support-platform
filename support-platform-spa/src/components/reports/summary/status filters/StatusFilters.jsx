import React from 'react';

import './StatusFilters.sass';

const StatusFilters = ({ currentFilter, onClickHandler }) => {
  return (
    <section className="status__filters">
      <button className={currentFilter === 0 ? "filter__selected" : null} onClick={() => onClickHandler(0)} disabled={currentFilter === 0 ? true : false}>Nowe</button>
      <button className={currentFilter === 1 ? "filter__selected" : null} onClick={() => onClickHandler(1)} disabled={currentFilter === 1 ? true : false}>Rozpatrywane</button>
      <button className={currentFilter === 2 ? "filter__selected" : null} onClick={() => onClickHandler(2)} disabled={currentFilter === 2 ? true : false}>Zamknięte</button>
    </section>
  );
}

export default StatusFilters;