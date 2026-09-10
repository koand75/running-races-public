import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { RaceCategoryTeamDto } from '../models/race.model';


@Injectable({ providedIn: 'root' })
export class RaceCategoryTeamService {
  private http = inject(HttpClient);

  getByCategory(raceId: string, categoryId: number): Observable<RaceCategoryTeamDto[]> {
    return this.http.get<RaceCategoryTeamDto[]>(
      `${environment.apiUrl}/race/${raceId}/category/${categoryId}/registration`
    );
  }

  create(raceId: string, categoryId: number, dto: RaceCategoryTeamDto): Observable<RaceCategoryTeamDto> {
    return this.http.post<RaceCategoryTeamDto>(
      `${environment.apiUrl}/race/${raceId}/category/${categoryId}/registration`, dto
    );
  }

  delete(raceId: string, categoryId: number, id: number): Observable<void> {
    return this.http.delete<void>(
      `${environment.apiUrl}/race/${raceId}/category/${categoryId}/registration/${id}`
    );
  }
}