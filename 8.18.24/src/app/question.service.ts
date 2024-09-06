import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { QuestionDto } from './QuestionDto';

@Injectable({
  providedIn: 'root'
})
export class QuestionService {

  private apiUrl = 'https://localhost:7239/api/Question'; 

  constructor(private http: HttpClient) { }

  getAllQuestions(): Observable<QuestionDto[]> {
    const authToken = localStorage.getItem('authToken'); 
    
  
  
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${authToken}`,
      'Content-Type': 'application/json'
    });
  
    return this.http.get<QuestionDto[]>(`${this.apiUrl}`, { headers });
  }
  

  getQuestionById(id: number): Observable<QuestionDto> {
    return this.http.get<QuestionDto>(`${this.apiUrl}/${id}`);
  }

  createQuestion(dto: QuestionDto): Observable<QuestionDto> {
    return this.http.post<QuestionDto>(`${this.apiUrl}/CreateQuestion`, dto, {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    });
  }

  updateQuestion(id: number, dto: QuestionDto): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, dto, {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    });
  }

  deleteQuestion(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  mapQuestionToSection(sectionId: number, questionId: number): Observable<void> {
    const dto = { SectionId: sectionId, QuestionId: questionId };
    return this.http.post<void>(`${this.apiUrl}/MapQuestionToSection`, dto, {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    });
  }

getAnswerTypes(): Observable<any> {
  return this.http.get<any>(`${this.apiUrl}/GetResponseTypes`);
}

getAllQuestion() : Observable<any> {
  return this.http.get<any>(`${this.apiUrl}`);
}

}


