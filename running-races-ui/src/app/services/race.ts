import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { RaceSearchModel } from '../models/race-search.model';
import { Observable } from 'rxjs';
import { Race } from '../models/race.model';
import { PagedResult } from '../models/paged-result.model';

@Injectable({
  providedIn: 'root'
})
export class RaceService {
  private http = inject(HttpClient);
  private apiUrl = 'https://localhost:7156/api/races';

  getRaces(endpoint: 'public' | 'admin', searchModel: RaceSearchModel): Observable<PagedResult<Race>> {

    let params = new HttpParams();

    if (searchModel.searchTerm) {
      params = params.set('searchTerm', searchModel.searchTerm);
    }
    if (searchModel.searchField) {
      params = params.set('searchField', searchModel.searchField);
    }
    if (searchModel.sortBy) {
      params = params.set('sortBy', searchModel.sortBy);
    }
    if (searchModel.sortDirection) {
      params = params.set('sortDirection', searchModel.sortDirection);
    }
    if (searchModel.isActive !== undefined) {
      params = params.set('isActive', searchModel.isActive.toString());
    }
    if (searchModel.page) {
      params = params.set('page', searchModel.page.toString());
    }
    if (searchModel.pageSize) {
      params = params.set('pageSize', searchModel.pageSize.toString());
    }

    return this.http.get<PagedResult<Race>>(this.apiUrl + `/${endpoint}`, { params });
  }

  createRace(race: Race): Observable<Race> {
    return this.http.post<Race>(this.apiUrl, race);
  }

  deleteRace(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  getRaceById(id: string): Observable<Race> {
    return this.http.get<Race>(`${this.apiUrl}/${id}`);
  }

  updateRace(id: string, race: Race): Observable<Race> {
    return this.http.put<Race>(`${this.apiUrl}/${id}`, race);
  }

  restoreRace(id: string): Observable<void> {
    return this.http.patch<void>(`${this.apiUrl}/${id}/restore`, {});
  }
}