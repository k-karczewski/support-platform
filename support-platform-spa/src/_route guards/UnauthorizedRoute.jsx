import React from 'react';
import { Route, Redirect } from 'react-router-dom';

const UnauthorizedRoute = ({ component: Component, isAuthenticated, ...rest }) => {
  return (
    <Route {...rest} render={(props) => (
      isAuthenticated() === false
        ? <Component {...props} />
        : <Redirect to='/reports' />
    )} />
  )
}

export default UnauthorizedRoute;