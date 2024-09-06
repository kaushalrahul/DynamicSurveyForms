import { ChangeDetectorRef, Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthLayoutComponent } from './auth-layout/auth-layout.component';
import { MainLayoutComponent } from './main-layout/main-layout.component';
import { CommonModule } from '@angular/common';
import { SignupComponent } from './signup/signup.component';



@Component({
  selector: 'app-root',
  template: `
    <router-outlet></router-outlet>
  `,
  standalone: true,
  imports: [RouterModule, CommonModule, AuthLayoutComponent, MainLayoutComponent,SignupComponent]
})
export class AppComponent {}
