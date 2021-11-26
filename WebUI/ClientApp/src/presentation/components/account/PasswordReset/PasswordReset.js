import React,{useState} from 'react'
import {Formik,Form,Field} from 'formik';
import { useParams, useLocation } from 'react-router';
import * as Yup from 'yup'
import './style.scss'
import { passwordResetRequest } from '../../../../infrastructure/services/api/auth/AuthRequests';

const phoneRegExp = /^((\\+[1-9]{1,4}[ \\-]*)|(\\([0-9]{2,3}\\)[ \\-]*)|([0-9]{2,4})[ \\-]*)*?[0-9]{3,4}?[ \\-]*[0-9]{3,4}?$/
const passwordRegExp = /^(.{0,7}|[^0-9]*|[^A-Z]*|[a-zA-Z0-9]*)$/

function useQuery() {
    const { search } = useLocation();
  
    return React.useMemo(() => new URLSearchParams(search), [search]);
  }

function PasswordReset() {
    const query = useQuery();
    const [isReset,setIsReset] = useState(false);
    
    if(isReset){
        return (
            <div className="signup__container">
                <div className="signup__container__box">
                    <p>Successfully reset password, you can now login to your account with your new password</p>
                </div>
            </div>
        )
    }

    return (
        <div>
            <Formik
                initialValues={{
                    code : query.get("code"),
                    email : query.get("email"),
                    password : '',
                    confirmPassword: '',
                 }}
                 validationSchema = {Yup.object({
                    code : Yup.string()
                        .required('Reset code is missing'),
                    email : Yup.string()
                        .email('Email is invalid')
                        .required('Email is missing'),
                    password : Yup.string()
                        //.matches(passwordRegExp,'Password has to be at least 8 characters long,contain 1 special sign and 1 uppercase letter')
                        .required('Password is required'),
                    confirmPassword: Yup.string()
                        .required('Password confirmation is required')
                 })}
                 
                 onSubmit = {async (values,{setSubmitting, setStatus, resetForm}) => {

                    if(values){
                        try{
                            console.log('is here');
                            await passwordResetRequest(values);
                            setIsReset(true);
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
                    <h3>Reset Password</h3>             
                    <div className="reset__container__box__input">
                    <label>New password</label>
                    <Field type="text" placeholder="Enter new password" name="password"></Field>
                    {errors.password && touched.password ? <div className="reset-validation">{errors.password}</div> : null}
                    </div>   
                    <div className="reset__container__box__input">
                    <label>Confirm new password</label>
                    <Field type="text" placeholder="Enter your new password again" name="confirmPassword"></Field>
                    {errors.confirmPassword && touched.confirmPassword ? <div className="reset-validation">{errors.confirmPassword}</div> : null}
                    </div>                        
                    <button className="reset__container__box__button">{isSubmitting ? 'Reseting password ...' : 'Reset password'}</button> 
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

export default PasswordReset
