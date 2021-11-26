import React from 'react'
import ResendEmailConfirmation from '../../components/account/ResendEmailConfirmation/ResendEmailConfirmation'
import Navbar from '../../components/Navbar/Navbar'

function ResendConfirmEmailPage() {
    return (
        <div>
            <Navbar></Navbar>
            <div className="reset__form__container">
                <ResendEmailConfirmation></ResendEmailConfirmation>
            </div>
        </div>
    )
}

export default ResendConfirmEmailPage
