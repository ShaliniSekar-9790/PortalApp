import { Component } from '@angular/core';
import { NewsPortalService } from './service/news-portal.service';
import { Subject } from 'rxjs/internal/Subject';
import { Observable } from 'rxjs/internal/Observable';
import { INewsPagedResponse } from './model/interfaces/INewsPagedResponse';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {

}
