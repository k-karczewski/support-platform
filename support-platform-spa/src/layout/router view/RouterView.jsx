import React from 'react';
import { Switch, Route } from 'react-router-dom';

import Register from '../../views/register/Register';
import AccountCreatedMessage from '../../views/account created/AccountCreatedMessage';
import ConfirmAccount from '../../views/confirm account/ConfirmAccount';

const RouterView = () => {
  return (
    <Switch>
      {/* <Route path="/" exact component={Home} /> */}
      <Route path="/register" component={Register} />
      <Route path="/accountCreated" component={AccountCreatedMessage} />
      <Route path="/confirmAccount" component={ConfirmAccount} />
      {/* <Route path="/login" component={Login} /> */}
    </Switch>
  );
}

export default RouterView;