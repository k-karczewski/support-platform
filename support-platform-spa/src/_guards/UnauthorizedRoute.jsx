import React from 'react';
import { Route, Redirect } from 'react-router-dom';

const UnauthorizedRoute = ({ component: Component, authCondition, ...rest }) => {
  return (
    <Route {...rest} render={(props) => (
      authCondition() === false
        ? <Component {...props} />
        : <Redirect to='/reports' />
    )} />
  )
}

export default UnauthorizedRoute;