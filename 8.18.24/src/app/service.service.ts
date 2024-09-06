import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map, Observable, of, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ServiceService {

 
  private apiUrl = 'https://localhost:7239/api/Form/GetAllForms';
  private baseUrl = 'https://localhost:7239/api/Form';
  private sectionUrl = 'https://localhost:7239/api/Section';
  private questionUrl = 'https://localhost:7239/api/Question';
  private ab='https://localhost:7239/api/Form/GetFormDetails';
  private response='https://localhost:7239/api';
  
  constructor(private http: HttpClient) { }

  getAllForm(): Observable<any> {
    const token = localStorage.getItem('authToken'); 
    let headers = new HttpHeaders();

    
    if (token) {
      headers = headers.set('Authorization', `Bearer ${token}`);
    }

    return this.http.get<any>(this.apiUrl, { headers }); 
  }

  createForm(formData: any): Observable<any> {
    return this.http.post(this.baseUrl + '/CreateForm', formData, { responseType: 'text' });
  }

 
  associateSectionsWithForm(sectionAssociations: any): Observable<any> {
    return this.http.post<any>(`${this.sectionUrl}/CreateSection`, sectionAssociations);
  }

  getAllQuestions(): Observable<Question[]> { 
    return this.http.get<Question[]>(this.questionUrl).pipe(
      catchError(error => {
        console.error('Error fetching questions:', error);
        return throwError(error);
      })
    );
  }

  

  getFormDetailsByFormId(formId: number): Observable<FormDetailsDto> {
    return this.http.get<FormDetailsDto>(`${this.ab}/${formId}`);
  }



  getQuestionById(id:number):Observable<Question>{
    return this.http.get<Question>(`${this.questionUrl}/${id}`)
  }

  MapSectionToQuestion(questionID: number, sectionId: any): Observable<any> {

    const obj = {
      questionId: questionID,
      sectionId: sectionId,
    };
    console.log('Sending payload to MapSectionToQuestion:', obj);
    return this.http.post<any>(`${this.questionUrl}/MapQuestionToSection`, obj).pipe(
      catchError(error => {
        console.error('Error in MapSectionToQuestion API call:', error);
        return throwError(error);
      })
    );
  }
  MapSectionToQuestionDelete(sectionId: number, questionID: number ):Observable<any>{
    return this.http.delete(`${this.questionUrl}/MapQuestionToSection/${sectionId}/${questionID}`);
  }
  

  updateForm(id: number, formData: any): Observable<any> {
    const url = `${this.baseUrl}/UpdateForm/${id}`;
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });

    return this.http.put(url, formData, { headers }).pipe(
      catchError(this.handleError('updateForm', []))
    );
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(`${operation} failed: ${error.message}`);
      return of(result as T);
    };
  }

  deleteForm(id: number) {
    return this.http.delete(`https://localhost:7239/api/Form/DeleteForm/${id}`);
}

  addResponse(payload: any): Observable<any> {
    return this.http.post<any>(`${this.response}/Response`, payload);
  }

  getNextQuestion(nextQuestionId: number): Observable<QuestionDetailDto> {
    return this.http.get<QuestionDetailDto>(`${this.questionUrl}/${nextQuestionId}`);
  }

  getFormDetailsById(formId: number): Observable<FormResponse> {
    debugger
    return this.http.get<FormResponse>(`${this.baseUrl}/GetFormById_nextQuestion/${formId}`).pipe(
      catchError(error => {
        console.error('Error fetching form details by ID:', error);
        return throwError(error); 
      })
    );
  }

  getAnswerType():Observable<any>{
    return this.http.get<any>(`${this.response}/AnswerType`)
  }
}


export interface FormResponse {
  success: boolean;
  form: FormDetailsDto;
  link: string;
}


export interface Question {
  id:any;
  question: string;
  serialNumber: number;
  responseType: string;
  answerOptions: string[];
  nextQuestionId: number;
  dataType: string;
  constraint: string;
  constraintValue: string;
  warningMessage: string;
}


export interface AnswerOptionDto {
  id: number;
  optionValue: string;
  answerType: string; 
}

export interface QuestionDetailDto {
  id: number;
  question: string; 
  slno: number;
  dataType: string; 
  constraint: string;
  constraintValue: string;
  warningMessage: string;
  active: boolean;
  answerOptions: AnswerOptionDto[];
  answerTypeId: number;
  responseType: string;
  sectionId:number;
  questionText:any;
  answerType:string;
  required:boolean;
}

export interface SectionDetailDto {
  id: number;
  sectionName: string; 
  description: string; 
  slno: number;
  currentQuestion?: QuestionDetailDto; 
  active: boolean;
  
  questions: QuestionDetailDto[];
}

export interface FormDetailsDto {
  formName:string;
  id: number;
  name: string; 
  description: string; 
  active: boolean;
  sections: SectionDetailDto[];
  version:number;
}

export interface AnswerOptionDto {
  id: number;
  optionValue: string; 
  answerTypeId: number;
  nextQuestionId: number;
  answerType:string;
}
