import React from 'react';
import { NavLink } from 'react-router-dom';
import AuthService from '../../_services/AuthService';

import './Navigation.sass';

const Navigation = ({ decodedToken }) => {
  const handleLogout = () => {
    const authService = new AuthService();
    authService.logout();
  }

  const generateNavLinks = () => {
    if (!decodedToken) {
      return (<>
        <li className="nav__link">
          <NavLink to="/login">Zaloguj się</NavLink>
        </li>
        <li className="nav__link">
          <NavLink to="/register">Zarejestruj się</NavLink>
        </li>
      </>)
    } else if (decodedToken.role === 'Client') {
      return (<>
        <li className="nav__link">
          <NavLink to="/reports/create">Stwórz zgłoszenie</NavLink>
        </li>
        <li className="nav__link">
          <NavLink to="/" onClick={handleLogout}>Wyloguj się</NavLink>
        </li>
      </>)
    } else {
      return (
        <li className="nav__link">
          <NavLink to="/" onClick={handleLogout}>Wyloguj się</NavLink>
        </li>
      )
    }
  }

  return (
    <nav className="nav">
      <div className="nav__logo">
        <NavLink to="/">Support Platform</NavLink>
      </div>
      <ul className="nav__links">
        {
          generateNavLinks()
        }
      </ul>
    </nav>
  );
}

export default Navigation;