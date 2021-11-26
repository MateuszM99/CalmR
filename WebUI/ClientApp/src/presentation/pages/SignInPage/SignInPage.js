import React from 'react'
import Navbar from '../../components/Navbar/Navbar'
import SignIn from '../../components/account/SignIn/SignIn'
import './style.scss'

function SignInPage() {
    return (
        <div>
            <Navbar></Navbar>
            <div className="signIn__form__container">
                <SignIn></SignIn>
            </div>
        </div>
    )
}

export default SignInPage
