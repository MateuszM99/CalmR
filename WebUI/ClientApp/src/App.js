import React, { Component } from 'react';
import { BrowserRouter as Router,
  Switch,
  Route,
  Link } from 'react-router-dom';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { Counter } from './components/Counter';

import './custom.css'
import SignIn from './pages/SignIn/SignIn';

function App() {

  // if(localStorage.getItem('userData') != null){
  // const userData = JSON.parse(localStorage.getItem('userData'));

  //   axios.interceptors.request.use(
  //     config => {
  //       config.headers.authorization = `Bearer ${userData.token} `;
  //       return config;
  //     },
  //     error => {
  //       return Promise.reject(error);
  //     }
  //   );
  // }
  return (
    <Router>
        <div className="App">
          <Switch>
            <Route path="/signIn">
                <SignIn/>
            </Route>
            <Route path="/">
              <Counter/>
            </Route>
          </Switch>
        </div>
    </Router>
  );
}

export default App;
