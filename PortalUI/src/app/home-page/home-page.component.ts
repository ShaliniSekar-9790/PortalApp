import { AfterViewInit, Component, ElementRef, OnInit, Renderer2, ViewChild } from '@angular/core';
import { NewsPortalService } from '../service/news-portal.service';
import { Router } from '@angular/router';
import { InewsInfo } from '../model/interfaces/InewsInfo';
import { INewsPagedResponse } from '../model/interfaces/INewsPagedResponse';
import { Subscription } from 'rxjs/internal/Subscription';
import { ModalInfo } from '../model/modalInfo';
import { ModalDismissReasons, NgbAlert, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ModalDialogComponent } from '../modal-dialog/modal-dialog.component';
import { Subject } from 'rxjs/internal/Subject';
import { debounceTime } from 'rxjs';
import { ModalserviceService } from '../service/modalservice.service';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.css',
  providers: [NewsPortalService]
})
export class HomePageComponent implements OnInit {
  
  subscription!: Subscription;
  newsData!: InewsInfo[];
  paginationResponse!: INewsPagedResponse;
  totalPages: number = 0;
  modalDialogInfo : ModalInfo = new ModalInfo();
  toBeDeletedNews : number = -1;
  confirmText: string = "";
  searchText: string = "";
  closeResult='';
  showDialog = false;
  private alertSubject = new Subject<string>();
  alertMessageType = 'success';
  alertMessage = '';
  @ViewChild('selfClosingAlert', { static: false }) selfClosingAlert?: NgbAlert;
  pageSize = 5;
  currentPage = 1;

  constructor(private newsPortalService: NewsPortalService, 
    private router: Router, private modalService: ModalserviceService) {}

  ngOnInit(): void {
    this.alertSubject.subscribe((message) => (this.alertMessage = message));
    this.alertSubject.pipe(debounceTime(5000)).subscribe(() => {
      if (this.selfClosingAlert) {
        this.selfClosingAlert.close();
      }
    });
    this.loadItems(this.currentPage, this.pageSize);
  }

  loadItems(currentPage: number = 1, pageSize: number =5) {
    // Example data, replace with your data source
    this.newsPortalService.getNewsInfoByPagination(currentPage, pageSize).subscribe({
      next: (data: any) => {
        if(data.data != null )this.setLoadData(data.data);
        console.log("Load Data by page");
      },
      error: (error: any) => {
        console.error('Error occurred:', error);
      }
    })
  }

  private setLoadData(data:any) {
    this.paginationResponse = data;
        this.newsData = this.paginationResponse.newsInfos;
        this.totalPages = this.paginationResponse.totalPages;
        console.log('News Data received:', this.paginationResponse);
  }

  updatePagination() {
   this.loadItems()
  }

  onPageChange(page: number) {
    if(this.paginationResponse.pageNumber != page) {
      this.currentPage = page;
        if(this.searchText?.length > 0) this.onSearch(this.searchText, page);   
        else this.loadItems(page, this.pageSize);
    }  
  }

  public showAlertMessage(message: string, alertType: string = "success") {
    this.alertMessageType = alertType;
    this.alertSubject.next(message);
  }

  onDelete(toBeDeletedId : number) {
      const that = this;
      this.modalService.confirmThis("Are you sure to delete?",
        function () {
          that.newsPortalService.deleteNews(toBeDeletedId).subscribe({
            next: async (response) => {
              that.showAlertMessage("Deleted succesfully.", "danger");
              that.loadItems(that.currentPage, that.pageSize);
            },
            error: (error) => {
              that.showAlertMessage(`failed to delete news ${error.detail}`);
            }
          });
        },
        function () {
        })
  }


  deleteNews(event: string) {
    console.log("Inside Delete");
    this.newsPortalService.deleteNews(this.toBeDeletedNews).subscribe({
      next: (data: any) => {
        this.loadItems();
      },
      error: (error: any) => {
        console.error('Error occurred:', error);
      }
    })  }

    onKeyup(event: KeyboardEvent) : void {
      if(this.searchText == null) this.searchText = "";
      console.log(this.searchText);
      this.onSearch(this.searchText, this.paginationResponse?.pageNumber);   
    }

    onSearch(searchText : string, page: number) : void {
      this.newsPortalService.getNewsInfoBySearch(this.searchText ,page ,this.pageSize).subscribe({
        next: (data: any) => {
          this.setLoadData(data);
        },
        error: (error: any) => {
          console.error('Error occurred:', error);
        }
      })  
    }

    onEdit(newsData: any) : void {
        this.router.navigate(['/updatenews'],  { queryParams: { id: newsData.news_Id } });
     }
}
