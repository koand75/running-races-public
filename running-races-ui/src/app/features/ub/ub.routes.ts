import { Routes } from '@angular/router';

export const UB_ROUTES: Routes = [
  {
    path: 'sections',
    loadComponent: () => import('./components/section-list/section-list').then(m => m.SectionListComponent)
  },
  {
    path: 'teams',
    loadComponent: () => import('./components/team-list/team-list').then(m => m.TeamListComponent)
  },
  {
    path: 'teams/:id',
    loadComponent: () => import('./components/team-detail/team-detail').then(m => m.TeamDetailComponent)
  },
  {
    path: 'teams/:id/plan',
    loadComponent: () => import('./components/planner/planner').then(m => m.PlannerComponent)
  },
  {
    path: 'teams/:id/edit',
    loadComponent: () => import('./components/team-edit/team-edit').then(m => m.TeamEdit)
  },
  {
    path: 'waypoints',
    loadComponent: () => import('./components/waypoints/waypoints').then(m => m.Waypoints)
  },
  {
    path: 'map', loadComponent: () => import('./components/map/map').then(m => m.MapComponent)
  }
];