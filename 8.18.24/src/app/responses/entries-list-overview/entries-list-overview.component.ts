import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { forkJoin, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ResponseService } from '../../services/response.service';
import { QuestionAnswerService } from '../../services/question-answer.service';
import { SidebarService } from '../../sidebar.service';

interface QuestionAnswerPair {
  question: string;
  answer: string;
}

@Component({
  selector: 'app-entries-list-overview',
  templateUrl: './entries-list-overview.component.html',
  styleUrls: ['./entries-list-overview.component.css']
})
export class EntriesListOverviewComponent {
  questionAnswerPairs: QuestionAnswerPair[] = [];
  id: number | null = null;
  email: string | null = null;
  title: string | null = null;
  isSidebarCollapsed = false;
  constructor(
    private responseService: ResponseService,
    private route: ActivatedRoute,
    private questionAnswerService: QuestionAnswerService,
    private sidebarService: SidebarService,
  ) {
    this.sidebarService.sidebarCollapsed$.subscribe(
      (collapsed) => (this.isSidebarCollapsed = collapsed)
    );
  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.fetchResponseById(+id);
    }
  }


//   fetchResponseById(id: number): void {
//   this.responseService.getResponseById(id).subscribe({
//     next: (response) => {
//       if (response && response.response) {
//         const responseData = response as any; // Type assertion for API response

//         this.title = responseData.title || 'No Title';
//         this.id = responseData.id;
//         this.email = responseData.email || 'No Email';

//         // Parse the response field, where the first key is the sectionID and the inner keys are questionID
//         let parsedResponse: { [sectionId: string]: { [questionId: string]: string } };
//         try {
//           parsedResponse = JSON.parse(responseData.response);
//         } catch (error) {
//           console.error('Error parsing response data:', error);
//           return; // Early return if parsing fails
//         }

//         // Create an array of Observables for the section-question-answer pairs
//         const sectionQuestionAnswerRequests: Observable<SectionQuestionAnswerPair>[] = [];

//         // Iterate over sections and questions
//         for (const sectionID of Object.keys(parsedResponse)) {
//           const questions = parsedResponse[sectionID];

//           // For each sectionID, fetch the section name
//           sectionQuestionAnswerRequests.push(
//             this.getSectionWithQuestionAnswers(+sectionID, questions)
//           );
//         }

//         // Use forkJoin to handle multiple observables
//         forkJoin(sectionQuestionAnswerRequests).subscribe({
//           next: (results) => {
//             this.sectionQuestionAnswerPairs = results; // Store the section-question-answer pairs
//           },
//           error: (error) => {
//             console.error('Error fetching sections, questions, and answers:', error);
//           }
//         });
//       } else {
//         console.warn('No responses data found');
//       }
//     },
//     error: (error) => {
//       console.error('Error fetching response by ID:', error);
//     }
//   });
// }

// // This method now fetches the section name and its associated question-answer pairs
// getSectionWithQuestionAnswers(sectionId: number, questions: { [questionId: string]: string }): Observable<SectionQuestionAnswerPair> {
//   return this.sectionService.fetchSectionById(sectionId).pipe(
//     switchMap((section) => {
//       const questionAnswerRequests: Observable<QuestionAnswerPair>[] = [];

//       // Iterate over the questions in the section
//       for (const questionID of Object.keys(questions)) {
//         const answerValue = questions[questionID];
//         questionAnswerRequests.push(this.getQuestionAnswerPair(+questionID, answerValue));
//       }

//       // Combine section name with question-answer pairs
//       return forkJoin(questionAnswerRequests).pipe(
//         map((questionAnswerPairs) => ({
//           sectionName: section?.name || 'No Section Name',
//           questionAnswerPairs: questionAnswerPairs
//         }))
//       );
//     })
//   );
// }

// // This method fetches the question and pairs it with the provided answerValue
// getQuestionAnswerPair(questionId: number, answerValue: string): Observable<QuestionAnswerPair> {
//   return this.questionAnswerService.fetchQuestionById(questionId).pipe(
//     map((question) => ({
//       question: question?.question || 'No Question',
//       answer: answerValue || 'No Answer'
//     }))
//   );
// }



  fetchResponseById(id: number): void {
    this.responseService.getResponseById(id).subscribe({
      next: (response) => {
        if (response && response.response) {
          const responseData = response as any; // Type assertion for API response
  
          this.title = responseData.title || 'No Title';
          this.id = responseData.id;
          this.email = responseData.email || 'No Email';
  
          // Parse the response field, where the first key is the sectionID and the inner keys are questionID
          let parsedResponse: { [sectionId: string]: { [questionId: string]: string } };
          try {
            parsedResponse = JSON.parse(responseData.response);
          } catch (error) {
            console.error('Error parsing response data:', error);
            return; // Early return if parsing fails
          }
  
          // Create an array of Observables for the question-answer pairs
          const questionAnswerRequests: Observable<QuestionAnswerPair>[] = [];
  
          // Iterate over sections and questions
          for (const sectionID of Object.keys(parsedResponse)) {
            const questions = parsedResponse[sectionID];
            for (const questionID of Object.keys(questions)) {
              const answerValue = questions[questionID];
  
              // Push the Observable request to the array for each questionID and answer value
              questionAnswerRequests.push(this.getQuestionAnswerPair(+questionID, answerValue));
            }
          }
  
          // Use forkJoin to handle multiple observables
          forkJoin(questionAnswerRequests).subscribe({
            next: (results) => {
              this.questionAnswerPairs = results;
            },
            error: (error) => {
              console.error('Error fetching questions and answers:', error);
            }
          });
        } else {
          console.warn('No responses data found');
        }
      },
      error: (error) => {
        console.error('Error fetching response by ID:', error);
      }
    });
  }
  
  // This method now fetches the question and pairs it with the provided answerValue
  getQuestionAnswerPair(questionId: number, answerValue: string): Observable<QuestionAnswerPair> {
    return this.questionAnswerService.fetchQuestionById(questionId).pipe(
      map((question) => ({
        question: question?.question || 'No Question',
        answer: answerValue || 'No Answer'
      }))
    );
  }
  
  
  // fetchResponseById(id: number): void {
  //   this.responseService.getResponseById(id).subscribe({
  //     next: (response) => {
  //       if (response && response.response) {
  //         const responseData = response as any; // Type assertion for API response

  //         this.title = responseData.title || 'No Title';
  //         this.id = responseData.id;
  //         this.email = responseData.email || 'No Email';

  //         // Parse the response field
  //         let parsedResponse: { QuestionID: number; AnswerOptionID: number }[] = [];
  //         try {
  //           parsedResponse = JSON.parse(responseData.response);
  //         } catch (error) {
  //           console.error('Error parsing response data:', error);
  //           return; // Early return if parsing fails
  //         }

  //         // Create an array of Observables for the question-answer pairs
  //         const questionAnswerRequests: Observable<QuestionAnswerPair>[] = parsedResponse.map((item) =>
  //           this.getQuestionAnswerPair(item.QuestionID, item.AnswerOptionID)
  //         );

  //         // Use forkJoin to handle multiple observables
  //         forkJoin(questionAnswerRequests).subscribe({
  //           next: (results) => {
  //             this.questionAnswerPairs = results;
  //           },
  //           error: (error) => {
  //             console.error('Error fetching questions and answers:', error);
  //           }
  //         });
  //       } else {
  //         console.warn('No responses data found');
  //       }
  //     },
  //     error: (error) => {
  //       console.error('Error fetching response by ID:', error);
  //     }
  //   });
  // }

  // getQuestionAnswerPair(questionId: number, answerOptionId: number): Observable<QuestionAnswerPair> {
  //   return forkJoin({
  //     question: this.questionAnswerService.fetchQuestionById(questionId),
  //     answer: this.questionAnswerService.fetchAnswerOptionById(answerOptionId)
  //   }).pipe(
  //     map(({ question, answer }) => ({
  //       question: question?.question || 'No Question',
  //       answer: answer?.optionValue || 'No Answer'
  //     }))
  //   );
  // }
}
