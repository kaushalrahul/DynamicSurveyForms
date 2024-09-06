import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class JwtService {

  constructor() { }

  getUserIdFromToken(): number | null {
    const token = localStorage.getItem('authToken');
    
    if (token) {
      const parts = token.split('.');
      if (parts.length !== 3) {
        console.error('Invalid token structure');
        return null;
      }
      const payload = parts[1]; 
      const decodedPayload = JSON.parse(this.base64UrlDecode(payload));
      return decodedPayload.UserID || null;  
    }
    return null;
  }

  private base64UrlDecode(str: string): string {
    str = str.replace(/-/g, '+').replace(/_/g, '/');
    while (str.length % 4) {
      str += '=';
    }
    return atob(str);  
  }
}
