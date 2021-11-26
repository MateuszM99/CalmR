import React from 'react'
import Navbar from '../../components/Navbar/Navbar'
import PasswordReset from '../../components/account/PasswordReset/PasswordReset'
import './style.scss'

function PasswordResetPage() {
    return (
        <div>
            <Navbar></Navbar>
            <div className="reset__form__container">
                <PasswordReset></PasswordReset>
            </div>
        </div>
    )
}

export default PasswordResetPage
