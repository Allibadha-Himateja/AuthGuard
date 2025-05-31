import { Routes } from '@angular/router';
import { UserComponent } from './user/user.component';
import { RegistrationComponent } from './user/registration/registration.component';
import { LoginComponent } from './user/login/login.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { authGuard } from './shared/auth.guard';
import { AdminOnlyComponent } from './authorizeDemo/admin-only/admin-only.component';
import { AdminOrTeacherComponent } from './authorizeDemo/admin-or-teacher/admin-or-teacher.component';
import { ApplyMaternityLeaveComponent } from './authorizeDemo/apply-maternity-leave/apply-maternity-leave.component';
import { LibraryMembersOnlyComponent } from './authorizeDemo/library-members-only/library-members-only.component';

import { MainLayoutComponent } from './layouts/main-layout/main-layout.component';
import { ForbidddenComponent } from './forbiddden/forbiddden.component';
import { Under10FemalesComponent } from './authorizeDemo/under10-females/under10-females.component';

export const routes: Routes = [
    {path:'',redirectTo:'/SignIn',pathMatch:'full'},
    {path:'',component:UserComponent,
        children:[{path:'SignUp',component:RegistrationComponent},
            {path:'SignIn',component:LoginComponent}
        ]
    },
    {path:'',component:MainLayoutComponent,canActivate:[authGuard],
        canActivateChild:[authGuard],
        children:[
            {path:'forbidden',component:ForbidddenComponent},
            {path:"dashboard",component:DashboardComponent},
            {path:"AdminOnly",component:AdminOnlyComponent,
                data:{claimReq:(c:any)=>c.role=="Admin"}
            },
            {path:"AdminOrTeacher",component:AdminOrTeacherComponent,
                data:{claimsReq:(c:any)=>c.role=="Admin" || c.role=="Teacher"}
            },
            {path:"ApplyMaternityleave",component:ApplyMaternityLeaveComponent},
            {path:"LibraryMembersOnly",component:LibraryMembersOnlyComponent},
            {path:"Under10Females",component:Under10FemalesComponent}
        ]
    }
];
