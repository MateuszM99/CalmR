import React,{useState,useEffect} from 'react';
import {Formik,Form,Field} from 'formik';
import * as Yup from 'yup'
import {Link,withRouter,useHistory, Redirect} from "react-router-dom"
import { signInRequest } from '../../../../infrastructure/services/api/auth/AuthRequests';
import './style.scss'


function SignIn() {
    const history = useHistory();
    const [isSignedIn,setIsSignedIn] = useState(false);

    const handleLoggedIn = () => {
        if(localStorage.getItem("userData") != null){
            setIsSignedIn(true);
        }
    }

    useEffect(() => {    
        handleLoggedIn();
    });

    if(isSignedIn){
        return <Redirect to="/app"/>
    }

    return (  
        <div>
            <Formik
                initialValues={{
                    username : '',
                    password : '',
                 }}
                 validationSchema = {Yup.object({
                     username : Yup.string()
                         .required('Username is required'),
                     password : Yup.string()
                         .required('Password is required'),
                 })}
                 
                 onSubmit = {async (values,{setSubmitting, setStatus, resetForm}) => {
                    if(values){
                        try{
                            let response = await signInRequest(values);
                            localStorage.setItem('userData',JSON.stringify(response.data));
                            history.push('/app')
                        } catch(err){
                            console.log(err.response.data.title);
                            setSubmitting(false);
                            resetForm();
                            setStatus({
                                errorMessage : err.response.data.title
                            });
                        }                                                                                                                                                                                        
                    }      
                }}
            >
                {({ errors, touched,isSubmitting,status}) => (
                <Form>
                <div className="signin__container__box">                           
                    <h3>Sign in</h3>             
                    <div className="signin__container__box__input">
                    <label>Username</label>
                    <Field type="text" placeholder="Enter your username" name="username"></Field>
                    {errors.username && touched.username ? <div className="signin-validation">{errors.username}</div> : null}
                    </div>
                    <div className="signin__container__box__input">
                    <label>Password</label>
                    <Field type="password" placeholder="Enter your password" name="password"></Field>
                    {errors.password && touched.password ? <div className="signin-validation">{errors.password}</div> : null}
                    </div>                           
                    <button className="signin__container__box__button">{isSubmitting ? 'Login in ...' : 'Log in'}</button> 
                    {status && status.errorMessage ? (
                        <div className="signin-validation">{status.errorMessage}</div>
                    ) : null}            
                    <Link className="signin__container__box__signup" to="/signup">Don't have account yet? Sign up</Link> 
                    <Link className="signin__container__box__signup" to="/account/requestReset">Forgot your password ?</Link> 
                    </div>
                </Form>
                )}
            </Formik>
        </div>
    );
}

export default SignIn;