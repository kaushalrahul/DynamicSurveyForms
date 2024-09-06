import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class QuestionAnswerService {
  
  private questionApiUrl = 'https://localhost:7239/api/Question';
  private answerOptionApiUrl = 'https://localhost:7239/api/AnswerOption';

  constructor(private http: HttpClient) {}

  fetchQuestionById(questionId: number): Observable<any> {
    return this.http.get<any>(`${this.questionApiUrl}/${questionId}`);
  }

  fetchAnswerOptionById(answerOptionId: number): Observable<any> {
    return this.http.get<any>(`${this.answerOptionApiUrl}/${answerOptionId}`);
  }
}
