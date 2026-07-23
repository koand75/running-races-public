import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Section } from '../models/ub.models';

@Injectable({
  providedIn: 'root'
})
export class SectionService {
  private http = inject(HttpClient); 
  private apiUrl = 'https://localhost:7156/api/section';

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

  importCsv(file: File): Observable<{ imported: number }> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post<{ imported: number }>('api/section-import', formData);
  }

  insertAfter(afterOrder: number, section: Section): Observable<Section> {
    return this.http.post<Section>(`${this.apiUrl}/insert-after/${afterOrder}`, section);
  }
}