import React, { useEffect } from 'react';
import { useLocation, useHistory } from 'react-router-dom';

const ConfirmAccount = () => {
  const location = useLocation().search;
  const history = useHistory()

  useEffect(() => {
    const query = new URLSearchParams(location);
    const userId = Number(query.get('userId'));
    const token = query.get('token');
    if (userId && token) {

      const data = {
        userId,
        token
      };

      fetch('https://localhost:44339/api/auth/confirmEmail', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
      })
        .then(response => {
          debugger
          if (!response.ok) {
            throw response.json();
          }
          history.push('/accountConfirmed');
        })
        .catch((error) => {
          console.log(error);
        });
    }
  }, [location, history]);

  return (<div>trwa potwierdzanie konta</div>);
}

export default ConfirmAccount;