import { Routes } from '@angular/router';
import { SignupClientComponent } from './auth/signup-client/signup-client.component';
import { SignupOperatorComponent } from './auth/signup-operator/signup-operator.component';
import { LandingComponent } from './auth/landing/landing.component';
import { AdminDashboardComponent } from './admin/admin-dashboard/admin-dashboard.component';
import { TransportOperatorsComponent } from './admin/transport-operators/transport-operators.component';
import { BusesComponent as AdminBusesComponent } from './admin/buses/buses.component';
import { OperatorDashboardComponent } from './operator/operator-dashboard/operator-dashboard.component';
import { BusesComponent as OperatorBusesComponent } from './operator/buses/buses.component';
import { AuthGuard } from './core/guards/auth.guard';
import { RoleGuard } from './core/guards/role.guard';

export const routes: Routes = [
  { path: '', component: LandingComponent },
  { path: 'signup-client', component: SignupClientComponent },
  { path: 'signup-operator', component: SignupOperatorComponent },
  { 
    path: 'trip-search', 
    component: LandingComponent,
    canActivate: [AuthGuard, RoleGuard],
    data: { role: 'Client' } 
  },
  { 
    path: 'operator-pending', 
    component: LandingComponent,
    canActivate: [AuthGuard, RoleGuard],
    data: { role: 'TransportOperator' } 
  },
  { 
    path: 'admin-dashboard', 
    component: AdminDashboardComponent,
    canActivate: [AuthGuard, RoleGuard],
    data: { role: 'Admin' },
    children: [
      { path: '', redirectTo: 'transport-operators', pathMatch: 'full' },
      { path: 'transport-operators', component: TransportOperatorsComponent },
      { path: 'buses', component: AdminBusesComponent }
    ]
  },
  { 
    path: 'operator-dashboard', 
    component: OperatorDashboardComponent,
    canActivate: [AuthGuard, RoleGuard],
    data: { role: 'TransportOperator' },
    children: [
      { path: '', redirectTo: 'buses', pathMatch: 'full' },
      { path: 'buses', component: OperatorBusesComponent }
    ]
  }
];