import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { LoaderService } from '../service/loader.service';

@Component({
  selector: 'app-spinner',
  templateUrl: './spinner.component.html',
  styleUrls: ['./spinner.component.css'],
  encapsulation: ViewEncapsulation.ShadowDom
})
export class SpinnerComponent implements OnInit {
  constructor(public loader: LoaderService) { 

  }

  loading:boolean = false;

  ngOnInit(): void {
    this.loader.getLoading().subscribe(load => {
      console.log("Load value from spinner", load);
      this.loading = load;
    });
  }
}