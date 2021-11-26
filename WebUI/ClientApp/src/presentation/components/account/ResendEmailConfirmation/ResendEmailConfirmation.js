import React, { useState } from 'react';
import {Formik,Form,Field} from 'formik';
import * as Yup from 'yup';
import './style.scss';
import { resendConfirmationEmailRequest } from '../../../../infrastructure/services/api/auth/AuthRequests';

function ResendEmailConfirmation() {
    const [isSent,setIsSent] = useState(false);
    
    if(isSent){
        return (
            <div className="signup__container">
                <div className="signup__container__box">
                    <p>Successfully sent confirmation email, now go to your email and confirm your account</p>
                </div>
            </div>
        )
    }

    return (
        <div>
            <Formik
                initialValues={{
                    email : '',
                 }}
                 validationSchema = {Yup.object({
                     email : Yup.string()
                         .required('Email is required')
                         .email('Email is not valid'),
                 })}
                 
                 onSubmit = {async (values,{setSubmitting, setStatus, resetForm}) => {
                    if(values){
                        try{
                            await resendConfirmationEmailRequest(values);
                            setIsSent(true);
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
                <div className="reset__container__box">                           
                    <h3>Resend email confirmation</h3>             
                    <div className="reset__container__box__input">
                    <label>Email</label>
                    <Field type="text" placeholder="Enter your username" name="email"></Field>
                    {errors.email && touched.email ? <div className="reset-validation">{errors.email}</div> : null}
                    </div>                           
                    <button className="reset__container__box__button">{isSubmitting ? 'Sending ...' : 'Send confirmation link'}</button> 
                    {status && status.errorMessage ? (
                        <div className="reset-validation">{status.errorMessage}</div>
                    ) : null}            
                    </div>
                </Form>
                )}
            </Formik>
        </div>
    )
}

export default ResendEmailConfirmation
