import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { SectionDto } from '../section-dto.model';

@Injectable({
  providedIn: 'root'
})
export class SectionService {
  private apiUrl = 'https://localhost:7239/api/Section'

  constructor(private http: HttpClient) { }


  createSection(sectionDto: SectionDto): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/CreateSection`, sectionDto)
      .pipe(catchError(this.handleError));
  }

  updateSection(id: number, sectionDto: SectionDto): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/UpdateSection/${id}`, sectionDto)
      .pipe(catchError(this.handleError));
  }

  deleteSection(id: number | null): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/DeleteSection/${id}`)
      .pipe(catchError(this.handleError));
  }

  private handleError(error: any): Observable<never> {
    console.error('An error occurred:', error);
    return throwError(error.message || 'Server error');
  }
}
