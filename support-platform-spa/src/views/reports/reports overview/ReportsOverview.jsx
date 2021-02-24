import React, { useEffect, useState } from 'react';
import ReactPaginate from 'react-paginate';

import { apiUrl } from '../../../_environments/environment';
import HttpService from '../../../_services/HttpService';
import ReportList from '../../../components/reports/overview/report list/ReportList';
import StatusFilters from '../../../components/reports/overview/status filters/StatusFilters';

import './ReportsOverview.sass';

const ReportsOverview = () => {
  const [currentPage, setCurrentPage] = useState(1);
  const [itemsPerPage] = useState(10);
  const [reports, setReports] = useState([]);
  const [statusFilter, setStatusFilter] = useState(null);
  const [totalPages, setTotalPages] = useState(0);

  useEffect(() => {
    const fetchData = async () => {
      let resourceUrl = `${apiUrl}/report/list?pageNumber=${currentPage}`;

      if (statusFilter !== null) {
        resourceUrl += `&statusFilter=${statusFilter}`;
      }

      const http = new HttpService();
      await http.sendRequest(resourceUrl, 'get')
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

  const handlePageChange = ({ selected }) => {
    // selected stores page number counted from 0
    setCurrentPage(selected + 1);
  }

  const filterChangeHandler = status => {
    setStatusFilter(status);
  }

  const renderList = () => {
    let heading = "";
    if (statusFilter === 0) {
      heading = "Nowe zgłoszenia";
    } else if (statusFilter === 1) {
      heading = "Zgłoszenia rozpatrywane";
    } else if (statusFilter === 2) {
      heading = "Zamkniete zgłoszenia";
    } else {
      heading = "Wszystkie zgłoszenia"
    }
    return <ReportList heading={heading} reports={reports} />
  }

  return (
    <main className="reports__overview">
      <div className="container">
        <StatusFilters currentFilter={statusFilter} onClickHandler={filterChangeHandler} />
        {renderList()}
        <ReactPaginate
          previousLabel={'<'}
          nextLabel={'>'}
          breakLabel={'...'}
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

export default ReportsOverview;