import { NgFor } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit {
  title = 'client';
  http = inject(HttpClient);
  users: any;

  ngOnInit(): void {
    this.http.get('http://localhost:5003/api/users').subscribe({
      next: (response) => console.log((this.users = response)),
      error: (error) => console.log(error),
      complete: () => 'Request has been completed',
    });
  }
}
