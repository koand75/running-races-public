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
  private apiUrl = `${environment.apiUrl}/section`;

  private getApiUrl(raceId: string): string {
    return `${environment.apiUrl}/race/${raceId}/section`;
  }

  getAll(raceId: string): Observable<Section[]> {
    return this.http.get<Section[]>(this.getApiUrl(raceId));
  }

  getById(raceId: string, id: number): Observable<Section> {
    return this.http.get<Section>(`${this.getApiUrl(raceId)}/${id}`);
  }

  create(raceId: string, section: Section): Observable<Section> {
    return this.http.post<Section>(this.getApiUrl(raceId), section);
  }

  update(raceId: string, section: Section): Observable<void> {
    return this.http.put<void>(`${this.getApiUrl(raceId)}/${section.id}`, section);
  }

  delete(raceId: string, id: number): Observable<void> {
    return this.http.delete<void>(`${this.getApiUrl(raceId)}/${id}`);
  }

  previewCsv(raceId: string, file: File): Observable<SectionImportPreviewResultDto> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post<SectionImportPreviewResultDto>(`${this.getApiUrl(raceId)}/section-import/preview`, formData);
  }

  insertAfter(raceId: string, afterOrder: number, section: Section): Observable<Section> {
    return this.http.post<Section>(`${this.getApiUrl(raceId)}/insert-after/${afterOrder}`, section);
  }

  exportCsv(raceId: string, includeId: boolean = false): Observable<Blob> {
    return this.http.get(`${this.getApiUrl(raceId)}/section-export?includeId=${includeId}`, { responseType: 'blob' });
  }
  importSections(raceId: string, sections: any[]): Observable<any> {
    return this.http.post(`${this.getApiUrl(raceId)}/section-import`, sections);
  }
}