import { Routes } from '@angular/router';
import { adminGuard } from '../../guards/admin.guard';
import { authGuard } from '../../guards/auth-guard';
import { PublicLayoutComponent } from '../../components/public-layout/public-layout';

export const UB_ROUTES: Routes = [
  {
    path: 'categories/:categoryId/sections',
    loadComponent: () =>
      import('./components/section-list/section-list')
        .then(m => m.SectionListComponent)
  }
  ,
  {
    path: 'map', loadComponent: () =>
      import('./components/map/map')
        .then(m => m.MapComponent)
  },
  // user
  {
    path: 'teams',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./components/team-list/team-list').then(m => m.TeamListComponent)
  },
  {
    path: 'teams/:id',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./components/team-detail/team-detail').then(m => m.TeamDetailComponent)
  },
  {
    path: 'teams/:id/edit',
    canActivate: [authGuard],
    loadComponent: () => import('./components/team-edit/team-edit').then(m => m.TeamEdit)
  },
  {
    path: 'categories/:categoryId/teams/:id/plan',
    canActivate: [authGuard],
    loadComponent: () => import('./components/planner/planner').then(m => m.PlannerComponent)
  },
  {
    path: 'categories/:categoryId/teams/:id/map',
    canActivate: [authGuard],
    loadComponent: () => import('./components/team-map/team-map').then(m => m.TeamMapComponent)
  },
  //admin
  {
    path: 'categories/:categoryId/waypoints',
    canActivate: [adminGuard],
    loadComponent: () => import('./components/waypoints/waypoints').then(m => m.Waypoints)
  },
  {
    path: 'categories/:categoryId/sections/import',
    canActivate: [adminGuard],
    loadComponent: () => import('./components/section-import/section-import').then(m => m.SectionImport)
  },
  {
    path: 'categories/:categoryId/teams/:id/plan',
    canActivate: [authGuard],
    loadComponent: () => import('./components/planner/planner').then(m => m.PlannerComponent)
  }
];