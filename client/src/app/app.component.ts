import { HttpClient } from "@angular/common/http";
import { Component, OnInit } from "@angular/core";

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  styleUrls: ['./app.scss'],  // fixed: it should be `styleUrls` (plural)
  standalone: false
})
export class App implements OnInit {
 title = 'client';

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
   this.http.get('http://localhost:8010/Catalog/GetAllProducts').subscribe({
     next: (response) => {
       console.log('Response from backend:', response);
     },
     error: (error) => {
       console.error('Error fetching data from backend:', error);
     }
   });
  }
}
