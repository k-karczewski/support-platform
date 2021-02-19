import { BrowserRouter as Router } from 'react-router-dom';
import {useSelector} from 'react-redux';

import Navigation from './layout/navigation/Navigation';
import RouterView from './layout/router view/RouterView';

import './App.css';
import { useEffect } from 'react';
import AuthService from './_services/AuthService';

function App() {
  const userLoggedIn = useSelector(store => store.authStates.decodedToken) ? true : false;

  useEffect(() => {
    const authService = new AuthService();
    authService.initializeStore();
  },[])

  return (
    <div className="App">
        <Router>
          <Navigation userLoggedIn={userLoggedIn} />
          <RouterView />
        </Router>
    </div>
  );
}

export default App;
