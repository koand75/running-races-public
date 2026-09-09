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
  private getApiUrl(raceId: string, categoryId: number): string {
    return `${environment.apiUrl}/race/${raceId}/category/${categoryId}/waypoint`;
  }

  getAll(raceId: string, categoryId: number): Observable<WayPointModel[]> {
    return this.http.get<PagedResult<WayPointModel>>(`${this.getApiUrl(raceId, categoryId)}?pageSize=1000`).pipe(
      map(result => result.items)
    );
  }

  create(raceId: string, categoryId: number, wayPoint: WayPointModel): Observable<WayPointModel> {
    return this.http.post<WayPointModel>(this.getApiUrl(raceId, categoryId), wayPoint);
  }

  update(raceId: string, categoryId: number, id: number, wayPoint: WayPointModel): Observable<WayPointModel> {
    return this.http.put<WayPointModel>(`${this.getApiUrl(raceId, categoryId)}/${id}`, wayPoint);
  }

  delete(raceId: string, categoryId: number, id: number): Observable<void> {
    return this.http.delete<void>(`${this.getApiUrl(raceId, categoryId)}/${id}`);
  }
}