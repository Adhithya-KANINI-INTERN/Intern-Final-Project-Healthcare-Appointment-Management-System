import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoadingService } from '../services/loading.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-loading',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './loading.component.html',
  styleUrl: './loading.component.css'
})
export class LoadingComponent implements OnInit{

  loadings$: Observable<boolean> | undefined;

  constructor(private loadingService: LoadingService) {}

  ngOnInit(): void {
    this.loadings$ = this.loadingService.loading$;
  }

}