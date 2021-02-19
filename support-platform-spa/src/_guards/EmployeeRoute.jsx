import React from 'react';
import { Route, Redirect } from 'react-router-dom';

const EmployeeRoute = ({ component: Component, isAuthenticated, role, ...rest }) => {
  return (
    <Route {...rest} render={(props) => {
      if (isAuthenticated()) {
        if (role() === 'Employee') {
          return <Component {...props} />
        } else {
          return <Redirect to='/dashboard' />
        }
      }
      else {
        return <Redirect to='/login' />
      }
    }}
    />
  )
}

export default EmployeeRoute