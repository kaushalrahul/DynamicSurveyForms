import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css'],
  standalone: true,
  imports: [CommonModule,FormsModule]
})
export class SignupComponent {

  email: string = '';  
  password: string = '';  
  errorMessage: string = '';  

  constructor(private http: HttpClient,private route:Router,private toaster:ToastrService) {}

  // Function to handle login form submission
  login() {
    const loginData = {
      email: this.email,
      password: this.password
    };
   
    
    this.http.post<any>('https://localhost:7239/api/Login', loginData).subscribe({
      next: (response) => {
        
        console.log('Login successful!', response);
        localStorage.setItem('authToken', response.token);
        this.route.navigate(['app/dashboard']);
        this.toaster.success(" login successfully")
      },
      error: (error) => {
        this.errorMessage = 'Login failed! Invalid email or password.';
        console.error('Login error', error);
        this.toaster.error('invalid credentials');
      }
    });
  }




}
