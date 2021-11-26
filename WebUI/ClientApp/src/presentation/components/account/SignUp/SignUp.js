import React, { Component,useState, useEffect } from 'react'
import {Formik,Form, yupToFormErrors,Field} from 'formik'
import * as Yup from 'yup'
import {Link, Redirect} from 'react-router-dom'
import './style.scss'
import { signUpRequest } from '../../../../infrastructure/services/api/auth/AuthRequests'


const phoneRegExp = /^((\\+[1-9]{1,4}[ \\-]*)|(\\([0-9]{2,3}\\)[ \\-]*)|([0-9]{2,4})[ \\-]*)*?[0-9]{3,4}?[ \\-]*[0-9]{3,4}?$/
const passwordRegExp = /^(.{0,7}|[^0-9]*|[^A-Z]*|[a-zA-Z0-9]*)$/

function SignUp() {
    const [didSignUp,setDidSignUp] = useState(false);

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

    if(didSignUp){
        return (
            <div className="signup__container">
                <div className="signup__container__box">
                    <p>You successfully signed up, now go to your email and confirm your account</p>
                </div>
            </div>
        )
    }
    if(!didSignUp){
        return (
            <div className="signup__container">    
                <Formik
                    initialValues={{
                        username : '',
                        email : '',
                        password : '',
                        confirmPassword: '',
                    }}
                    validationSchema = {Yup.object({
                        username : Yup.string()
                            .required('Username is required'),
                        email : Yup.string()
                            .email('Invalid email')
                            .required('Email is required'),
                        password : Yup.string()
                            //.matches(passwordRegExp,'Password has to be at least 8 characters long,contain 1 special sign and 1 uppercase letter')
                            .required('Password is required'),
                        confirmPassword: Yup.string()
                            .required('You must confirm your password')
                    })}

                    onSubmit = {async (values,{setSubmitting,setStatus,resetForm}) => {
                        if(values){
                            try{
                                let response = await signUpRequest(values);
                                setDidSignUp(true);
                                setSubmitting(false);
                                resetForm();
                            } catch(err){
                                setSubmitting(false);
                                resetForm();
                                setStatus({
                                    errorMessage : err.response.data.title
                                });
                            }                                                                                                                                                                                        
                        }  
                    }} 
                >
                    {({ errors, touched,status,isSubmitting }) => (
                    <Form>  
                        <div className="signup__container__box">
                            <h3>Sign up</h3>       
                            <div className="signup__container__box__input">
                            <label>Username</label>
                            <Field type="text" placeholder="Enter your username" name="username"></Field>  
                            {errors.username && touched.username ? <div className="signup-validation">{errors.username}</div> : null}              
                            </div>
                            <div className="signup__container__box__input">
                            <label>E-mail</label>
                            <Field type="text" placeholder="Enter your email" name="email"></Field>
                            {errors.email && touched.email ? <div className="signup-validation">{errors.email}</div> : null}
                            </div>
                            <div className="signup__container__box__input">
                            <label>Password</label>
                            <Field type="password" placeholder="Enter your password" name="password"></Field>
                            {errors.password && touched.password ? <div className="signup-validation">{errors.password}</div> : null}
                            </div>
                            <div className="signup__container__box__input">
                            <label>Confirm Password</label>
                            <Field type="password" placeholder="Enter your password" name="confirmPassword"></Field>
                            {errors.confirmPassword && touched.confirmPassword ? <div className="signup-validation">{errors.confirmPassword}</div> : null}
                            </div>
                            <button className="signup__container__box__button" type="submit">{isSubmitting ? 'Signin up ...' : 'Sign up'}</button>
                            {status && status.errorMessage ? (
                                    <div className="signup-validation">{status.errorMessage}</div>
                                ) : null}
                            <Link className="signup__container__box__signin" to="/signIn">Already have an account? Login</Link>
                        </div>
                    </Form>
                    )}
                </Formik>
            </div>
        )
    }
}

export default SignUp