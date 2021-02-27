import { useEffect } from 'react';
import { BrowserRouter as Router } from 'react-router-dom';
import { useSelector } from 'react-redux';

import AuthService from './_services/AuthService';

import Navigation from './layout/navigation/Navigation';
import RouteView from './layout/route view/RouteView';
import Footer from './layout/footer/Footer';

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
          <Footer />
        </Router>
    </div>
  );
}

export default App;
