import React from 'react'
import Navbar from '../../components/Navbar/Navbar'
import RequestPasswordReset from '../../components/account/RequestPasswordReset/RequestPasswordReset'
import './style.scss'

function RequestPasswordResetPage() {
    return (
        <div>
             <Navbar></Navbar>
            <div className="reset__form__container">
                <RequestPasswordReset></RequestPasswordReset>
            </div>
        </div>
    )
}

export default RequestPasswordResetPage
