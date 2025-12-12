import { Routes } from '@angular/router';
import { LoginComponent } from './components/auth/login/login.component';
import { HomePageComponent } from './components/home-page/home-page.component';
import { SignupComponent } from './components/auth/signup/signup.component';
import { guestGuard } from './guards/guest/guest-guard';
import { authGuard } from './guards/auth/auth-guard';
import { CodeOptimizerComponent } from './components/code-optimizer/code-optimizer.component';

export const routes: Routes = [
    {path:'',redirectTo:'login',pathMatch:'full'},
  {
    path: 'login',
    component: LoginComponent,
    pathMatch:'full',
    canActivate:[guestGuard]
  },
  {
    path: 'signup',
    component: SignupComponent,
    pathMatch:'full',
    canActivate:[guestGuard]
  },
  {
    path:'home',
    component:HomePageComponent,
    pathMatch:'full',
    canActivate:[authGuard]
  },
  {
    path:'code-optimizer',
    component:CodeOptimizerComponent,
    pathMatch:'full',
    canActivate:[authGuard]
  }
];
