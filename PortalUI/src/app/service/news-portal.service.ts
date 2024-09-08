import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import { News } from '../model/News';
import { ICategory } from '../model/interfaces/Icategory';
import { InewsInfo } from '../model/interfaces/InewsInfo';
import { INewsPagedResponse } from '../model/interfaces/INewsPagedResponse';

@Injectable({
  providedIn: 'root'
})
export class NewsPortalService {
  
  private apiUrl = 'http://localhost:5076';
  
  constructor(private http: HttpClient) {
  }

  public searchResponsesubject = new Subject<any>;

  getSearchResponse(): Observable<any> {
    return this.searchResponsesubject.asObservable();
  }

  getNewsInfoByPagination(currentPage: number, pageSize: number): Observable<INewsPagedResponse[]> {
    const params = new HttpParams()
      .set('PageNumber', currentPage)
      .set('PageSize', pageSize);
    return this.http.get<INewsPagedResponse[]>(`${this.apiUrl}/api/News/GetAllByPagination`,{params: params});
  }

  getNewsInfoBySearch(searchTerm: String): Observable<INewsPagedResponse[]> {
    const params = new HttpParams()
      .set('SearchTerm', searchTerm.toString())
    return this.http.get<INewsPagedResponse[]>(`${this.apiUrl}/api/News/GetAllBySearchTerm`,{params: params});
  }

  getNewsById(id: number): Observable<InewsInfo> {
    return this.http.get<InewsInfo>(`${this.apiUrl}/api/News/GetNewsById/${id}`);
  }

  createNews(news: News): Observable<News> {
    const headers = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');
    return this.http.post<News>(`${this.apiUrl}/api/News/createNews`, news);
  }

  updateNews(newsInfo: News): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/api/News/UpdateNews`, newsInfo);
  }

  deleteNews(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/api/News/DeleteNewsById/${id}`);
  }

  getCategories(): Observable<ICategory[]> {
    return this.http.get<ICategory[]>(`${this.apiUrl}/getAllCategories`);
  }


}
