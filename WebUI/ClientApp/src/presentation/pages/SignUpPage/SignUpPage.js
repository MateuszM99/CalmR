import React from 'react'
import Navbar from '../../components/Navbar/Navbar'
import SignUp from '../../components/account/SignUp/SignUp'
import './style.scss'

function SignUpPage() {
    return (
        <div>
            <Navbar></Navbar>
            <div className="signUp__form__container">
                <SignUp></SignUp>
            </div>
        </div>
    )
}

export default SignUpPage
