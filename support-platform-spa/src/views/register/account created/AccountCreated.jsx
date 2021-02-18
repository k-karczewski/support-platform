import React from 'react';
import { NavLink } from 'react-router-dom';

import './AccountCreated.sass';

const AccountCreated = () => {
  return (
    <main className="accountCreated__message">
      <h2>Twoje konto zostało utworzone pomyślnie</h2>
      <h3>Aktywuj swoje konto przy pomocy linka aktywacyjnego, który właśnie został wysłany na Twoj adres email</h3>
      <NavLink to="/login">Przejdź do strony logowania</NavLink>
    </main>
  );
}

export default AccountCreated