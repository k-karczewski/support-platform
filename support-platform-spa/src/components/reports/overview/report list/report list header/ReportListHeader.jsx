import React from 'react';

import './ReportListHeader.sass';

const ReportListHeader = () => {
  return (
    <div className="reportList__header">
      <p className="header__element">#</p>
      <p className="header__element">Tytuł</p>
      <p className="header__element">Data</p>
      <p className="header__element">Status</p>
      <p className="header__element">Zgłoszone przez</p>
    </div>
  );
}

export default ReportListHeader;