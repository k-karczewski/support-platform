import { apiUrl } from '../_environments/environment';
import HttpService from './HttpService';

export default class ReportService {

  constructor() {
    this.http = new HttpService();
  }

  getReportList = async (resourceUrl) => {
    return await this.http.sendRequest(resourceUrl, 'get')
      .then(async response => {
        const json = await response.json()
        if (response.ok) {
          return json;
        }
        return Promise.reject(json)
      })
  }

  getReportDetailsById = (reportId) => {
    return this.http.sendRequest(`${apiUrl}/report/${reportId}`, 'get')
      .then(async response => {
        const json = await response.json();
        if (response.ok) {
          return json;
        }
        return Promise.reject(json);
      });
  }

  createReport = (heading, message, file) => {
    const reportToCreate = {
      heading,
      message,
      file
    }

    return this.http.sendRequest(`${apiUrl}/report/create`, 'POST', reportToCreate)
      .then(async response => {
        const json = await response.json();
        if (response.ok) {
          return json;
        }
        return Promise.reject(json);
      });
  }

  updateStatus = (reportId, newStatus) => {
    const statusToUpdate = {
      reportId,
      newStatus: newStatus
    }

    return this.http.sendRequest(`${apiUrl}/report/change-status`, 'post', statusToUpdate)
      .then(async response => {
        const json = await response.json();
        if (response.ok) {
          return json;
        }
        return Promise.reject(json)
      })
  }

  postResponse = (reportId, message) => {
    const reportResponse = {
      reportId,
      message: message
    }

    return this.http.sendRequest(`${apiUrl}/report/send-response`, 'post', reportResponse)
      .then(async response => {
        const json = await response.json();
        if (response.ok) {
          return json;
        }
        return Promise.reject(json)
      })
  }
}