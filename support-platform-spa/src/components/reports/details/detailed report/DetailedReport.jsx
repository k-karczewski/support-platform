import React from 'react';
import { Link } from 'react-router-dom';

import MessageElement from '../../../shared/message element/MessageElement';

import './DetailedReport.sass';

const DetailedReport = ({ id, heading, message, date, status, createdBy, attachment }) => {
  return (
    <section className="detailed__report">
      <section className="report__header">
        <h2> Zgłoszenie: #{id}</h2>
      </section>
      <section className="report__content">
        <h4 className="content__item report__heading">Tytuł: <span>{heading}</span></h4>
        <MessageElement date={date} createdBy={createdBy} message={message} />
        <p className="content__item report__status">Status: <span>{status}</span></p>
        {attachment ? <p className="content__item report__status">Załącznik: <Link to={attachment.url}>{attachment.name}</Link></p> : null}
      </section>
    </section>
  );
}

export default DetailedReport;