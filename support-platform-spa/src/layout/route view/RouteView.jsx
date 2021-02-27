import React from 'react';
import { Switch } from 'react-router-dom';

import ClientRoute from '../../_route guards/ClientRoute';
import UserRoute from '../../_route guards/UserRoute';
import UnauthorizedRoute from '../../_route guards/UnauthorizedRoute';

import AuthService from '../../_services/AuthService';

import AuthError from '../../views/error/auth error/AuthError';
import AccountCreated from '../../views/register/account created/AccountCreated';
import ConfirmationInProgress from '../../views/register/confirm messages/confirmation in progress/ConfirmationInProgress';
import ConfirmationMessage from '../../views/register/confirm messages/confirmation message/ConfirmationMessage';
import CreateReport from '../../views/reports/create report/CreateReport';
import LoginForm from '../../views/login/login form/LoginForm';
import RegisterForm from '../../views/register/register form/RegisterForm';
import ReportsOverview from '../../views/reports/reports overview/ReportsOverview';
import ReportDetails from '../../views/reports/report details/ReportDetails';
import ServerError from '../../views/error/server error/ServerError';


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
      <UnauthorizedRoute path="/login" component={LoginForm} isAuthenticated={isUserAuthenticated} />
      <UnauthorizedRoute path="/register" component={RegisterForm} isAuthenticated={isUserAuthenticated} />
      <UnauthorizedRoute path="/confirmMessage" component={ConfirmationMessage} isAuthenticated={isUserAuthenticated} />
      <UnauthorizedRoute path="/confirmInProgress" component={ConfirmationInProgress} isAuthenticated={isUserAuthenticated} />
      <UnauthorizedRoute path="/accountCreated" component={AccountCreated} isAuthenticated={isUserAuthenticated} />

      <UserRoute path="/" exact component={ReportsOverview} isAuthenticated={isUserAuthenticated} />
      <UserRoute path="/reports" exact component={ReportsOverview} isAuthenticated={isUserAuthenticated} />
      <UserRoute path="/reports/details/:id" exact component={ReportDetails} isAuthenticated={isUserAuthenticated} />
      <UserRoute path="/error-auth" component={AuthError} isAuthenticated={isUserAuthenticated} />
      <UserRoute path="/error-server" component={ServerError} isAuthenticated={isUserAuthenticated} />

      <ClientRoute path="/reports/create" exact component={CreateReport} isAuthenticated={isUserAuthenticated} role={getUsersRole} />

      <UnauthorizedRoute path="/tmp" component={ConfirmationMessage} isAuthenticated={() => false} />


      {/* default route */}
      <UserRoute component={AuthError} isAuthenticated={isUserAuthenticated} />
    </Switch>
  );
}

export default RouterView;