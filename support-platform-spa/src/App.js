import { BrowserRouter as Router } from 'react-router-dom';
import {useSelector} from 'react-redux';

import Navigation from './layout/navigation/Navigation';
import RouteView from './routes/RouteView';

import './App.css';
import { useEffect } from 'react';
import AuthService from './_services/AuthService';

function App() {
  const decodedToken = useSelector(store => store.authStates.decodedToken);

  useEffect(() => {
    const authService = new AuthService();
    authService.initializeStore();
  },[])

  return (
    <div className="App">
        <Router>
          <Navigation decodedToken={decodedToken} />
          <RouteView />
        </Router>
    </div>
  );
}

export default App;
