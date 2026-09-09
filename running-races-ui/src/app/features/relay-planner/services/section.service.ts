import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Section, SectionImportPreviewResultDto } from '../models/relay-planner.models';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SectionService {
  private http = inject(HttpClient);

  private getApiUrl(raceId: string, categoryId: number): string {
    return `${environment.apiUrl}/race/${raceId}/category/${categoryId}/section`;
  }

  getAll(raceId: string, categoryId: number): Observable<Section[]> {
    return this.http.get<Section[]>(this.getApiUrl(raceId, categoryId));
  }

  getById(raceId: string, categoryId: number, id: number): Observable<Section> {
    return this.http.get<Section>(`${this.getApiUrl(raceId, categoryId)}/${id}`);
  }

  create(raceId: string, categoryId: number, section: Section): Observable<Section> {
    return this.http.post<Section>(this.getApiUrl(raceId, categoryId), section);
  }

  update(raceId: string, categoryId: number, section: Section): Observable<void> {
    return this.http.put<void>(`${this.getApiUrl(raceId,categoryId)}/${section.id}`, section);
  }

  delete(raceId: string, categoryId: number, id: number): Observable<void> {
    return this.http.delete<void>(`${this.getApiUrl(raceId, categoryId)}/${id}`);
  }

  previewCsv(raceId: string, categoryId: number, file: File): Observable<SectionImportPreviewResultDto> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post<SectionImportPreviewResultDto>(`${environment.apiUrl}/race/${raceId}/category/${categoryId}/section-import/preview`, formData);
  }

  insertAfter(raceId: string, categoryId: number, afterOrder: number, section: Section): Observable<Section> {
    return this.http.post<Section>(`${this.getApiUrl(raceId, categoryId)}/insert-after/${afterOrder}`, section);
  }

  exportCsv(raceId: string, categoryId: number, includeId: boolean = false): Observable<Blob> {
    return this.http.get(`${this.getApiUrl(raceId, categoryId)}/section-export?includeId=${includeId}`, { responseType: 'blob' });
  }
  importSections(raceId: string, categoryId: number, sections: any[]): Observable<any> {
    return this.http.post(`${environment.apiUrl}/race/${raceId}/category/${categoryId}/section-import`, sections);
  }
}