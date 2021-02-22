import React, { useEffect, useState } from 'react';
import { Link, useHistory } from 'react-router-dom';

import ChangeHistory from '../../../components/reports/details/change history/ChangeHistory';
import DetailedReport from '../../../components/reports/details/detailed report/DetailedReport';
import ResponseList from '../../../components/reports/details/response list/ResponseList';
import ResponseForm from '../../../components/reports/details/response form/ResponseForm';

import AuthService from '../../../_services/AuthService';
import HttpService from '../../../_services/HttpService';
import { apiUrl } from '../../../_environments/environment';

import './ReportDetails.sass';

const ReportDetails = ({ location, match, details }) => {
  const [userRole, setUserRole] = useState('');
  const [reportDetails, setReportDetails] = useState(location.state ? location.state.data : null);
  const [reportId] = useState(match.params.id);
  const history = useHistory();

  const tmp = {
    id: 12,
    heading: "Zgłoszenie 1",
    message: "POMOCY!!!!! Nie działa mi",
    responses: [
      {
        id: 23,
        message: "Spróbuj zrestartować",
        date: "12.01.2021",
        createdBy: "Admin"
      },
      {
        id: 28,
        message: "U mnie działa",
        date: "13.01.2021",
        createdBy: "Najlepszy pracownik"
      }
    ],
    date: "11.01.2021",
    modificationEntries: [
      {
        id: 2,
        message: "stworzono raport",
        date: "11.01.2021"
      }
    ],
    attachment: {
      name: "document 1.pdf",
      url: "url2222"
    },
    status: 1
  }

  useEffect(() => {
    const authService = new AuthService();
    const role = authService.getDecodedToken().role;
    setUserRole(role);
  }, [])

  useEffect(() => {
    if (!reportDetails) {
      const http = new HttpService();
      http.sendRequest(`${apiUrl}/report/${reportId}`, 'get')
        .then(async response => {
          const json = await response.json();
          if (response.ok) {
            return json;
          }
          return Promise.reject(json);
        })
        .then(data => {
          console.log(data);
          setReportDetails(data)
        })
        .catch(() => {
          history.push('/error');
        });
    }
  }, [reportDetails, reportId, history])


  const renderComponent = () => {
    if (reportDetails) {
      return (
        <div className="report__details">
          <DetailedReport id={reportDetails.id} heading={reportDetails.heading} message={reportDetails.message} date={reportDetails.date} status="Nowe" createdBy={reportDetails.createdBy} attachment={reportDetails.attachment} />
          { reportDetails.responses.length > 0 ? <ResponseList responses={reportDetails.responses} /> : null}
          { userRole === "Employee" && reportDetails.status === 1 ? <ResponseForm /> : null}
          <ChangeHistory history={reportDetails.modificationEntries} />
        </div>
      )
    }
    else {
      return null;
    }
  }
  
  return (
    renderComponent()
  );
}


export default ReportDetails;