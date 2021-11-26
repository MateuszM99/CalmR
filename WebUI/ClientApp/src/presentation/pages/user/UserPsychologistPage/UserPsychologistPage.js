import React from 'react'
import Navbar from '../../../components/Navbar/Navbar'
import Sidebar from '../../../components/user/Sidebar/Sidebar'
import UserPsychologist from '../../../components/user/UserPsychologist/UserPsychologist'

function UserPsychologistPage() {
    return (
        <div>
            <Navbar></Navbar>
            <Sidebar></Sidebar>
            <UserPsychologist></UserPsychologist>
        </div>
    )
}

export default UserPsychologistPage
