import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomePageComponent } from './home-page/home-page.component';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptors } from '@angular/common/http';
import { CreateEditNewsComponent } from './create-edit-news/create-edit-news.component';
import { FormsModule } from '@angular/forms';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { DatePipe } from '@angular/common';
import { ModalDialogComponent } from './modal-dialog/modal-dialog.component';
import { AppHeaderComponent } from './app-header/app-header.component';
import { AppFooterComponent } from './app-footer/app-footer.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { SpinnerComponent } from './spinner/spinner.component'; // Import NgbModule
import { httpInterceptorInterceptor } from './http-interceptor.interceptor';
import { TimeagoPipe } from './pipes/timeago.pipe';

@NgModule({
  declarations: [
    AppComponent,
    HomePageComponent,
    CreateEditNewsComponent,
    ModalDialogComponent,
    AppHeaderComponent,
    AppFooterComponent,
    SpinnerComponent,
    TimeagoPipe
  ],
  imports: [
    BrowserModule,
    NgbModule,
    AppRoutingModule,
    FormsModule,
    FontAwesomeModule
  ],
  providers: [provideHttpClient(withInterceptors([httpInterceptorInterceptor])),DatePipe],
  bootstrap: [AppComponent]
})
export class AppModule { }
