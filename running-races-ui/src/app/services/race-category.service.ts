import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RaceCategory } from '../models/race-category.model';
import { environment } from '../../environments/environment';
import { RaceCategoryDropdownDto } from '../models/race.model';

@Injectable({
    providedIn: 'root'
})
export class RaceCategoryService {
    private http = inject(HttpClient);

    getAll(raceId: string): Observable<RaceCategory[]> {
        return this.http.get<RaceCategory[]>(`${environment.apiUrl}/race/${raceId}/category`);
    }

    getRelayCategories(): Observable<RaceCategoryDropdownDto[]> {
        return this.http.get<RaceCategoryDropdownDto[]>(`${environment.apiUrl}/race-categories/relay`);
    }

    create(raceId: string, category: RaceCategory): Observable<RaceCategory> {
        return this.http.post<RaceCategory>(`${environment.apiUrl}/race/${raceId}/category`, category);
    }

    update(raceId: string, id: number, category: RaceCategory): Observable<RaceCategory> {
        return this.http.put<RaceCategory>(`${environment.apiUrl}/race/${raceId}/category/${id}`, category);
    }

    delete(raceId: string, id: number): Observable<void> {
        return this.http.delete<void>(`${environment.apiUrl}/race/${raceId}/category/${id}`);
    }
}