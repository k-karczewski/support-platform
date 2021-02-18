import React, { useEffect, useState } from 'react';
import { useHistory } from 'react-router-dom';

import './ConfirmationMessage.sass';

const ConfirmationMessage = (props) => {
  const [message] = useState(props.location.state ? props.location.state.message : null);
  const history = useHistory();

  useEffect(() => {
    if (!message) {
      history.push('/')
    }
  }, [message, history])

  return (<div className="confirmation__message">{message}</div>);
}

export default ConfirmationMessage;