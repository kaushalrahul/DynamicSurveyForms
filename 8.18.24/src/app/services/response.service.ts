import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ResponseService {

  private apiUrl = 'https://localhost:7239/api/Response'; 

  constructor(private http: HttpClient) { }

  getResponseById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }

  getResponseIdsByFormId(formId: number): Observable<number[]> {
    return this.http.get<number[]>(`${this.apiUrl}/form/${formId}/responses`);
  }

  getResponseCountByFormId(formId: number): Observable<number> {
    return this.http.get<number>(`${this.apiUrl}/form/${formId}/responses/count`);
  }
}
