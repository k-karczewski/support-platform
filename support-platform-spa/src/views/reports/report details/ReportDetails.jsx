import React, { useEffect, useState } from 'react';
import { useHistory } from 'react-router-dom';

import ChangeHistory from '../../../components/reports/details/change history/ChangeHistory';
import DetailedReport from '../../../components/reports/details/detailed report/DetailedReport';
import ResponseList from '../../../components/reports/details/response list/ResponseList';
import ResponseForm from '../../../components/reports/details/response form/ResponseForm';

import AuthService from '../../../_services/AuthService';
import ReportService from '../../../_services/ReportService';
import { EmployeeRoleName } from '../../../_environments/environment';

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
      const reportService = new ReportService();

      reportService.getReportDetailsById(reportId)
        .then(data => {
          setReportDetails(data)
        })
        .catch(() => {
          history.push('/error-server');
        });
    }
  }, [reportDetails, reportId, history])

  const handleStatusUpdate = newStatus => {
    const reportService = new ReportService();

    reportService.updateStatus(reportId, newStatus)
      .then(data => {
        setReportDetails({ ...reportDetails, status: data.status, modificationEntries: data.modificationEntries })
      })
      .catch(() => {
        history.push('/error-server');
      });
  }

  const handleSendResponse = message => {
    const reportService = new ReportService();
    reportService.postResponse(reportId, message)
      .then(data => {
        setReportDetails(
          {
            ...reportDetails,
            responses: data.responses,
            modificationEntries: data.modificationEntries
          }
        );
      })
      .catch(() => {
        history.push('/error-server');
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
          { userRole === EmployeeRoleName && reportDetails.status === 1 ? <ResponseForm sendResponseHandler={handleSendResponse} /> : null}
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