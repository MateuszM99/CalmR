import React from 'react'
import Navbar from '../../../components/common/Navbar/Navbar'
import Sidebar from '../../../components/user/Sidebar/Sidebar'
import UserPsychologist from '../../../components/user/UserPsychologist/UserPsychologist'
import {PageContainer} from '../../../../application/common/PageContainer/PageContainer'

function UserPsychologistPage() {
    return (
        <PageContainer>
            <Navbar></Navbar>
            <Sidebar></Sidebar>
            <UserPsychologist></UserPsychologist>
        </PageContainer>
    )
}

export default UserPsychologistPage
