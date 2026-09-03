import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { WayPoint as WayPointModel } from '../models/relay-planner.models';
import { map } from 'rxjs/operators';
import { PagedResult } from '../../../models/paged-result.model';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class WayPoint {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/waypoint`;

  getAll(): Observable<WayPointModel[]> {
    return this.http.get<PagedResult<WayPointModel>>(`${this.apiUrl}?pageSize=1000`).pipe(
      map(result => result.items)
    );
  }

  create(wayPoint: WayPointModel): Observable<WayPointModel> {
    return this.http.post<WayPointModel>(this.apiUrl, wayPoint);
  }

  update(id: number, wayPoint: WayPointModel): Observable<WayPointModel> {
    return this.http.put<WayPointModel>(`${this.apiUrl}/${id}`, wayPoint);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}