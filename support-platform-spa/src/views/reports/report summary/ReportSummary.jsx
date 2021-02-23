import React, { useEffect, useState } from 'react';
import ReactPaginate from 'react-paginate';

import ReportList from '../../../components/reports/summary/report list/ReportList';
import StatusFilters from '../../../components/reports/summary/status filters/StatusFilters';
import HttpService from '../../../_services/HttpService';
import { apiUrl } from '../../../_environments/environment';

import './ReportSummary.sass';

const Reports = () => {
  const [currentPage, setCurrentPage] = useState(0);
  const [itemsPerPage] = useState(10);
  const [totalPages, setTotalPages] = useState(1);
  const [statusFilter, setStatusFilter] = useState(0);
  const [reports, setReports] = useState([]);

  useEffect(() => {
    const paginationOptions = {
      pageNumber: currentPage,
      itemsPerPage: itemsPerPage,
      reportStatus: statusFilter
    }
    const fetchData = async () => {
      const http = new HttpService();
      await http.sendRequest(`${apiUrl}/report/list`, 'post', paginationOptions)
        .then(async response => {
          const json = await response.json()
          if (response.ok) {
            return json;
          }
          return new Error(json)
        })
        .then(data => {
          setTotalPages(data.totalPages);
          setReports(data.reportListItems);
        });
    }

    fetchData()
  }, [currentPage, itemsPerPage, statusFilter])

  const handlePageChange = (data) => {

    setCurrentPage(data.selected);
  }

  const filterChangeHandler = status => {
    setStatusFilter(status);
  }

  const renderList = () => {
    let heading = "Nowe zgłoszenia";
    if (statusFilter === 1) {
      heading = "Zgłoszenia rozpatrywane";
    } else if (statusFilter === 2) {
      heading = "Zamkniete zgłoszenia";
    }
    return <ReportList heading={heading} reports={reports} />
  }

  return (
    <main className="report__summary">
      <div className="container">
        <StatusFilters currentFilter={statusFilter} onClickHandler={filterChangeHandler} />
        {renderList()}
        <ReactPaginate
          previousLabel={'<'}
          nextLabel={'>'}
          breakLabel={'...'}
          breakClassName={'break-me'}
          pageCount={totalPages}
          marginPagesDisplayed={2}
          pageRangeDisplayed={1}
          onPageChange={handlePageChange}
          containerClassName={'pagination'}
          subContainerClassName={'pages pagination'}
          activeClassName={'active'} />
      </div>

    </main>
  );
}

export default Reports;