import React from 'react';
import { Switch } from 'react-router-dom';

import AuthorizedRoute from '../../_guards/AuthorizedRoute';
import UnauthorizedRoute from '../../_guards/UnauthorizedRoute';

import AuthService from '../../_services/AuthService';

import AccountCreated from '../../views/register/account created/AccountCreated';
import ConfirmationInProgress from '../../views/register/confirm messages/confirmation in progress/ConfirmationInProgress';
import ConfirmationMessage from '../../views/register/confirm messages/confirmation message/ConfirmationMessage';
import Dashboard from '../../views/dashboard/Dashboard';
import LoginForm from '../../views/login/login form/LoginForm';
import RegisterForm from '../../views/register/register form/RegisterForm';


const RouterView = () => {
  const userLoggedIn = () => {
    const authService = new AuthService();
    return authService.isUserLoggedIn();
  }

  return (
    <Switch>
      <UnauthorizedRoute path="/login" component={LoginForm} authCondition={userLoggedIn} />
      <UnauthorizedRoute path="/register" component={RegisterForm} authCondition={userLoggedIn} />
      <UnauthorizedRoute path="/confirmMessage" component={ConfirmationMessage} authCondition={userLoggedIn} />
      <UnauthorizedRoute path="/confirmInProgress" component={ConfirmationInProgress} authCondition={userLoggedIn} />
      <UnauthorizedRoute path="/accountCreated" component={AccountCreated} authCondition={userLoggedIn} />

      <AuthorizedRoute path="/" exact component={Dashboard} authCondition={userLoggedIn} />
      <AuthorizedRoute path="/dashboard" component={Dashboard} authCondition={userLoggedIn} />
    </Switch>
  );
}

export default RouterView;