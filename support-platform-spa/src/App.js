import Navigation from './layout/navigation/Navigation';
import RouterView from './layout/router view/RouterView';

import './App.css';
import { BrowserRouter as Router } from 'react-router-dom';

function App() {
  return (
    <div className="App">
      <Router>
        <Navigation />
        <RouterView />
      </Router>
    </div>
  );
}

export default App;
