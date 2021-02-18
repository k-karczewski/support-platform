import React from 'react';
import { Switch, Route } from 'react-router-dom';

import RegisterForm from '../../views/register/register form/RegisterForm';
import LoginForm from '../../views/login/login form/LoginForm';

import AccountCreated from '../../views/register/account created/AccountCreated';
import ConfirmationInProgress from '../../views/register/confirm messages/confirmation in progress/ConfirmationInProgress';
import ConfirmationMessage from '../../views/register/confirm messages/confirmation message/ConfirmationMessage';

const RouterView = () => {
  return (
    <Switch>
      {/* <Route path="/" exact component={Home} /> */}
      <Route path="/register" component={RegisterForm} />
      <Route path="/confirmMessage" component={ConfirmationMessage} />
      <Route path="/confirmInProgress" component={ConfirmationInProgress} />
      <Route path="/accountCreated" component={AccountCreated} />
      <Route path="/login" component={LoginForm} />
    </Switch>
  );
}

export default RouterView;