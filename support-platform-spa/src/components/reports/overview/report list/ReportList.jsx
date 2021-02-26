import React from 'react';

import ReportListHeader from './report list header/ReportListHeader';
import ReportListItem from './report list item/ReportListItem';

import './ReportList.sass';

const ReportList = ({ heading, reports, message }) => {

  const getReportItems = () => {
    if (reports && reports.length > 0) {
      const reportItems = reports.map(report =>
        <ReportListItem
          key={report.id}
          id={report.id}
          heading={report.heading}
          date={report.date}
          status={report.status}
          createdBy={report.createdBy} />);

      return reportItems;
    } else {
      return (
        <p>{message}</p>
      );
    }
  }

  return (
    <section className="report__list">
      <h3 className="list__heading">{heading}</h3>
      <ReportListHeader />
      <ul>
        {getReportItems()}
      </ul>
    </section>
  );
}

export default ReportList;