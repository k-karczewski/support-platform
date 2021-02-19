import React from 'react';
import { NavLink } from 'react-router-dom';
import AuthService from '../../_services/AuthService';

import './Navigation.sass';

const Navigation = ({ userLoggedIn }) => {
  const handleLogout = () => {
    const authService = new AuthService();
    authService.logout();
  }

  return (
    <nav className="nav">
      <div className="nav__logo">
        <NavLink to="/">Support Platform</NavLink>
      </div>
      <ul className="nav__links">
        {!userLoggedIn ?
          <>
            <li className="nav__link">
              <NavLink to="/login">Zaloguj się</NavLink>
            </li>
            <li className="nav__link">
              <NavLink to="/register">Zarejestruj się</NavLink>
            </li>
          </> :
          <li className="nav__link">
            <NavLink to="/" onClick={handleLogout}>Wyloguj się</NavLink>
          </li>
        }
      </ul>
    </nav>
  );
}

export default Navigation;