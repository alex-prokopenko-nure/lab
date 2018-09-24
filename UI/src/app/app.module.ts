import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatButtonModule, MatInputModule } from '@angular/material';

import { AppComponent } from './app.component';
import { API_BASE_URL, APIService } from './services/api.service';
import { environment } from '../environments/environment';
import { APPService } from './services/app.service';
import { HttpClientModule } from '../../node_modules/@angular/common/http';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    MatButtonModule,
    MatInputModule,
    FormsModule,
    HttpClientModule
  ],
  providers: [
    { provide: API_BASE_URL, useValue: environment.API_BASE_URL},
    APIService,
    APPService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
