import React, { Component } from 'react';
import { BrowserRouter as Router,
  Switch,
  Route,
  Link } from 'react-router-dom';
import axios from 'axios';
import './custom.css';
import Navbar from './presentation/components/Navbar/Navbar';
import Home from './presentation/components/Home/Home';
import SignInPage from './presentation/pages/SignInPage/SignInPage';
import SignUpPage from './presentation/pages/SignUpPage/SignUpPage';
import RequestPasswordResetPage from './presentation/pages/RequestPasswordResetPage/RequestPasswordResetPage';
import PasswordResetPage from './presentation/pages/PasswordResetPage/PasswordResetPage';
import ConfirmEmailPage from './presentation/pages/ConfirmEmailPage/ConfirmEmailPage';
import ResendConfirmEmailPage from './presentation/pages/ResendConfirmEmailPage/ResendConfirmEmailPage';
import UserHomePage from './presentation/pages/user/UserHomePage/UserHomePage';
import UserPsychologistPage from './presentation/pages/user/UserPsychologistPage/UserPsychologistPage';
import UserAppointmentsPage from './presentation/pages/user/UserAppointmentsPage/UserAppointmentsPage';
import UserMeetingsPage from './presentation/pages/user/UserMeetingsPage/UserMeetingsPage';
import UserNotesPage from './presentation/pages/user/UserNotesPage/UserNotesPage';

function App() {

  if(localStorage.getItem('userData') != null){
  const userData = JSON.parse(localStorage.getItem('userData'));

    axios.interceptors.request.use(
      config => {
        config.headers.authorization = `Bearer ${userData.token} `;
        return config;
      },
      error => {
        return Promise.reject(error);
      }
    );
  }
  return (
    <Router>
        <div className="App">
          <Switch>
            <Route path="/signin">
                <SignInPage></SignInPage>
            </Route>
            <Route path="/signup">
                <SignUpPage></SignUpPage>
            </Route>
            <Route path="/account/request-reset">
              <RequestPasswordResetPage></RequestPasswordResetPage>
            </Route>
            <Route path="/account/reset">
              <PasswordResetPage></PasswordResetPage>
            </Route>
            <Route path="/account/confirm">
              <ConfirmEmailPage></ConfirmEmailPage>
            </Route>
            <Route path="/account/resend-confirm">
              <ResendConfirmEmailPage></ResendConfirmEmailPage>
            </Route>
            <Route path="/app/home">
                <UserHomePage></UserHomePage>
            </Route>
            <Route path="/app/psychologist">
                <UserPsychologistPage></UserPsychologistPage>
            </Route>
            <Route path="/app/appointments">
                <UserAppointmentsPage></UserAppointmentsPage>
            </Route>
            <Route path="/app/meetings">
                <UserMeetingsPage></UserMeetingsPage>
            </Route>
            <Route path="/app/notes">
                <UserNotesPage></UserNotesPage>
            </Route>
            <Route path="/">
              <Navbar></Navbar>
              <Home></Home>
            </Route>
          </Switch>
        </div>
    </Router>
  );
}

export default App;
