import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { Subject } from 'rxjs/internal/Subject';

@Injectable({
  providedIn: 'root'
})
export class LoaderService {


  constructor() { }

  private loadSubject = new Subject<boolean>();

  setLoading(loading: boolean) {
   this.loadSubject.next(loading);
  }

  getLoading(): Observable<boolean> {
    return this.loadSubject.asObservable();
  }
}