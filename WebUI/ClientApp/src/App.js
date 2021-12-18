import React, { Component } from 'react';
import { BrowserRouter as Router,
  Switch,
  Route,
  Link } from 'react-router-dom';
import axios from 'axios';
import './custom.css';
import Navbar from './presentation/components/common/Navbar/Navbar';
import Home from './presentation/components/common/Home/Home';
import UserHomePage from './presentation/pages/user/UserHomePage/UserHomePage';
import UserPsychologistPage from './presentation/pages/user/UserPsychologistPage/UserPsychologistPage';
import UserAppointmentsPage from './presentation/pages/user/UserAppointmentsPage/UserAppointmentsPage';
import UserMeetingsPage from './presentation/pages/user/UserMeetingsPage/UserMeetingsPage';
import UserNotesPage from './presentation/pages/user/UserNotesPage/UserNotesPage';
import ChatPage from './presentation/pages/chat/ChatPage/ChatPage';
import SignInPage from './presentation/pages/account/SignInPage/SignInPage';
import SignUpPage from './presentation/pages/account/SignUpPage/SignUpPage';
import RequestPasswordResetPage from './presentation/pages/account/RequestPasswordResetPage/RequestPasswordResetPage';
import PasswordResetPage from './presentation/pages/account/PasswordResetPage/PasswordResetPage';
import ConfirmEmailPage from './presentation/pages/account/ConfirmEmailPage/ConfirmEmailPage';
import SignUpPsychologistPage from './presentation/pages/account/SignUpPsychologistPage/SignUpPsychologistPage';
import ResendConfirmEmailPage from './presentation/pages/account/ResendConfirmEmailPage/ResendConfirmEmailPage';

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
                <SignInPage/>
            </Route>
            <Route path="/signup">
                <SignUpPage/>
            </Route>
            <Route path="/signup-psychologist">
                <SignUpPsychologistPage/>
            </Route>
            <Route path="/account/request-reset">
              <RequestPasswordResetPage/>
            </Route>
            <Route path="/account/reset">
              <PasswordResetPage/>
            </Route>
            <Route path="/account/confirm">
              <ConfirmEmailPage/>
            </Route>
            <Route path="/account/resend-confirm">
              <ResendConfirmEmailPage />
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
            <Route path="/app/meeting">
                <UserMeetingsPage></UserMeetingsPage>
            </Route>
            <Route path="/app/notes">
                <UserNotesPage></UserNotesPage>
            </Route>
            <Route path="/app/chat/:conversationId">
                <ChatPage></ChatPage>
            </Route>
            <Route path="/">
              <Home></Home>
            </Route>
          </Switch>
        </div>
    </Router>
  );
}

export default App;
