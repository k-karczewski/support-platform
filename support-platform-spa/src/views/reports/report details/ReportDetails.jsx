import React, { useEffect, useState } from 'react';
import { useHistory } from 'react-router-dom';

import ChangeHistory from '../../../components/reports/details/change history/ChangeHistory';
import DetailedReport from '../../../components/reports/details/detailed report/DetailedReport';
import ResponseList from '../../../components/reports/details/response list/ResponseList';
import ResponseForm from '../../../components/reports/details/response form/ResponseForm';

import AuthService from '../../../_services/AuthService';
import HttpService from '../../../_services/HttpService';
import { apiUrl } from '../../../_environments/environment';

import './ReportDetails.sass';

const ReportDetails = ({ location, match }) => {
  const [userRole, setUserRole] = useState('');
  const [reportDetails, setReportDetails] = useState(location.state ? location.state.data : null);
  const [reportId] = useState(match.params.id);
  const history = useHistory();

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
          setReportDetails(data)
        })
        .catch(() => {
          history.push('/error');
        });
    }
  }, [reportDetails, reportId, history])

  const handleStatusUpdate = newStatus => {
    const statusToUpdate = {
      reportId,
      newStatus: newStatus
    }
    const http = new HttpService();

    http.sendRequest(`${apiUrl}/report/change-status`, 'post', statusToUpdate)
      .then(async response => {
        const json = await response.json();
        if (response.ok) {
          return json;
        }
        return new Error(json)
      })
      .then(data => {
        setReportDetails({ ...reportDetails, status: data.status, modificationEntries: data.modificationEntries })
      })
      .catch(() => {
        history.push('/error');
      });
  }

  const handleSendResponse = message => {
    const reportResponse = {
      reportId,
      message: message
    }
    const http = new HttpService();

    http.sendRequest(`${apiUrl}/report/send-response`, 'post', reportResponse)
      .then(async response => {
        const json = await response.json();
        if (response.ok) {
          return json;
        }
        return new Error(json)
      })
      .then(data => {
        console.log(data)

        const newResponse = {
          id: data.id,
          message: data.message,
          date: data.date,
          createdBy: data.createdBy
        }

        const newModification = {
          id: data.modificationEntry.id,
          message: data.modificationEntry.message,
          date: data.modificationEntry.date,
        }

        setReportDetails({ ...reportDetails, responses: [...reportDetails.responses, newResponse], modificationEntries: [...reportDetails.modificationEntries, newModification] })
      })
      .catch(() => {
        history.push('/error');
      });

  }

  const renderComponent = () => {
    if (reportDetails) {
      return (
        <div className="report__details">
          <DetailedReport id={reportDetails.id}
            heading={reportDetails.heading}
            message={reportDetails.message}
            date={reportDetails.date}
            status={reportDetails.status}
            createdBy={reportDetails.createdBy}
            attachment={reportDetails.attachment}
            userRole={userRole}
            statusUpdateHandler={handleStatusUpdate} />
          { reportDetails.responses.length > 0 ? <ResponseList responses={reportDetails.responses} /> : null}
          { userRole === "Employee" && reportDetails.status === 1 ? <ResponseForm sendResponseHandler={handleSendResponse} /> : null}
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