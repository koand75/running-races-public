import { Routes } from '@angular/router';
import { RaceListComponent } from './components/race-list/race-list';
import { RaceFormComponent } from './components/race-form/race-form';
import { LoginComponent } from './components/login/login';
import { adminGuard } from './guards/admin.guard';
import { PublicLayoutComponent } from './components/public-layout/public-layout';
import { AdminLayoutComponent } from './components/admin-layout/admin-layout';
import { PaceCalculator } from './features/pace-calculator/pace-calculator';

export const routes: Routes = [
  { path: '', redirectTo: '/races', pathMatch: 'full' },

  // PUBLIC ROUTES 
  {
    path: '',
    component: PublicLayoutComponent,
    children: [
      { path: 'races', component: RaceListComponent },
      { path: 'login', component: LoginComponent },
      {
        path: 'relay-planner',
        loadChildren: () => import('./features/relay-planner/relay-planner.routes').then(m => m.UB_ROUTES)
      },
      { path: 'pace-calculator', component: PaceCalculator }
    ]
  },

  //  ADMIN ROUTES 
  {
    path: 'admin',
    component: AdminLayoutComponent,
    canActivate: [adminGuard],
    children: [
      { path: 'races', component: RaceListComponent },
      { path: 'races/new', component: RaceFormComponent },
      { path: 'races/:id/edit', component: RaceFormComponent }
    ]
  }
];