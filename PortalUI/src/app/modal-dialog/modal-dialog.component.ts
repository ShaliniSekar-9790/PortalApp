import { Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewEncapsulation } from '@angular/core';
import { ModalInfo } from '../model/modalInfo';
import { ModalserviceService } from '../service/modalservice.service';

@Component({
  selector: 'app-modal-dialog',
  templateUrl: './modal-dialog.component.html',
  styleUrl: './modal-dialog.component.css',
  encapsulation: ViewEncapsulation.None
  })
export class ModalDialogComponent implements OnInit {

  @Input() modalInfo: ModalInfo = new ModalInfo;
  @Output() btnConfirmEvent = new EventEmitter<string>();

  message: any;
  constructor(private modalService: ModalserviceService) { }

  ngOnInit(): void {
    this.modalService.getMessage().subscribe(message => {
      this.message = message;
    });

  }
 
}
