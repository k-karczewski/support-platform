import React, { useEffect } from 'react';
import { useLocation, useHistory } from 'react-router-dom';

import './ConfirmationInProgress.sass';

const ConfirmationInProgress = () => {
  const location = useLocation().search;
  const history = useHistory();

  useEffect(() => {
    const query = new URLSearchParams(location);
    const userId = query.get('userId');
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
          if (!response.ok) {
            throw response.json();
          }
          const message = 'Twoje konto zostało potwierdzone pomyślnie. Możesz się już zalogować.';
          history.push('/confirmMessage', { message });
        })
        .catch(() => {
          const message = 'Błąd podczas potwierdzania konta. Spróbuj ponownie za chwilę :)';
          history.push('/confirmMessage', { message });
        });
    } else {
      history.push('/');
    }
  }, [location, history]);

  return (<div className="confirmation__inProgress">Proszę czekać, trwa potwierdzanie konta</div>);
}

export default ConfirmationInProgress;