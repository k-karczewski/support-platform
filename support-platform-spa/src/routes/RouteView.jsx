import React from 'react';
import { Switch } from 'react-router-dom';

import ClientRoute from '../_guards/ClientRoute';
import EmployeeRoute from '../_guards/EmployeeRoute';
import UserRoute from '../_guards/UserRoute';
import UnauthorizedRoute from '../_guards/UnauthorizedRoute';

import AuthService from '../_services/AuthService';

import AccountCreated from '../views/register/account created/AccountCreated';
import ConfirmationInProgress from '../views/register/confirm messages/confirmation in progress/ConfirmationInProgress';
import ConfirmationMessage from '../views/register/confirm messages/confirmation message/ConfirmationMessage';
import LoginForm from '../views/login/login form/LoginForm';
import RegisterForm from '../views/register/register form/RegisterForm';
import Reports from '../views/reports/Reports';
import CreateReport from '../views/reports/create report/CreateReport';


const RouterView = () => {
  const isUserAuthenticated = () => {
    const authService = new AuthService();
    return authService.isUserLoggedIn();
  }

  const getUsersRole = () => {
    const authService = new AuthService();
    return authService.getDecodedToken().role;
  }

  return (
    <Switch>
      <UnauthorizedRoute path="/login" component={LoginForm} authCondition={isUserAuthenticated} />
      <UnauthorizedRoute path="/register" component={RegisterForm} authCondition={isUserAuthenticated} />
      <UnauthorizedRoute path="/confirmMessage" component={ConfirmationMessage} authCondition={isUserAuthenticated} />
      <UnauthorizedRoute path="/confirmInProgress" component={ConfirmationInProgress} authCondition={isUserAuthenticated} />
      <UnauthorizedRoute path="/accountCreated" component={AccountCreated} authCondition={isUserAuthenticated} />

      <UserRoute path="/" exact component={Reports} isAuthenticated={isUserAuthenticated} />
      <UserRoute path="/reports" exact component={Reports} isAuthenticated={isUserAuthenticated} />

      <ClientRoute path="/reports/create" exact component={CreateReport} isAuthenticated={isUserAuthenticated} role={getUsersRole} />
      {/* <ClientRoute path="/dashboard" component={Dashboard} isAuthenticated={isUserAuthenticated} role={getUsersRole} /> */}
      {/* <ClientRoute path="/reports" component={Dashboard} isAuthenticated={isUserAuthenticated} role={getUsersRole} />
      <ClientRoute path="/reports/create" component={Dashboard} isAuthenticated={isUserAuthenticated} role={getUsersRole} />

      <EmployeeRoute path="/reports" component={LoginForm} isAuthenticated={isUserAuthenticated} role={getUsersRole} /> */}
    </Switch>
  );
}

export default RouterView;