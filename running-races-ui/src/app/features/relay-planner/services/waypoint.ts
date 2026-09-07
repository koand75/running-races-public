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
  private getApiUrl(raceId: string): string {
    return `${environment.apiUrl}/race/${raceId}/waypoint`;
  }

  getAll(raceId: string): Observable<WayPointModel[]> {
    return this.http.get<PagedResult<WayPointModel>>(`${this.getApiUrl(raceId)}?pageSize=1000`).pipe(
      map(result => result.items)
    );
  }

  create(raceId: string, wayPoint: WayPointModel): Observable<WayPointModel> {
    return this.http.post<WayPointModel>(this.getApiUrl(raceId), wayPoint);
  }

  update(raceId: string, id: number, wayPoint: WayPointModel): Observable<WayPointModel> {
    return this.http.put<WayPointModel>(`${this.getApiUrl(raceId)}/${id}`, wayPoint);
  }

  delete(raceId: string, id: number): Observable<void> {
    return this.http.delete<void>(`${this.getApiUrl(raceId)}/${id}`);
  }
}