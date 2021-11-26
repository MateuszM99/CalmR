import React, { useEffect, useState} from 'react';
import { Route, Redirect } from 'react-router-dom'
import jwt_decode from "jwt-decode";
import * as worker from './registerServiceWorker';

const PrivateRoute = ({ component: Component, ...rest }) => {
  const [isAuthenticated, setIsAuthenticated] = useState(null);
  
  useEffect(() => {
      let user = JSON.parse(localStorage.getItem('userData'));
      console.log(user);
        if(user != null){          
            let token = user.token;
            let tokenExpiration = jwt_decode<MyToken>(token).exp;
            let dateNow = new Date();
            console.log("User found!")
            console.log(user)
            if (tokenExpiration < dateNow.getTime() / 1000) {
                localStorage.setItem('userData', null) 
                setIsAuthenticated(false)
            } 
            else 
            {              
                setIsAuthenticated(true); 
            }
        } 
        else 
        {     
          console.log("User not found")
          setIsAuthenticated(false)
        }
  },[isAuthenticated])

    if (isAuthenticated == null) {
        return (
            <div style={{ color: "#FFFFFF" }}>Loading...</div>
            )
    }
    return (
    <Route {...rest} render={props =>
      isAuthenticated  ? (
        <Component {...props} />
      ) : (
        <Redirect to='/signIn'/>
      )
    }
    />
  );
};

export default PrivateRoute;