import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
@Injectable({
  providedIn: 'root'
})
export class AppService {
   

  private baseUri = 'api/calculation';

 
  constructor(private http: HttpClient) {
  }

  sum(input: any) {
    var requestHeader = { headers: new HttpHeaders({ 'Content-Type': 'application/json', 'No-Auth': 'False' }) };
    return this.http.post(this.baseUri, input, requestHeader);
  }
  getResults() {
    return this.http.get<any>(this.baseUri + "/get-all");
  }
}
