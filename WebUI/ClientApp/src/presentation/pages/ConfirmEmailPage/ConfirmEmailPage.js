import React from 'react'
import Navbar from '../../components/Navbar/Navbar'
import ConfirmEmail from '../../components/account/ConfirmEmail/ConfirmEmail'

function ConfirmEmailPage() {
    return (
        <div>
            <Navbar></Navbar>
            <div className="reset__form__container">
                <ConfirmEmail></ConfirmEmail>
            </div>
        </div>
    )
}

export default ConfirmEmailPage
