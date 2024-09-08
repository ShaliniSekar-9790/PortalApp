import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router'
import { NewsPortalService } from '../service/news-portal.service';
import { News } from '../model/News';
import { ICategory } from '../model/interfaces/Icategory';
import { DatePipe } from '@angular/common';
import { Category } from '../model/Category';

@Component({
  selector: 'app-create-edit-news',
  templateUrl: './create-edit-news.component.html',
  styleUrl: './create-edit-news.component.css'
})
export class CreateEditNewsComponent implements OnInit{
  
  newsData = new News();
  categoriesData?: ICategory[];
  id! : number;
  title : string = "Create News";
  customcategory: string = "";
  entercategory: string = "Entercategory";


  constructor(private route: ActivatedRoute,
    private router: Router,
    private newsPortalService: NewsPortalService, private datePipe: DatePipe) {
  }

  ngOnInit(): void {
  this.newsData.category = new Category();
   this.route.queryParams.subscribe(params => {
      this.id = params['id'];
    });   
    this. loadCategory();
    if(this.id) {
      this.loadNews();
      this.title = "Edit News";
    } else {
      this.newsData.category.categoryName = this.entercategory;
    }
  }

  loadCategory() : void {
    this.newsPortalService.getCategories().subscribe({
      next: (response:any) => {
        this.categoriesData = response.data;
        console.log('Category received:', this.categoriesData);
      },
      error: (error: any) => {
        console.error('Error occurred:', error);
      }
    })
  }

  loadNews() {
    this.newsPortalService.getNewsById(this.id).subscribe({
      next: (response:any) => {
        if(response.data) {
          this.newsData = response.data;
          this.newsData.category.categoryName = response.data.category.categoryName;
        } 
        console.log('NewsData received:', this.newsData);
      },
      error: (error: any) => {
        console.error('Error occurred:', error);
      }
    })
  }

  onOptionsSelected(value:string) {
    this.newsData.category.categoryName = value;
    if(value != this.entercategory) {
      this.customcategory = "";
    } 
  }

  onSubmit(form: any) {
    if (form.invalid) {
      return;
    }
   
    if (this.id == null) {
      if(this.customcategory.length > 0) {
        this.newsData.category.categoryName = this.customcategory;
      }
     console.log(this.newsData);
      this.newsPortalService.createNews(this.newsData).subscribe({
        next: async (response) => {
          this.router.navigate(['/home']);
        },
        error: (error) => {
        }
      });
    }
   else {
      this.newsPortalService.updateNews(this.newsData).subscribe({
        next: async (response) => {
          this.router.navigate(['/home']);
        },
        error: (error) => {
        }
     });
    }
  }

  onClose(event: Event) : void {
    event.preventDefault();
    this.router.navigate(['/home']);
  }

}
