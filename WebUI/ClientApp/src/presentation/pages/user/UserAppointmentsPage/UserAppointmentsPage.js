import React from 'react'
import Navbar from '../../../components/common/Navbar/Navbar'
import Sidebar from '../../../components/user/Sidebar/Sidebar'
import UserAppointments from '../../../components/user/UserAppointments/UserAppointments'

function UserAppointmentsPage() {
    return (
        <div>
            <Navbar></Navbar>
            <Sidebar></Sidebar>
            <UserAppointments></UserAppointments>
        </div>
    )
}

export default UserAppointmentsPage
