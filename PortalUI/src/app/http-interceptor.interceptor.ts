import { HttpInterceptorFn } from '@angular/common/http';
import { LoaderService } from './service/loader.service';
import { finalize } from 'rxjs/internal/operators/finalize';
import { inject } from '@angular/core';



export const httpInterceptorInterceptor: HttpInterceptorFn = (req, next) => {
  var loadingService = inject(LoaderService);
  loadingService.setLoading(true);
  console.log("loading turned on");
  return next(req).pipe(
    finalize(() => {
      console.log("loading turned off");
        loadingService.setLoading(false);
      }
  ))
}