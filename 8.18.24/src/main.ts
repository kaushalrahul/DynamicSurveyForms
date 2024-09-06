import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { appConfig } from './app/app.config';

// Import Bootstrap CSS
import 'bootstrap/dist/css/bootstrap.min.css';
// Import Bootstrap JS
import 'bootstrap/dist/js/bootstrap.bundle.min.js'; 

bootstrapApplication(AppComponent, appConfig)
  .catch((err) => console.error(err));
