import React from 'react';
import { Link } from 'react-router-dom';

import { ReportStatusConverter } from '../../../../../_helpers/ReportStatusConverter';

import './ReportListItem.sass';

const ReportListItem = ({ id, date, heading, status, createdBy }) => {
  return (
    <li className="report__item">
      <Link className="report__link" to={`reports/details/${id}`} >
        <p className="item__element">#{id}</p>
        <p className="item__element">{heading}</p>
        <p className="item__element">{date}</p>
        <p className="item__element">{ReportStatusConverter(status)}</p>
        <p className="item__element">{createdBy}</p>
      </Link>
    </li>
  );
}

export default ReportListItem;