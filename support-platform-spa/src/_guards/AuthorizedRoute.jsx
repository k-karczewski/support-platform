import React from 'react';
import { Route, Redirect } from 'react-router-dom';

const AuthorizedRoute = ({ component: Component, authCondition, ...rest }) => {
  return (
    <Route {...rest} render={(props) => (
      authCondition() === true
        ? <Component {...props} />
        : <Redirect to='/login' />
    )} />
  )
}

export default AuthorizedRoute;