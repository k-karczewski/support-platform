import React, { useState } from 'react';
import Dropdown from 'react-dropdown';

import { GetStatuses } from '../../../../_helpers/ReportStatusConverter';

import 'react-dropdown/style.css';
import './StatusEditor.sass';

const StatusEditor = ({ currentStatus, statusUpdateHandler }) => {
  const [dropdownChanged, setDropdownTouched] = useState(false);
  const [dropdownValue, setDropdownValue] = useState(currentStatus);
  const [statuses] = useState(GetStatuses());

  const handleOnChange = data => {
    if (dropdownChanged === false && data.value !== currentStatus) {
      setDropdownTouched(true);
      setDropdownValue(data.value);
    } else if (dropdownChanged === true && data.value === currentStatus) {
      setDropdownTouched(false);
      setDropdownValue(data.value);
    }
  }

  const handleSaveChanges = () => {
    setDropdownTouched(false);
    statusUpdateHandler(dropdownValue);
  }

  return (
    <div className="status__editor">
      <p>Status:</p>
      <Dropdown className="status__dropdown" options={statuses} onChange={handleOnChange} placeholder={statuses.find(x => x.value === currentStatus).label} />
      {dropdownChanged ? <button onClick={handleSaveChanges}>Zapisz</button> : null}
    </div>
  );
}

export default StatusEditor;