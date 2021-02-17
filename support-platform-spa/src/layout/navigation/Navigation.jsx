import React from 'react';
import { NavLink } from 'react-router-dom';

import './Navigation.sass';

const Navigation = () => {
  return (
    <nav className="nav">
      <div className="nav__logo">
        <NavLink to="/">Support Platform</NavLink>
      </div>
      <ul className="nav__links">
        <li className="nav__link">
          <NavLink to="/login">Zaloguj się</NavLink>
        </li>
        <li className="nav__link">
          <NavLink to="/register">Zarejestruj się</NavLink>
        </li>
      </ul>
    </nav>
  );
}

export default Navigation;