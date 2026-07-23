import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { WayPoint as WayPointModel } from '../models/ub.models';


@Injectable({
  providedIn: 'root'
})
export class WayPoint {
  private http = inject(HttpClient);
  private apiUrl = 'https://localhost:7156/api/waypoint';

  getAll(): Observable<WayPointModel[]> {
    return this.http.get<WayPointModel[]>(this.apiUrl);
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