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

  getAll(): Observable<Section[]> {
    return this.http.get<Section[]>(this.apiUrl);
  }

  getById(id: number): Observable<Section> {
    return this.http.get<Section>(`${this.apiUrl}/${id}`);
  }

  create(section: Section): Observable<Section> {
    return this.http.post<Section>(this.apiUrl, section);
  }

  update(section: Section): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${section.id}`, section);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  previewCsv(file: File): Observable<SectionImportPreviewResultDto> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post<SectionImportPreviewResultDto>(`${environment.apiUrl}/section-import/preview`, formData);
  }

  insertAfter(afterOrder: number, section: Section): Observable<Section> {
    return this.http.post<Section>(`${this.apiUrl}/insert-after/${afterOrder}`, section);
  }

  exportCsv(includeId: boolean = false): Observable<Blob> {
    return this.http.get(`${environment.apiUrl}/section-export?includeId=${includeId}`, { responseType: 'blob' });
  }
  importSections(sections: any[]): Observable<any> {
    return this.http.post(`${environment.apiUrl}/section-import`, sections);
  }

}