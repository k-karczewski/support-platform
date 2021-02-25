import React from 'react';

import { EmployeeRoleName } from '../../../../_environments/environment';
import { ReportStatusConverter } from '../../../../_helpers/ReportStatusConverter';

import MessageElement from '../message element/MessageElement';
import StatusEditor from '../status editor/StatusEditor';

import './DetailedReport.sass';

const DetailedReport = ({ id, heading, message, date, status, createdBy, attachment, userRole, statusUpdateHandler }) => {
  return (
    <section className="detailed__report">
      <section className="report__header">
        <h2> Zgłoszenie: #{id}</h2>
      </section>
      <section className="report__content">
        <h4 className="content__item report__heading">Tytuł: <span>{heading}</span></h4>
        <MessageElement date={date} createdBy={createdBy} message={message} />
        {userRole === EmployeeRoleName ? <StatusEditor currentStatus={status} statusUpdateHandler={statusUpdateHandler} /> : <p className="content__item report__status">Status: <span>{ReportStatusConverter(status)}</span></p>}
        {attachment ? <p className="content__item report__status">Załącznik: <a href={attachment.url}>{attachment.name}</a></p> : null}
      </section>
    </section>
  );
}

export default DetailedReport;