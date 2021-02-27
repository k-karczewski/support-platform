import React, { useState } from 'react';
import Dropdown from 'react-dropdown'

import { GetStatuses } from '../../../../_helpers/ReportStatusConverter';

import './StatusFilters.sass';

const StatusFilters = ({ currentFilter, onClickHandler }) => {
  const [statuses] = useState([...GetStatuses(), { label: "Wszystkie", value: null }]);

  const handleFilterChange = data => {
    onClickHandler(data.value);
  }

  return (
    <section className="status__filters">
      <Dropdown className="filter__dropdown" options={statuses} onChange={handleFilterChange} placeholder={statuses.find(x => x.value === currentFilter).label} />
    </section>
  );
}

export default StatusFilters;