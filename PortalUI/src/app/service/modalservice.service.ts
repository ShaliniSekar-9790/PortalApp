import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ModalserviceService {
    private subject = new Subject<any>();
  
    confirmThis(message: string, submit: () => void, close: () => void): any {
      this.setConfirmation(message, submit, close);
    }
  
    setConfirmation(message: string, submit: () => void, close: () => void): any {
      const that = this;
      this.subject.next({
        type: 'confirm',
        text: message,
        submit(): any {
          that.subject.next(this.subject); 
          submit();
        },
        close(): any {
          that.subject.next(this.subject);
          close();
        }
      });
  
    }
  
    getMessage(): Observable<any> {
      return this.subject.asObservable();
    }
}
