import React from 'react';
import { Route, Redirect } from 'react-router-dom';

const ClientRoute = ({ component: Component, isAuthenticated, role, ...rest }) => {
  return (
    <Route {...rest} render={(props) => {
      if (isAuthenticated()) {
        if (role() === 'Client') {
          return <Component {...props} />
        } else {
          return <Redirect to='/reports' />
        }
      }
      else {
        return <Redirect to='/login' />
      }
    }}
    />
  )
}

export default ClientRoute