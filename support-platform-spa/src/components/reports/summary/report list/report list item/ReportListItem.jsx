import React from 'react';
import { Link } from 'react-router-dom';

import './ReportListItem.sass';

const ReportListItem = ({ id, date, heading, status, createdBy }) => {

  const convertStatus = () => {
    if (status === 0) {
      return "Nowe";
    } else if (status === 1) {
      return "Rozpatrywane";
    } else if (status === 2) {
      return "Zamknięte";
    } else {
      return "Status";
    }
  }

  return (
    <li className="report__item">
      <Link className="report__link" to={`reports/details/${id}`} >
        <p className="item__element">#{id}</p>
        <p className="item__element">{heading}</p>
        <p className="item__element">{date}</p>
        <p className="item__element">{convertStatus()}</p>
        <p className="item__element">{createdBy}</p>
      </Link>
    </li>
  );
}

export default ReportListItem;