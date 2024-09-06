  export interface AnswerOptionDto {
    optionValue: string;
    nextQuestionId?: number; 
  }

  export interface QuestionDto {
    id: number;                          
    question: string;                    
    serialNumber: number;                
    responseType: string;               
    answerOptions: AnswerOptionDto[];    
    dataType: string;                   
    constraint: string;                  
    constraintValue: string;             
    warningMessage: string;           
    required: boolean;
    nextQuestionId: string;
    userId:any;
  }
  
