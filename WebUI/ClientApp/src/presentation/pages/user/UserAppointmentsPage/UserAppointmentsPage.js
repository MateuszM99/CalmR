import React from 'react'
import Navbar from '../../../components/common/Navbar/Navbar'
import Sidebar from '../../../components/user/Sidebar/Sidebar'
import UserAppointments from '../../../components/user/UserAppointments/UserAppointments'
import {PageContainer} from '../../../../application/common/PageContainer/PageContainer'

function UserAppointmentsPage() {
    return (
        <PageContainer>
            <Navbar></Navbar>
            <Sidebar></Sidebar>
            <UserAppointments></UserAppointments>
        </PageContainer>
    )
}

export default UserAppointmentsPage
