import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-auth-layout',
  templateUrl: './auth-layout.component.html',
  styleUrls: ['./auth-layout.component.css'],
  standalone: true,
  imports: [CommonModule, RouterModule]
})
export class AuthLayoutComponent implements OnInit {
  ngOnInit() {
    console.log('AuthLayoutComponent initialized');
  }
}
